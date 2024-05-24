export interface QuestionDiscussion {
    id: number;
    questionId: number;
    senderId: number;
    senderName?: string;
    SenderPhotoUrl?: string;
    repliedId: number;
    repliedName?: string;
    repliedPhotoUrl?: string;
    messageText: string;
    isRead?: boolean;
    createDate?: Date;
}
