import {
    Component,
    OnInit,
    ChangeDetectionStrategy,
    Output,
    ChangeDetectorRef,
    Input,
} from '@angular/core';
import { MatTreeNestedDataSource } from '@angular/material/tree';
import { NestedTreeControl } from '@angular/cdk/tree';
import { of as observableOf, BehaviorSubject } from 'rxjs';
import { State } from '../../Services/CommonMemmber';
import {
    ActionUserNodeTree,
    DataUserTreeService,
} from '@app/infrastructure/models/usernodeTree';
import { UserTreeItemNode } from '@app/infrastructure/models/project/fatawa/MuftiTreeModal';
import { Constants } from '@app/infrastructure/utils/constants';
import { TranslateService } from '@ngx-translate/core';

@Component({
    selector: 'app-treeuser',
    templateUrl: './treeuser.component.html',
    styleUrls: ['./treeuser.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TreeuserComponent implements OnInit {
    nestedTreeControl: NestedTreeControl<UserTreeItemNode>;
    treeDataSource = new MatTreeNestedDataSource();
    // tslint:disable-next-line:no-output-on-prefix
    @Output() onAddNode = new BehaviorSubject<ActionUserNodeTree>({
        type: State.Non,
        nodeInfo: null,
    });
    // tslint:disable-next-line:no-output-on-prefix
    @Output() onEditNode = new BehaviorSubject<ActionUserNodeTree>({
        type: State.Non,
        nodeInfo: null,
    });
    // tslint:disable-next-line:no-output-on-prefix
    @Output() onDeleteNode = new BehaviorSubject<ActionUserNodeTree>({
        type: State.Non,
        nodeInfo: null,
    });

    @Output() onNodeMovedUp = new BehaviorSubject<ActionUserNodeTree>({
        type: State.Non,
        nodeInfo: null,
    });

    @Output() onNodeMovedDown = new BehaviorSubject<ActionUserNodeTree>({
        type: State.Non,
        nodeInfo: null,
    });

    @Output() onNodeSearch = new BehaviorSubject<ActionUserNodeTree>({
        type: State.Non,
        nodeInfo: null,
    });

    hasNestedChild = (_: number, nodeData: UserTreeItemNode) =>
        nodeData.children.length > 0;
    @Input() IsShowMainActionControls = true;
    dir: string = Constants.DefaultLanguageDirection;
    constructor(
        private data: DataUserTreeService,
        private cdr: ChangeDetectorRef,
        private translate: TranslateService,
    ) {
        //execute when language change to fix tree design depending classes (example-tree)-dir in css file
        this.translate.onLangChange.subscribe((e: any) => {
            this.dir = e.lang === 'AR' || e.lang === 'UR' ? 'rtl' : 'ltr';
            this.cdr.detectChanges();
        });
    }

    ngOnInit(): void {
        this.nestedTreeControl = new NestedTreeControl<UserTreeItemNode>(
            (node: any) => observableOf(node.children),
        );

        this.data.dataChange.subscribe((data) => {
            this.treeDataSource.data = data;
            // add this line to solve bug expandAll() function.
            this.nestedTreeControl.dataNodes = data;

            this.data.IsExpandAllTree
                ? this.nestedTreeControl.expandAll()
                : this.nestedTreeControl.collapseAll();
            // add this to refresh tree with latest data
            this.cdr.detectChanges();
        });
    }

    addNode(nodeToBeAdd) {
        const actionNode: ActionUserNodeTree = {
            type: State.Add,
            nodeInfo: nodeToBeAdd,
        };
        this.onAddNode.next(actionNode);
    }

    editNode(nodeToBeEdited) {
        const actionNode: ActionUserNodeTree = {
            type: State.Edit,
            nodeInfo: nodeToBeEdited,
        };
        this.onEditNode.next(actionNode);
    }

    deleteNode(nodeToBeDeleted) {
        const actionNode: ActionUserNodeTree = {
            type: State.Delete,
            nodeInfo: nodeToBeDeleted,
        };
        this.onDeleteNode.next(actionNode);
    }

    collapseAll() {
        this.nestedTreeControl.collapseAll();
    }

    expandAll() {
        this.nestedTreeControl.expandAll();
    }

    nodeMovedUp(nodeMovedUp) {
        const actionNode: ActionUserNodeTree = {
            type: State.NodeMovedUp,
            nodeInfo: nodeMovedUp,
        };
        this.onNodeMovedDown.next(actionNode);
    }

    nodeMovedDown(nodeMovedDown) {
        const actionNode: ActionUserNodeTree = {
            type: State.NodeMovedDown,
            nodeInfo: nodeMovedDown,
        };
        this.onNodeMovedDown.next(actionNode);
    }

    searchOnTree(nodeSearchFatawa) {
        const actionNode: ActionUserNodeTree = {
            type: State.NodeSearchFatawa,
            nodeInfo: nodeSearchFatawa,
        };
        this.onNodeSearch.next(actionNode);
    }
}
