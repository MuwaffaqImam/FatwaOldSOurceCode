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
    ActionNodeTree,
    DataTreeService,
} from '@app/infrastructure/models/nodeTree';
import { TreeItemNode } from '@app/infrastructure/models/project/department/departmentModel';
import { Constants } from '@app/infrastructure/utils/constants';
import { TranslateService } from '@ngx-translate/core';

@Component({
    selector: 'app-tree',
    templateUrl: './tree.component.html',
    styleUrls: ['./tree.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TreeComponent implements OnInit {
    nestedTreeControl: NestedTreeControl<TreeItemNode>;
    treeDataSource = new MatTreeNestedDataSource();
    // tslint:disable-next-line:no-output-on-prefix
    @Output() onAddNode = new BehaviorSubject<ActionNodeTree>({
        type: State.Non,
        nodeInfo: null,
    });
    // tslint:disable-next-line:no-output-on-prefix
    @Output() onEditNode = new BehaviorSubject<ActionNodeTree>({
        type: State.Non,
        nodeInfo: null,
    });
    // tslint:disable-next-line:no-output-on-prefix
    @Output() onDeleteNode = new BehaviorSubject<ActionNodeTree>({
        type: State.Non,
        nodeInfo: null,
    });

    @Output() onNodeMovedUp = new BehaviorSubject<ActionNodeTree>({
        type: State.Non,
        nodeInfo: null,
    });

    @Output() onNodeMovedDown = new BehaviorSubject<ActionNodeTree>({
        type: State.Non,
        nodeInfo: null,
    });

    @Output() onNodeSearch = new BehaviorSubject<ActionNodeTree>({
        type: State.Non,
        nodeInfo: null,
    });

    hasNestedChild = (_: number, nodeData: TreeItemNode) =>
        nodeData.children.length > 0;
    @Input() IsShowMainActionControls = true;
    dir: string = Constants.DefaultLanguageDirection;
    constructor(
        private data: DataTreeService,
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
        this.nestedTreeControl = new NestedTreeControl<TreeItemNode>(
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
        const actionNode: ActionNodeTree = {
            type: State.Add,
            nodeInfo: nodeToBeAdd,
        };
        this.onAddNode.next(actionNode);
    }

    editNode(nodeToBeEdited) {
        const actionNode: ActionNodeTree = {
            type: State.Edit,
            nodeInfo: nodeToBeEdited,
        };
        this.onEditNode.next(actionNode);
    }

    deleteNode(nodeToBeDeleted) {
        const actionNode: ActionNodeTree = {
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
        const actionNode: ActionNodeTree = {
            type: State.NodeMovedUp,
            nodeInfo: nodeMovedUp,
        };
        this.onNodeMovedDown.next(actionNode);
    }

    nodeMovedDown(nodeMovedDown) {
        const actionNode: ActionNodeTree = {
            type: State.NodeMovedDown,
            nodeInfo: nodeMovedDown,
        };
        this.onNodeMovedDown.next(actionNode);
    }

    searchOnTree(nodeSearchFatawa) {
        const actionNode: ActionNodeTree = {
            type: State.NodeSearchFatawa,
            nodeInfo: nodeSearchFatawa,
        };
        this.onNodeSearch.next(actionNode);
    }
}
