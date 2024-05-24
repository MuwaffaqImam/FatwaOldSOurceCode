import {
    ChangeDetectionStrategy,
    ChangeDetectorRef,
    Component,
    EventEmitter,
    Input,
    Output,
} from '@angular/core';
import { AbstractControl, FormGroup } from '@angular/forms';
import { FileConfig, IFileConfig } from '@models/file';
import { FormConfig, IFormConfig } from '@models/form/form';
import { SaveCancelButtonConfig } from '@models/form/button';

@Component({
    selector: 'app-file-upload',
    templateUrl: './file-upload.component.html',
    styleUrls: ['./file-upload.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FileUploadComponent {
    @Input() public fileConfig: IFileConfig = new FileConfig();
    @Input() public fileListFormControl: AbstractControl;
    @Input() public form: FormGroup;
    @Input() public formConfig: IFormConfig = new FormConfig();
    @Input() public title = 'File Upload';
    @Input() public validationMessageList: string[] = [];

    @Output() public emitFileSelect = new EventEmitter<FileList>();
    @Output() public emitSubmit = new EventEmitter<void>();

    public imageUrl: string | ArrayBuffer = '';

    constructor(private cd: ChangeDetectorRef) {}

    public onFileSelect(fileList: FileList): void {
        this.emitFileSelect.emit(fileList);

        if (this.formConfig.submit) {
            this.formConfig.submit = new SaveCancelButtonConfig({
                save: this.updateSubmitButtonText(),
            });
        }
        this.previewImageUpload(fileList);
    }

    public submit(): void {
        this.emitSubmit.emit();
    }

    public updateSubmitButtonText(): string {
        const fileOrFiles =
            this.fileListFormControl.value.length > 1 ? 'files' : 'file';
        return this.fileConfig.canUploadMultiple &&
            this.form.valid &&
            this.fileListFormControl.value.length
            ? `Upload ${this.fileListFormControl.value.length} ${fileOrFiles}`
            : 'Upload';
    }

    private previewImageUpload(fileList: FileList): void {
        this.imageUrl = '';
        if (this.form.valid && fileList[0]) {
            const file = fileList[0];
            const mimeType = file.type;
            if (mimeType.match(/image\/*/) !== null) {
                if (fileList.length === 1) {
                    const reader = new FileReader();
                    reader.readAsDataURL(file);
                    reader.onload = (event) => {
                        this.imageUrl = event.target.result;
                        this.cd.markForCheck();
                    };
                }
            }
        }
    }
}
