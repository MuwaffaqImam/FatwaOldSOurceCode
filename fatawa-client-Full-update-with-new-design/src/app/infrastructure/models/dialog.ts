export interface IConfirmDialogConfig {
    id?: number;
    title: string;
    messageList: string[];
    action: string;
    showCancel: boolean;
}

export class ConfirmDialogConfig implements IConfirmDialogConfig {
    id?: number = 0;
    title: string = 'Confirm';
    messageList: string[] = ['AskToContinue'];
    action: string = 'Confirm';
    showCancel: boolean = true;

    constructor(configOverride?: Partial<IConfirmDialogConfig>) {
        if (configOverride) {
            Object.assign(this, configOverride);
        }
    }
}
