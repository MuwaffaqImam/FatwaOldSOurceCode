import { BehaviorSubject } from 'rxjs';
import { State } from '../shared/Services/CommonMemmber';
import { Injectable } from '@angular/core';
import { UserTreeItemNode } from './project/fatawa/MuftiTreeModal';

export interface ActionUserNodeTree {
    type: State;
    nodeInfo: UserTreeItemNode;
}

@Injectable()
export class DataUserTreeService {
    private isExpandAll: boolean = false;
    dataChange = new BehaviorSubject<UserTreeItemNode[]>([]);

    get IsExpandAllTree(): boolean {
        return this.isExpandAll;
    }
    set IsExpandAllTree(value: boolean) {
        this.isExpandAll = value;
    }
}
