export interface IMainBar {
    subject: string;
    time: string;
}

export interface IMessageBar {
    round: string;
    icon: string;
    title: string;
}

export class MessageBar implements IMessageBar, IMainBar {
    round: string = '';
    icon: string = '';
    title: string = '';
    subject: string = '';
    time: string = '';

    constructor(configOverride?: Partial<IMessageBar>) {
        if (configOverride) {
            Object.assign(this, configOverride);
        }
    }
}

export interface INotificationBar {
    useravatar: string;
    status: string;
    from: string;
}

export class NotificationBar implements INotificationBar, IMainBar {
    useravatar: string = '';
    status: string = '';
    from: string = '';
    subject: string = '';
    time: string = '';

    constructor(configOverride?: Partial<INotificationBar>) {
        if (configOverride) {
            Object.assign(this, configOverride);
        }
    }
}
