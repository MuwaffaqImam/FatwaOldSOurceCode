import {
    Component,
    OnInit,
    ChangeDetectionStrategy,
    Inject,
} from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { LanguageModel } from '@models/project/LanguageModel';
import { MatDialogRef } from '@angular/material/dialog';
import { LanguagesService } from '@core/services/language/language.service';
import { FileService } from '@core/services/file.service';
import { FileConfig } from '@models/file';
import { RequiredValidation } from '@utils/validation/required-validation';
import { UploadValidation } from '@utils/validation/upload-validation';
import { IUploadedDocument } from '@app/infrastructure/models/uploaded-document';
import { Observable, of, BehaviorSubject } from 'rxjs';
import { AbstractControl } from '@angular/forms';
import { DocumentService } from '@app/infrastructure/core/services/documents/document.service';
import { FormService } from '@core/services/form.service';
import { FormConfig, FormField } from '@models/form/form';
import { IInputField, InputField } from '@models/form/input';
import { catchError, mergeMap, tap } from 'rxjs/operators';
@Component({
    templateUrl: './add-language.component.html',
    styleUrls: ['./add-language.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AddLanguageComponent implements OnInit {
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
    public formConfig = new FormConfig({
        formName: 'formFlagImage',
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
    public validationMessageList: string[] = [];
    public isInProgress = false;
    public frmAddNew: FormGroup;
    public formFlagImage: FormGroup;

    constructor(
        @Inject(MAT_DIALOG_DATA) public languageModel: LanguageModel,
        private documentService: DocumentService,
        private fileService: FileService,
        private formService: FormService,
        private formBuilder: FormBuilder,
        private languagesService: LanguagesService,
        private dialogRef: MatDialogRef<AddLanguageComponent>,
    ) {}

    get ID() {
        return this.frmAddNew.controls.Id.value;
    }

    ngOnInit(): void {
        this.formFlagImage = this.buildForm();
        this.ngInitialControlForm();
        this.setLanguageDetails();
    }

    ngInitialControlForm() {
        this.frmAddNew = this.formBuilder.group({
            Id: [0],
            LanguageCode: ['', Validators.required],
            LanguageDefaultDisply: ['', Validators.required],
            LanguageDirection: ['RTL', Validators.required],
            languageFlag: [''],
        });
    }

    setLanguageDetails() {
        if (this.languageModel) {
            this.frmAddNew.controls.Id.setValue(this.languageModel.id);
            this.frmAddNew.controls.LanguageCode.setValue(
                this.languageModel.languageCode,
            );
            this.frmAddNew.controls.LanguageDefaultDisply.setValue(
                this.languageModel.languageDefaultDisply,
            );
            this.frmAddNew.controls.LanguageDirection.setValue(
                this.languageModel.languageDirection,
            );
            this.frmAddNew.controls.languageFlag.setValue(
                this.languageModel.languageFlag,
            );

            this.languagesService
                .getLanguageById(this.languageModel.id)
                .pipe(
                    mergeMap((resultLanguageFlag: LanguageModel) => {
                        return resultLanguageFlag.languageFlag
                            ? this.documentService.getUploadedDocument(
                                  resultLanguageFlag.languageFlag,
                              )
                            : of('');
                    }),
                    tap((binaryImage: string) => {
                        return binaryImage
                            ? this.setLanguageFlagImage(binaryImage)
                            : of('');
                    }),
                )
                .subscribe((result) => {});
        }
    }

    OnSubmit() {
        this.isInProgress = true;

        var initialObservable = this.fileListFormControl.value
            ? this.uploadFlag()
            : of({});
        return initialObservable
            .pipe(
                mergeMap((uploadedDocuments: IUploadedDocument[]) => {
                    if (this.fileListFormControl.value)
                        this.frmAddNew.controls.languageFlag.setValue(
                            uploadedDocuments[0].url,
                        );

                    return this.ID === 0
                        ? this.languagesService.addLanguage(
                              this.frmAddNew.value,
                          )
                        : this.languagesService.updateLanguage(
                              this.frmAddNew.value,
                          );
                }),
                catchError(() => of(null)),
            )
            .subscribe(
                (id: any) => {
                    if (id) {
                        this.frmAddNew.controls.Id.setValue(id);
                        this.dialogRef.close(this.frmAddNew.value);
                        this.frmAddNew.reset();
                    }
                },
                () => {
                    this.dialogRef.close(this.frmAddNew.value);
                },
            );
    }

    resetFormBuilder() {
        this.frmAddNew.reset();
        this.isInProgress = false;
    }
    public get fileListFormControl(): AbstractControl {
        return this.formFlagImage.get(this.formConfig.controls[0].name);
    }
    public onFileSelect(fileList: FileList): void {
        this.validationMessageList = this.validateFiles(fileList);
        this.fileListFormControl.setValue(fileList);
    }
    public uploadFlag(): Observable<IUploadedDocument[]> {
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
    public setLanguageFlagImage(languageFlagImage: string): void {
        this.imageBase64.next(`data:image/jpeg;base64,${languageFlagImage}`);
    }
}
