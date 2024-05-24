export interface Message {
    id: number;
    senderId: number;
    senderName?: string;
    SenderPhotoUrl?: string;
    receiverId: number;
    receiverName?: string;
    receiverPhotoUrl?: string;
    messageText: string;
    isRead?: boolean;
    createDate?: Date;
}
