import { Injectable } from '@angular/core';

import { Constants } from '@utils/constants';
import { IFileConfig } from '@models/file';

@Injectable({
    providedIn: 'root',
})
export class FileService {
    static validateFileExtension(
        fileName: string,
        allowedExtensionList: string[] = [],
    ): boolean {
        const extension = this.getFileExtension(fileName);
        const hasValidFileExtension =
            !!allowedExtensionList &&
            allowedExtensionList.some(
                (extensionType) => extensionType === extension,
            );

        return hasValidFileExtension;
    }

    static validateFileSize(size: number): boolean {
        const isFileSizeValid = !!size && size < Constants.MAXFILESIZE;

        return isFileSizeValid;
    }

    static validateFileNameLength(filename: string): boolean {
        const isFileNameValid = filename?.length < Constants.MAXFILENAMELENGTH;

        return isFileNameValid;
    }

    static getFileExtension(fileName: string): string {
        if (!!fileName && fileName.includes('.')) {
            return fileName.split('.').pop();
        } else {
            throw new Error('File name is missing extension.');
        }
    }

    /**
     * Convert the provided `files` into a `FormData` object that can be uploaded.
     */
    public buildFormData(files: FileList): FormData {
        const fileListFormData: FormData = new FormData();

        // FileList does not have access to Array methods like `forEach()`
        /* tslint:disable:prefer-for-of */
        if (files.length > 0) {
            for (let i: number = 0; i < files.length; i++) {
                appendToFormData('File', files[i]);
            }
        } else {
            throw new Error('FileList must not be empty.');
        }

        return fileListFormData;

        function appendToFormData(propertyName: string, file: File): void {
            fileListFormData.append(propertyName, file);
        }
    }

    public buildValidationSuccessMessageList(
        fileList: FileList,
        fileConfig: IFileConfig,
    ): string[] {
        const validationSuccessMessageList: string[] = [];
        // FileList does not have access to Array methods like `forEach()`
        for (let i = 0; i < fileList.length; i++) {
            if (
                FileService.validateFileExtension(
                    fileList[i].name,
                    fileConfig.allowedFileExtensionList,
                ) &&
                FileService.validateFileSize(fileList[i].size) &&
                FileService.validateFileNameLength(fileList[i].name)
            ) {
                const message = `${fileList[i].name} can be uploaded.`;
                validationSuccessMessageList.push(message);
            }
        }

        return validationSuccessMessageList;
    }
}
