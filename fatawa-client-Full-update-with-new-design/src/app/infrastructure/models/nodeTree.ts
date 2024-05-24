import { BehaviorSubject } from 'rxjs';
import { State } from '../shared/Services/CommonMemmber';
import { Injectable } from '@angular/core';
import { TreeItemNode } from './project/department/departmentModel';

export interface ActionNodeTree {
    type: State;
    nodeInfo: TreeItemNode;
}

@Injectable()
export class DataTreeService {
    private isExpandAll: boolean = false;
    dataChange = new BehaviorSubject<TreeItemNode[]>([]);

    get IsExpandAllTree(): boolean {
        return this.isExpandAll;
    }
    set IsExpandAllTree(value: boolean) {
        this.isExpandAll = value;
    }
}
