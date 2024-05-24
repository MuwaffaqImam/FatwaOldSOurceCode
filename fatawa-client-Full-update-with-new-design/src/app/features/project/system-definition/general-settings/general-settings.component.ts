import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { AbstractControl, FormGroup } from '@angular/forms';
import { DocumentService } from '@app/infrastructure/core/services/documents/document.service';
import { FormService } from '@core/services/form.service';
import { FormConfig, FormField } from '@models/form/form';
import { IInputField, InputField } from '@models/form/input';
import { FileService } from '@core/services/file.service';
import { FileConfig } from '@models/file';
import { RequiredValidation } from '@utils/validation/required-validation';
import { UploadValidation } from '@utils/validation/upload-validation';
import { IUploadedDocument } from '@app/infrastructure/models/uploaded-document';
import { GeneralSettingsService } from '@app/infrastructure/core/services/general-settings.service';
import { GeneralSettingsModel } from '@models/project/general-settings';
import { Observable, of, Subscription, BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { catchError, mergeMap, tap } from 'rxjs/operators';
import { NotificationService } from '@app/infrastructure/core/services/notification.service';
import { Constants } from '@app/infrastructure/utils/constants';

@Component({
    templateUrl: './general-settings.component.html',
    styleUrls: ['./general-settings.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class GeneralSettingsComponent implements OnInit {
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

    public form: FormGroup;
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
    public validationMessageList: string[] = [];
    public generalSettingsList: GeneralSettingsModel[];
    public imageBase64: BehaviorSubject<string> = new BehaviorSubject('');

    constructor(
        private documentService: DocumentService,
        private fileService: FileService,
        private formService: FormService,
        private generalSettingsService: GeneralSettingsService,
        private notify: NotificationService,
    ) {}

    public ngOnInit(): void {
        this.form = this.buildForm();
        this.getGeneralSettingsInfo();
    }

    public getGeneralSettingsInfo() {
        return this.generalSettingsService
            .getGeneralSettings()
            .pipe(
                map((generalSettings: GeneralSettingsModel[]) => {
                    let defaultFatawaImage: string = '';
                    if (generalSettings) {
                        defaultFatawaImage = generalSettings.find(
                            (generalSetting: GeneralSettingsModel) =>
                                generalSetting.settingName ===
                                Constants.Fatawa_Default_Image_Setting_Name,
                        ).settingValue;
                    }
                    this.generalSettingsList = generalSettings;
                    return defaultFatawaImage;
                }),
                mergeMap((defaultFatawaImage: string) => {
                    if (defaultFatawaImage) {
                        return this.documentService.getUploadedDocument(
                            defaultFatawaImage,
                        );
                    }
                    return of('');
                }),
                tap((defaultFatawaImage: string) => {
                    this.setFatawaDefaultImage(defaultFatawaImage);
                    return true;
                }),
            )
            .subscribe((result) => {});
    }

    public setFatawaDefaultImage(defaultFatawaImage: string): void {
        this.imageBase64.next(`data:image/jpeg;base64,${defaultFatawaImage}`);
    }

    public get fileListFormControl(): AbstractControl {
        return this.form.get(this.formConfig.controls[0].name);
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

    public saveSettings(): Subscription {
        const generalSettingsModel: GeneralSettingsModel = new GeneralSettingsModel();
        if (this.fileListFormControl.value) {
            return this.uploadDocument()
                .pipe(
                    mergeMap((uploadedDocuments: IUploadedDocument[]) => {
                        generalSettingsModel.settingName =
                            Constants.Fatawa_Default_Image_Setting_Name;
                        generalSettingsModel.settingValue =
                            uploadedDocuments[0].url;
                        return this.generalSettingsService.setGeneralSettings([
                            generalSettingsModel,
                        ]);
                    }),
                    catchError((error) => of(null)),
                )
                .subscribe((value) => {
                    this.getGeneralSettingsInfo();
                    if (value) {
                        this.notify.showTranslateMessage(
                            'UpdatedSuccessfully',
                            false,
                        );
                    } else {
                        this.notify.showTranslateMessage('UpdatedFailed');
                    }
                });
        }
        return this.generalSettingsService
            .setGeneralSettings([generalSettingsModel])
            .pipe(catchError((error) => of(null)))
            .subscribe((value) => {
                this.getGeneralSettingsInfo();
                if (value) {
                    this.notify.showTranslateMessage(
                        'UpdatedSuccessfully',
                        false,
                    );
                } else {
                    this.notify.showTranslateMessage('UpdatedFailed');
                }
            });
    }

    private buildForm(): FormGroup {
        return this.formService.buildForm(this.formConfig);
    }

    private validateFiles(fileList: FileList): string[] {
        return this.fileService.buildValidationSuccessMessageList(
            fileList,
            this.fileConfig,
        );
    }
}
