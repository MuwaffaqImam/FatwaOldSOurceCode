import { MainModel } from './main-model';

export interface IChatMessageModel {
    toUserId: number;
    type: string;
    message: string;
    fromConnectionId: string;
    toConnectionId: string;
}

export class ChatMessageModel implements IChatMessageModel, MainModel {
    id: number = 0;
    createdBy: number = 0;
    createdDate: Date = new Date();
    updatedBy?: number = 0;
    updatedDate?: Date = new Date();
    toUserId: number = 0;
    type: string = '';
    message: string = '';
    fromConnectionId: string = '';
    toConnectionId: string = '';

    constructor(configOverride?: Partial<IChatMessageModel>) {
        if (configOverride) {
            Object.assign(this, configOverride);
        }
    }
}
