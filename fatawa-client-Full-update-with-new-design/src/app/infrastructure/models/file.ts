export interface IFileConfig {
    allowedFileExtensionList: string[];
    allowedMimeTypeList: string[];
    canUploadMultiple: boolean;
}

export class FileConfig implements IFileConfig {
    allowedFileExtensionList: string[] = [];
    allowedMimeTypeList: string[] = [];
    canUploadMultiple: boolean = false;

    constructor(configOverride?: Partial<IFileConfig>) {
        if (configOverride) {
            Object.assign(this, configOverride);
        }
    }
}

export interface IEncodedImageDTO {
    base64: string;
    contentType: string;
}
