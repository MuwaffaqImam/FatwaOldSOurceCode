export interface UserNodeModel {
    id: number;
    parentId: number;
    nodeNumber: string;
    nodeMain: string;
    mufitUserId: number;
    nodeName: string;
    nodeLevelNumber: number;
    sort: number;
}

export class UserTreeItemNode {
    children: UserTreeItemNode[];
    id: number;
    parentId: number;
    nodeNumber: string;
    nodeMain: string;
    mufitUserId: number;
    nodeName: string;
    nodeLevelNumber: number;
    sort: number;
}
