import { ENTER } from '@angular/cdk/keycodes';
import {
    ChangeDetectionStrategy,
    ChangeDetectorRef,
    Component,
    OnInit,
} from '@angular/core';
import {
    AbstractControl,
    FormBuilder,
    FormGroup,
    Validators,
} from '@angular/forms';
import { MatChipInputEvent } from '@angular/material/chips';
import { ActivatedRoute, Data, Router } from '@angular/router';
import { UserService } from '@app/infrastructure/core/services/auth/user.service';
import { DocumentService } from '@app/infrastructure/core/services/documents/document.service';
import { FatawaService } from '@app/infrastructure/core/services/fatawa/fatawa.service';
import { FileService } from '@app/infrastructure/core/services/file.service';
import { FormService } from '@app/infrastructure/core/services/form.service';
import { GeneralSettingsService } from '@app/infrastructure/core/services/general-settings.service';
import { NotificationService } from '@app/infrastructure/core/services/notification.service';
import { TokenService } from '@app/infrastructure/core/services/token.service';
import { IChatMessageModel } from '@app/infrastructure/models/chat-message';
import { FileConfig } from '@app/infrastructure/models/file';
import { Message } from '@app/infrastructure/models/message';
import {
    FatawaModel,
    TagModel,
} from '@app/infrastructure/models/project/fatawa/FatawaModel';
import { GeneralSettingsModel } from '@app/infrastructure/models/project/general-settings';
import { UserModel } from '@app/infrastructure/models/project/UserModel';
import { IUploadedDocument } from '@app/infrastructure/models/uploaded-document';
import { Constants } from '@app/infrastructure/utils/constants';
import { RequiredValidation } from '@app/infrastructure/utils/validation/required-validation';
import { UploadValidation } from '@app/infrastructure/utils/validation/upload-validation';
import { FormConfig, FormField } from '@models/form/form';
import { IInputField, InputField } from '@models/form/input';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { catchError, map, mergeMap, tap } from 'rxjs/operators';

@Component({
    templateUrl: './fatawa-default-settings-editor.component.html',
    styleUrls: ['./fatawa-default-settings-editor.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FatawaDefaultSettingsEditorComponent implements OnInit {
    public fileConfig = new FileConfig({
        allowedFileExtensionList: [
            'jpg',
            'jpeg',
            'png',
            'bmp',
            'gif',
            'tif',
            'tiff',
            'eps',
            'svg',
        ],
        allowedMimeTypeList: ['image/*'],
        canUploadMultiple: false,
    });

    public validationMessageList: string[] = [];
    public formGroup: FormGroup;
    public formConfig = new FormConfig({
        formName: 'form',
        submit: null,
        controls: [
            new FormField<IInputField>({
                name: 'fileList',
                fieldType: 'input',
                fieldConfig: new InputField({
                    inputType: 'file',
                }),
                validation: [
                    RequiredValidation.required(),
                    UploadValidation.validateFiles(this.fileConfig),
                ],
            }),
        ],
    });

    public imageBase64: BehaviorSubject<string> = new BehaviorSubject('');
    readonly separatorKeysCodes: number[] = [ENTER];
    public isInProgress = false;
    tags: TagModel[] = [];
    message: Message;
    url: string = '';
    visible = true;
    selectable = true;
    removable = true;
    addOnBlur = true;
    id: string;
    editorForm: FormGroup;
    submitted = false;
    data: FatawaModel;
    fatawaTypes = [];
    assistants: UserModel[] = [];
    muftiList: UserModel[] = [];
    fatawaDepartmens = [];
    mathhabs = [];
    currentQuestion: string;
    isAdd: boolean = false;
    isEdit: boolean = false;
    currentUserID: Observable<number>;
    newMessage: IChatMessageModel;
    translators: string[];
    translatorsFiltered: string[];
    constructor(
        private formBuilder: FormBuilder,
        private fileService: FileService,
        private fatawaService: FatawaService,
        private userService: UserService,
        private router: Router,
        private formService: FormService,
        private documentService: DocumentService,
        private notify: NotificationService,
        private tokenService: TokenService,
        private generalSettingsService: GeneralSettingsService,
        private activatedroute: ActivatedRoute,
        private cdr: ChangeDetectorRef,
    ) {
        this.editorForm = this.formBuilder.group({
            assistantId: ['', [Validators.required]],
            name: ['', null],
            description: ['', null],
            fatawaTypeId: ['', null],
            fatawaDepartmentId: ['', null],
            fatawaMathhabId: ['', null],
            muftiId: ['', null],
            translatorName: ['', null],
            tags: ['', null],
            url: [''],
            id: Number,
        });
        this.fillAutoCompleteValues();

        this.editorForm.controls.translatorName.valueChanges.subscribe(
            (newValue) => {
                this.translatorsFiltered = this.getFilteredValues(newValue);
            },
        );
    }

    fillAutoCompleteValues(): void {
        this.fatawaService
            .getTranslatorAutoCompleteValues()
            .subscribe((values: string[]) => {
                this.translators = this.translatorsFiltered = values;
            });
    }

    getFilteredValues(search: string): string[] {
        if (!search || !this.translators) {
            return this.translators;
        }

        return this.translators.filter(
            (value) => value.toLowerCase().indexOf(search.toLowerCase()) === 0,
        );
    }

    get fatawaTypeId(): number {
        return this.editorForm.controls.fatawaTypeId.value;
    }

    get isSuperAdminUser(): Observable<boolean> {
        return this.tokenService.isSuperAdmin();
    }

    ngOnInit(): void {
        this.formGroup = this.buildForm();

        this.getFatawaTypes()
            .pipe(
                mergeMap((data) => {
                    this.fatawaTypes = data;
                    return this.getFatawaAssistantsList();
                }),
                mergeMap((data) => {
                    this.assistants = data;
                    return this.getFatawaMuftiList();
                }),
                mergeMap((data) => {
                    this.muftiList = data;
                    return this.getFatawaDepartmens();
                }),
                mergeMap((data) => {
                    this.fatawaDepartmens = data;
                    return this.getMathhabs();
                }),
                mergeMap((data) => {
                    this.mathhabs = data;
                    return this.activatedroute.data;
                }),
            )
            .subscribe((data: Data) => {
                if (data.editFatwa) {
                    this.isEdit = true;
                    this.fillFatawaObj(data.editFatwa);
                } else {
                    this.isAdd = true;
                    this.loadFatawaImage();
                }
            });
    }

    fillFatawaObj(fatawaModel: FatawaModel): void {
        this.editorForm.setValue({
            name: fatawaModel.name,
            description: fatawaModel.description,
            muftiId: fatawaModel.muftiId,
            translatorName: fatawaModel.translatorName,
            fatawaTypeId: fatawaModel.fatawaTypeId,
            fatawaDepartmentId: fatawaModel.fatawaDepartmentId,
            fatawaMathhabId: fatawaModel.fatawaMathhabId,
            id: fatawaModel.id,
            tags: fatawaModel.tags,
            url: fatawaModel.url,
        });

        this.url = fatawaModel.url;
        this.tags = fatawaModel.tags;

        this.loadFatawaImage();
    }

    getFatawaAssistantsList(): Observable<UserModel[]> {
        return this.userService.getAllAdminsAsync();
    }

    getFatawaMuftiList(): Observable<UserModel[]> {
        return this.userService.getAllSuperAdminsAsync();
    }

    getFatawaTypes(): Observable<any> {
        return this.fatawaService.getFatawaTypes();
    }

    getFatawaDepartmens(): Observable<any> {
        return this.fatawaService.getFatawaDeparments();
    }

    changeFatawaDepartmens(e): void {
        this.editorForm.controls.fatawaDepartmentId.setValue(e.target.value);
    }

    getMathhabs(): Observable<any> {
        return this.fatawaService.getFatawaMathhabs();
    }

    changeMathhabs(e): void {
        this.editorForm.controls.fatawaMathhabId.setValue(e.target.value);
    }

    get form() {
        return this.editorForm.controls;
    }

    submitFatawa(): void {
        this.isInProgress = true;
        this.submitted = true;

        if (this.editorForm.invalid) {
            return;
        }

        const observableMethod: Observable<any> = this.fileListFormControl.value
            ? this.uploadDocumentThenSave()
            : this.saveFatawa();

        observableMethod.subscribe((value) => {
            this.isInProgress = false;
            if (!value) {
                this.notify.showTranslateMessage('SaveFailed');
                return;
            }
            void this.router.navigate(['fatawa/fatawa-default-settings-list']);
        });
    }

    uploadDocumentThenSave(): Observable<any> {
        return this.uploadDocument().pipe(
            mergeMap((uploadedDocuments: IUploadedDocument[]) => {
                this.editorForm.controls.url.setValue(uploadedDocuments[0].url);
                return of(true);
            }),
            catchError(() => of(null)),
            mergeMap(() => this.saveFatawa()),
            catchError(() => of(null)),
        );
    }

    saveFatawa(): Observable<any> {
        return this.fatawaService.saveFatawaDefaultSettings(
            this.editorForm.value,
        );
    }

    addTag(event: MatChipInputEvent): void {
        const input = event.input;
        const value = event.value;

        if ((value || '').trim()) {
            this.tags.push({ name: value.trim() });
        }

        if (input) {
            input.value = '';
        }

        this.editorForm.controls.tags.setValue(this.tags);
    }

    removeTag(tag: TagModel): void {
        const index = this.tags.indexOf(tag);

        if (index >= 0) {
            this.tags.splice(index, 1);
        }
    }

    public loadFatawaImage(): void {
        if (this.url) {
            this.documentService.getUploadedDocument(this.url).subscribe(
                (data) => {
                    this.setFatawaImage(data);
                },
                () => {
                    this.notify.showTranslateMessage('ErrorOnGittingImage');
                },
            );
        } else {
            this.generalSettingsService
                .getGeneralSettings()
                .pipe(
                    map((generalSettings: GeneralSettingsModel[]) => {
                        let defaultFatawaImageURL: string = '';
                        if (generalSettings) {
                            defaultFatawaImageURL = generalSettings.find(
                                (generalSetting: GeneralSettingsModel) =>
                                    generalSetting.settingName ===
                                    Constants.Fatawa_Default_Image_Setting_Name,
                            ).settingValue;
                        }
                        return defaultFatawaImageURL;
                    }),
                    mergeMap((defaultFatawaImageURL: string) => {
                        if (defaultFatawaImageURL) {
                            return this.documentService.getUploadedDocument(
                                defaultFatawaImageURL,
                            );
                        }
                        return of('');
                    }),
                    tap((defaultFatawaImage: string) => {
                        this.setFatawaImage(defaultFatawaImage);
                        return true;
                    }),
                )
                .subscribe((result) => {});
        }
    }

    public setFatawaImage(fatawaImage: string): void {
        this.imageBase64.next(`data:image/jpeg;base64,${fatawaImage}`);
    }

    public get fileListFormControl(): AbstractControl {
        return this.formGroup.get(this.formConfig.controls[0].name);
    }

    public onFileSelect(fileList: FileList): void {
        this.validationMessageList = this.validateFiles(fileList);
        this.fileListFormControl.setValue(fileList);
    }

    public uploadDocument(): Observable<IUploadedDocument[]> {
        return this.documentService.uploadDocuments(
            this.fileListFormControl.value,
        );
    }

    private validateFiles(fileList: FileList): string[] {
        return this.fileService.buildValidationSuccessMessageList(
            fileList,
            this.fileConfig,
        );
    }

    private buildForm(): FormGroup {
        return this.formService.buildForm(this.formConfig);
    }
}
