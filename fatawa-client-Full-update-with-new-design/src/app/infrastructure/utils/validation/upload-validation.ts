import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

import { FileService } from '@core/services/file.service';
import { IFileConfig } from '@models/file';

export class UploadValidation {
    /**
     * Returns errors if the passed in control `FileList` does not validate.
     */
    static validateFiles(fileConfig: IFileConfig): ValidatorFn {
        return (control: AbstractControl): ValidationErrors | null => {
            const fileList: FileList = control.value;
            const validationError: ValidationErrors = {};

            // FileList does not have access to Array methods like `forEach()`
            /* tslint:disable:prefer-for-of */
            for (let i = 0; i < fileList.length; i++) {
                if (
                    !FileService.validateFileExtension(
                        fileList[i].name,
                        fileConfig.allowedFileExtensionList,
                    )
                ) {
                    // Set unique iterated property names to prevent overwriting
                    const invalidExtension = `invalidExtension${i}`;
                    validationError[invalidExtension] = `${
                        fileList[i].name
                    } does not have an allowed file type. Allowed types are .${fileConfig.allowedFileExtensionList.join(
                        ', .',
                    )}.`;
                }
                if (!FileService.validateFileSize(fileList[i].size)) {
                    const invalidSize = `invalidSize${i}`;
                    validationError[
                        invalidSize
                    ] = `${fileList[i].name} is too large.`;
                }
                if (!FileService.validateFileNameLength(fileList[i].name)) {
                    const invalidName = `invalidName${i}`;
                    validationError[
                        invalidName
                    ] = `${fileList[i].name} has a file name that is too long.`;
                }
            }

            return validationError;
        };
    }
}
