export interface DepartmentNodeModel {
    id: number;
    nodeNumber: string;
    nodeMain: string;
    parentId: number;
    nodeName: string;
    nodeLevelNumber: number;
    fatawaDepartmentTranslateId: number;
    languageId: number;
    sort: number;
}

export class TreeItemNode {
    children: TreeItemNode[];
    id: number;
    nodeNumber: string;
    nodeMain: string;
    parentId: number;
    nodeName: string;
    nodeLevelNumber: number;
    fatawaDepartmentTranslateId: number;
    languageId: number;
    sort: number;
}
