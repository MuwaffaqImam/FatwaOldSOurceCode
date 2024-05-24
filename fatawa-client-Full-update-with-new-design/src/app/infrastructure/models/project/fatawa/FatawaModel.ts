export class FatawaModel {
    id: number;
    order: number;
    name: string;
    fatawaQuestion: string;
    description: string;
    fatawaTypeId: number;
    statusId: number;
    fatawaDepartmentId: number;
    fatawaMathhabId: number;
    fatawaDepartmentName: string;
    visitors: number;
    lastSeen: Date;
    createdDate: Date;
    statusName: string;
    muftiId: number;
    translatorName: string;
    url: string;
    tags: TagModel[];
    languageId: number;
    questionId?: number;
    mathhabName?: string;
    updatedDate: Date;
    muftiName: string;
}

export class TagModel {
    name: string;
}
