import { NotificationType } from '@app/infrastructure/models/SystemEnum';

export enum State {
    Non,
    View,
    Add,
    Edit,
    Delete,
    Search,
    Print,
    PrintAll,
    Excel,
    ExcelAll,
    Pagination,
    Available,
    InProgress,
    Rejected,
    Duplicated,
    Done,
    ToFatawa,
    ToMufti,
    Copy,
    Chatting,
    NodeMovedUp,
    NodeMovedDown,
    NodeSearchFatawa,
    ClearSearchBox,
    Language,
}

export interface ActionRowGrid {
    type: State;
    row: any;
}

export interface SystemNotification {
    id: number;
    createdBy: number;
    recipientId: number;
    senderId: number;
    messageText: string;
    referenceMassageId: number;
    notificationTypeId: NotificationType;
}

export enum FatawaType {
    FatawaArchived = 1,
    FatawaLive = 2,
    FatawaImport = 3,
}
