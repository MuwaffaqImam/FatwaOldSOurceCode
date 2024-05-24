/* eslint-disable arrow-body-style */
/* eslint-disable @typescript-eslint/explicit-module-boundary-types */
/* eslint-disable @typescript-eslint/no-unsafe-return */
/* eslint-disable @typescript-eslint/unbound-method */
/* eslint-disable @typescript-eslint/no-unsafe-assignment */
/* eslint-disable @typescript-eslint/no-unsafe-call */
/* eslint-disable @typescript-eslint/no-unsafe-member-access */
import { ENTER } from '@angular/cdk/keycodes';
import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import {
    AbstractControl,
    FormBuilder,
    FormGroup,
    Validators,
} from '@angular/forms';
import { MatChipInputEvent } from '@angular/material/chips';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '@app/infrastructure/core/services/auth/user.service';
import { DocumentService } from '@app/infrastructure/core/services/documents/document.service';
import { FatawaService } from '@app/infrastructure/core/services/fatawa/fatawa.service';
import { QuestionService } from '@app/infrastructure/core/services/fatawa/question.service';
import { FileService } from '@app/infrastructure/core/services/file.service';
import { FormService } from '@app/infrastructure/core/services/form.service';
import { GeneralSettingsService } from '@app/infrastructure/core/services/general-settings.service';
import { LanguagesService } from '@app/infrastructure/core/services/language/language.service';
import { NotificationService } from '@app/infrastructure/core/services/notification.service';
import { TokenService } from '@app/infrastructure/core/services/token.service';
import { IChatMessageModel } from '@app/infrastructure/models/chat-message';
import { FileConfig } from '@app/infrastructure/models/file';
import { Message } from '@app/infrastructure/models/message';
import { TreeItemNode } from '@app/infrastructure/models/project/department/departmentModel';
import {
    FatawaModel,
    TagModel,
} from '@app/infrastructure/models/project/fatawa/FatawaModel';
import { GeneralSettingsModel } from '@app/infrastructure/models/project/general-settings';
import { LanguageModel } from '@app/infrastructure/models/project/LanguageModel';
import { FatwaType } from '@app/infrastructure/models/SystemEnum';
import { IUploadedDocument } from '@app/infrastructure/models/uploaded-document';
import { Constants } from '@app/infrastructure/utils/constants';
import { RequiredValidation } from '@app/infrastructure/utils/validation/required-validation';
import { UploadValidation } from '@app/infrastructure/utils/validation/upload-validation';
import { FormConfig, FormField } from '@models/form/form';
import { IInputField, InputField } from '@models/form/input';
import { BehaviorSubject, combineLatest, Observable, of } from 'rxjs';
import {
    catchError,
    map,
    mergeMap,
    switchMap,
    take,
    tap,
} from 'rxjs/operators';
import { ClipboardService, IClipboardResponse } from 'ngx-clipboard';
import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic';
@Component({
    templateUrl: './fatawa-editor.component.html',
    styleUrls: ['./fatawa-editor.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FatawaEditorComponent implements OnInit {
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
    public Editor = ClassicEditor;
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
    id: number;
    editorForm: FormGroup;
    submitted = false;
    strorig: string = '';
    data: FatawaModel;
    fatawaTypes = [];
    muftiList = [];
    fatawaDepartments: TreeItemNode[] = [];
    selectedDepartmens: TreeItemNode[] = [];
    mathhabs = [];
    fatawaStatuses = [];
    currentQuestion: string;
    isAdd: boolean = false;
    isEdit: boolean = false;
    isQuestionToFatwa: boolean = false;
    currentUserID: Observable<number>;
    newMessage: IChatMessageModel;
    translators: string[];
    translatorsFiltered: string[];
    fatawaDefaultSettings: FatawaModel;
    languages: LanguageModel[];
    selectedLanguageId: number;
    showOriginal = false;
    originalFatwaName = '';
    originalFatwaQuestion = '';
    originalDescription = '';
    originalTags = [];
    isCopied = false;
    get fatawaTypeId() {
        return this.editorForm.controls.fatawaTypeId.value;
    }
    get isSuperAdminUser(): Observable<boolean> {
        return this.tokenService.isSuperAdmin();
    }
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
        private languagesService: LanguagesService,
        private activatedroute: ActivatedRoute,
        private questionService: QuestionService,
        private _clipboardService: ClipboardService,
    ) {
        this.handleClipboardResponse();
        this.editorForm = this.formBuilder.group({
            name: ['', [Validators.required]],
            order: [Number, null],
            fatawaQuestion: ['', [Validators.required]],
            description: [''],
            fatawaTypeId: ['', [Validators.required]],
            fatawaDepartmentId: ['', [Validators.required]],
            statusId: ['', null],
            fatawaMathhabId: ['', [Validators.required]],
            muftiId: ['', [Validators.required]],
            translatorName: ['', null],
            tags: ['', [Validators.required]],
            url: [''],
            id: Number,
            questionId: [Number, null],
        });
        this.fillAutoCompleteValues();

        this.editorForm.controls.translatorName.valueChanges.subscribe(
            (newValue) => {
                this.translatorsFiltered = this.getFilteredValues(newValue);
            },
        );

        this.userService.languageChangedSubject$
            .pipe(switchMap((languageId: string) => this.getFatawaDepartmens()))
            .subscribe((data) => {
                this.fatawaDepartments = this.selectedDepartmens = data;
            });
    }

    ngOnInit(): void {
        this.formGroup = this.buildForm();
        this.updateFieldsListBasedOnLanguage()
            .pipe(
                mergeMap(() => {
                    return this.languagesService.getAllLanguages();
                }),
                mergeMap((data) => {
                    this.languages = data;
                    return this.activatedroute.data;
                }),
                mergeMap((data) => {
                    this.id = data.editFatwa?.id;
                    return this.checkBeforeGetOriginalFatawa(
                        this.selectedLanguageId,
                        !data.editFatwa,
                    );
                }),
                mergeMap((data: FatawaModel | null) => {
                    this.setOriginalValues(data);
                    return this.activatedroute.data;
                }),
                mergeMap((data) => {
                    this.selectedLanguageId = this.userService.getLanguageId();
                    if (data.questionToFatwa) {
                        this.isQuestionToFatwa = true;
                        this.editorForm.controls.fatawaQuestion.setValue(
                            data.questionToFatwa.fatawaQuestion,
                        );
                        this.editorForm.controls.questionId.setValue(
                            data.questionToFatwa.id,
                        );
                        this.editorForm.controls.muftiId.setValue(
                            data.questionToFatwa.transferUserId,
                        );
                        return this.fatawaService.getFatawaDefaultSettingsByUser();
                    } else if (data.editFatwa) {
                        this.isEdit = true;
                        this.fillFatawaObj(data.editFatwa);
                        return this.fatawaService.getFatawaDefaultSettingsByUser();
                    } else if (data.addFatwaDefaultSettings) {
                        this.isAdd = true;
                        return of(data.addFatwaDefaultSettings);
                    } else {
                        return of(null);
                    }
                }),
            )
            .subscribe((data: FatawaModel) => {
                this.fatawaDefaultSettings = data ?? new FatawaModel();

                if (this.isAdd || this.isQuestionToFatwa) {
                    this.fillFatawaObj(this.fatawaDefaultSettings, true);
                }
            });
    }

    fillAutoCompleteValues() {
        this.fatawaService
            .getTranslatorAutoCompleteValues()
            .subscribe((values: string[]) => {
                this.translators = this.translatorsFiltered = values;
            });
    }

    getFilteredValues(search: string): any {
        if (!search || !this.translators) {
            return this.translators;
        }

        return this.translators.filter(
            (value) => value.toLowerCase().indexOf(search.toLowerCase()) === 0,
        );
    }

    fillFatawaObj(fatawaModel: FatawaModel, ignoreId: boolean = false): void {
        if (!fatawaModel) {
            return;
        }
        this.editorForm.setValue({
            name: fatawaModel.name,
            order: fatawaModel.order,
            fatawaQuestion: this.isQuestionToFatwa
                ? this.editorForm.controls.fatawaQuestion.value
                : fatawaModel.fatawaQuestion,
            description: fatawaModel.description,
            muftiId: this.isQuestionToFatwa
                ? this.editorForm.controls.muftiId.value
                : fatawaModel.muftiId,
            translatorName: fatawaModel.translatorName,
            fatawaTypeId: fatawaModel.fatawaTypeId,
            fatawaDepartmentId: fatawaModel.fatawaDepartmentId,
            statusId: fatawaModel.statusId,
            fatawaMathhabId: fatawaModel.fatawaMathhabId,
            id: ignoreId ? 0 : fatawaModel.id,
            tags: fatawaModel.tags,
            url: fatawaModel.url,
            questionId: this.isQuestionToFatwa
                ? this.editorForm.controls.questionId.value
                : fatawaModel.questionId ?? null,
        });

        this.url = fatawaModel.url;
        this.tags = fatawaModel.tags;

        this.loadFatawaImage();
    }

    getFatawaMuftiList() {
        return this.userService.getAllSuperAdminsAsync();
    }

    getFatawaTypes() {
        return this.fatawaService.getFatawaTypes();
    }

    getFatawaDepartmens() {
        return this.fatawaService.getFatawaDeparments();
    }

    changeFatawaDepartmens(e) {
        this.editorForm.controls.fatawaDepartmentId.setValue(e.target.value);
    }

    searchNodeName(searchNodeName: string) {
        let result = this.filterDepartments(searchNodeName);
        this.selectedDepartmens = result;
    }

    filterDepartments(searchNodeName: string): TreeItemNode[] {
        return this.fatawaDepartments.filter((department: TreeItemNode) => {
            return (
                department.nodeName?.toLowerCase().indexOf(searchNodeName) > -1
            );
        });
    }

    getFatawaStatuses() {
        return this.fatawaService.getFatawaStatuses();
    }

    changeFatawaStatuses(e) {
        this.editorForm.controls.statusId.setValue(e.target.value);
    }

    getMathhabs() {
        return this.fatawaService.getFatawaMathhabs();
    }

    changeMathhabs(e) {
        this.editorForm.controls.fatawaMathhabId.setValue(e.target.value);
    }

    get form() {
        return this.editorForm.controls;
    }

    submitFatawa(isPublish: boolean): void {
        this.isInProgress = true;
        this.submitted = true;

        if (this.editorForm.invalid) {
            return;
        }

        const observableMethod: Observable<any> = this.fileListFormControl.value
            ? this.uploadDocumentThenSave(isPublish)
            : this.saveFatawa(isPublish);

        observableMethod
            .pipe(
                mergeMap((data) => {
                    if (data && !isPublish) {
                        //Invoke For SuperAdmin only
                        this.notify.invokeAddedNewFatwa();
                        return of(0);
                    } else if (data && isPublish) {
                        return this.questionService.getUserIdAddedQuestion(
                            this.editorForm.controls.questionId.value,
                        );
                    }
                }),
                mergeMap((userId: number) => {
                    if (userId > 0) {
                        this.notify.invokePublishedNewFatwa(Number(userId));
                    }
                    return of(true);
                }),
                catchError(() => {
                    this.isInProgress = false;
                    return of(null);
                }),
            )
            .subscribe((value) => {
                this.isInProgress = false;

                if (value) {
                    this.notify.showTranslateMessage('UpdatedSuccessfully');
                } else {
                    this.notify.showTranslateMessage('SaveFailed');
                }

                if (this.fatawaTypeId === FatwaType.FatawaArchived) {
                    void this.router.navigate(['fatawa/fatawa-archived']);
                } else {
                    void this.router.navigate(['fatawa/fatawa-live']);
                }
            });
    }

    uploadDocumentThenSave(isPublish: boolean): Observable<any> {
        return this.uploadDocument().pipe(
            mergeMap((uploadedDocuments: IUploadedDocument[]) => {
                this.editorForm.controls.url.setValue(uploadedDocuments[0].url);
                return of(true);
            }),
            catchError(() => of(null)),
            mergeMap(() => this.saveFatawa(isPublish)),
            catchError(() => of(null)),
        );
    }

    saveFatawa(isPublish: boolean): Observable<number> {
        this.editorForm.controls.statusId.setValue(isPublish ? 2 : 1);
        const fatawaModel = this.editorForm.value;

        fatawaModel.languageId =
            this.selectedLanguageId ?? Constants.DefaultLanguageId;
        return this.fatawaService.saveFatawa(fatawaModel);
    }

    addTag(event: MatChipInputEvent): void {
        const input = event.input;
        const value = event.value;
        this.tags = this.tags ?? [];

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

    public setDefaultSettings(settingName: string): void {
        this.fatawaDefaultSettings[settingName] = this.editorForm.controls[
            settingName
        ].value;
        this.fatawaDefaultSettings.languageId =
            this.selectedLanguageId || Constants.DefaultLanguageId;
        this.fatawaService
            .saveFatawaDefaultSettings(this.fatawaDefaultSettings)
            .subscribe((resultId: number) => {
                this.fatawaDefaultSettings.id = resultId;
            });
    }

    // public copy(text: string) {
    //     this._clipboardService.copy(text);
    //     //
    // }

    updateFieldsListBasedOnLanguage(): Observable<any> {
        return this.getFatawaTypes().pipe(
            mergeMap((data) => {
                this.fatawaTypes = data;
                return this.getFatawaMuftiList();
            }),
            mergeMap((data) => {
                this.muftiList = data;
                return this.getFatawaDepartmens();
            }),
            mergeMap((data) => {
                this.fatawaDepartments = this.selectedDepartmens = data;
                return this.getMathhabs();
            }),
            mergeMap((data) => {
                this.mathhabs = data;
                return this.getFatawaStatuses();
            }),
            mergeMap((data) => {
                this.fatawaStatuses = data;
                return of({});
            }),
        );
    }

    public languageChanged(language: LanguageModel): void {
        this.updateFieldsListBasedOnLanguage()
            .pipe(
                switchMap(() =>
                    combineLatest([
                        this.checkBeforeGetOriginalFatawa(language.id),
                        this.fatawaService
                            .getFatawaByLanguage(
                                this.editorForm.controls.id.value,
                                language.id,
                            )
                            .pipe(take(1)),
                    ]),
                ),
            )
            .subscribe((fatawaModels: FatawaModel[]) => {
                this.setOriginalValues(fatawaModels[0]);
                this.fillFatawaObj(fatawaModels[1]);
            });
    }
    private handleClipboardResponse() {
        this._clipboardService.copyResponse$.subscribe(
            (res: IClipboardResponse) => {
                if (res.isSuccess) {
                    this.notify.showTranslateMessage('CopiedSuccessfully');
                }
            },
        );
    }
    private checkBeforeGetOriginalFatawa(
        languageId: number,
        isJustReturnNull: boolean = false,
    ): Observable<FatawaModel> | Observable<null> {
        return languageId !== Constants.DefaultLanguageId &&
            !isJustReturnNull &&
            this.id
            ? this.getOriginalFatwa()
            : of(null);
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

    private getOriginalFatwa(): Observable<FatawaModel> {
        return this.fatawaService.getFatawaByLanguage(
            this.id,
            Constants.DefaultLanguageId,
        );
    }
    public removeHTML(str: string): string {
        var stripedHtml = str.replace(/<[^>]+>/g, '').replace('&nbsp;', ' ');
        return stripedHtml;
    }
    private setOriginalValues(fatawaModel: FatawaModel) {
        this.originalFatwaName = fatawaModel?.name ?? '';
        this.originalFatwaQuestion = fatawaModel?.fatawaQuestion ?? '';
        this.originalDescription = this.removeHTML(
            fatawaModel?.description ?? '',
        );
        this.originalTags = fatawaModel?.tags ?? [];
        this.showOriginal = !!fatawaModel;
    }
}
