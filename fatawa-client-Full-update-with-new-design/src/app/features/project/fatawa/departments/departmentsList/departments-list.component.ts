import {
    Component,
    OnInit,
    ChangeDetectionStrategy,
    ChangeDetectorRef,
} from '@angular/core';
import { State } from '@app/infrastructure/shared/Services/CommonMemmber';
import { ConfirmDialogComponent } from '@app/infrastructure/shared/components/confirm-dialog/confirm-dialog.component';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { NotificationService } from '@app/infrastructure/core/services/notification.service';
import { addDepartmentComponent } from '../addDepartment/addDepartment.component';
import { FatawaDepartmentService } from '@app/infrastructure/core/services/fatawa/department.service';
import {
    DataTreeService,
    ActionNodeTree,
} from '@app/infrastructure/models/nodeTree';
import { TreeItemNode } from '@app/infrastructure/models/project/department/departmentModel';
import { catchError, map, tap } from 'rxjs/operators';
import { UserService } from '@app/infrastructure/core/services/auth/user.service';
import { DragDropModel } from '@app/infrastructure/models/project/dragDropModel';
import { TranslateService } from '@ngx-translate/core';

@Component({
    templateUrl: './departments-list.component.html',
    styleUrls: ['./departments-list.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DepartmentsListComponent implements OnInit {
    searchTreeData: TreeItemNode[];
    dragDropTreeData: TreeItemNode[] = [];
    checkOnAcceptTree: TreeItemNode[];
    buildtreeData: TreeItemNode[];
    isMoveToDown: boolean = false;
    constructor(
        private dialog: MatDialog,
        private fatawaDepartmentService: FatawaDepartmentService,
        private notify: NotificationService,
        private dataTreeService: DataTreeService,
        private userService: UserService,
        private cdr: ChangeDetectorRef,
        private translate: TranslateService,
    ) {
        this.translate.onLangChange.subscribe((e: any) => {
            this.loadDepartments();
            this.cdr.detectChanges();
        });
    }

    getConfigDialog(data, isAddGridHeader?: boolean): MatDialogConfig {
        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = true;
        dialogConfig.autoFocus = true;
        dialogConfig.position = { top: '5em' };
        dialogConfig.width = '50em';
        dialogConfig.data = data;
        return dialogConfig;
    }

    onActionNodeClick(actionNodeTree: ActionNodeTree) {
        switch (actionNodeTree.type) {
            case State.Add:
                this.addNode(actionNodeTree.nodeInfo);
                break;
            case State.Edit:
                this.editNode(actionNodeTree.nodeInfo);
                break;
            case State.Delete:
                this.deleteNode(actionNodeTree.nodeInfo);
                break;
            case State.NodeMovedUp:
                this.nodeMovedUpDown(actionNodeTree.nodeInfo, false);
                break;
            case State.NodeMovedDown:
                this.nodeMovedUpDown(actionNodeTree.nodeInfo, true);
                break;
        }
    }

    ngOnInit(): void {
        this.loadDepartments();
    }

    loadDepartments() {
        this.fatawaDepartmentService
            .getAllDepartmentsByLanguageId()
            .pipe(
                map((data) => {
                    this.buildtreeData = data;
                }),
                catchError((): any => {
                    this.notify.showTranslateMessage('ErrorOnLoadData');
                }),
            )
            .subscribe((result) => {
                this.searchTreeData = this.buildtreeData;
                this.dragDropTreeData.splice(0, this.dragDropTreeData.length);
                this.buildtreeData = this.buildDepartmentsTree(
                    this.buildtreeData,
                    '0',
                    0,
                );
                this.dataTreeService.dataChange.next(this.buildtreeData);
            });
    }

    buildDepartmentsTree(
        obj: any[],
        parent: string,
        level: number,
    ): TreeItemNode[] {
        return obj
            .filter((o) => o.nodeMain === parent && o.nodeLevelNumber == level)
            .map((o) => {
                const node = new TreeItemNode();
                node.id = o.id;
                node.parentId = o.parentId;
                node.nodeNumber = o.nodeNumber;
                node.nodeMain = o.nodeMain;
                node.nodeName = o.nodeName;
                node.nodeLevelNumber = o.nodeLevelNumber;
                node.fatawaDepartmentTranslateId =
                    o.fatawaDepartmentTranslateId;
                node.sort = o.sort;
                const children = obj.filter(
                    (so) => so.nodeLevelNumber >= o.nodeLevelNumber,
                );
                node.languageId = o.languageId;
                this.dragDropTreeData.push(node);
                if (children && children.length > 0) {
                    node.children = this.buildDepartmentsTree(
                        children,
                        o.nodeNumber,
                        o.nodeLevelNumber + 1,
                    );
                }
                return node;
            });
    }

    applyFilter(filterText: string) {
        let filteredTreeData;
        if (filterText) {
            filteredTreeData = this.searchTreeData.filter(
                (d) =>
                    d.nodeName
                        .toLocaleLowerCase()
                        .indexOf(filterText.toLocaleLowerCase()) > -1,
            );
            Object.assign([], filteredTreeData).forEach((ftd) => {
                let currentParent = <string>ftd.nodeMain;
                while (currentParent) {
                    const obj = this.searchTreeData.find(
                        (d) => d.nodeNumber === currentParent,
                    );

                    currentParent = obj ? obj.nodeMain : null;

                    if (
                        obj &&
                        !filteredTreeData.find(
                            (t) => t.nodeNumber === obj.nodeNumber,
                        )
                    ) {
                        filteredTreeData.push(obj);
                    }
                }
            });
        } else {
            filteredTreeData = this.searchTreeData;
        }

        const data = this.buildDepartmentsTree(filteredTreeData, '0', 0);
        filterText != ''
            ? (this.dataTreeService.IsExpandAllTree = true)
            : (this.dataTreeService.IsExpandAllTree = false);
        this.dataTreeService.dataChange.next(data);
    }

    addNode(nodeToAdd: TreeItemNode) {
        const initNewNode = this.getNewChildNumber(nodeToAdd);
        this.dialog
            .open(addDepartmentComponent, this.getConfigDialog(initNewNode))
            .afterClosed()
            .subscribe(
                (result) => {
                    if (result) {
                        // this line will be update
                        this.dataTreeService.IsExpandAllTree = true;
                        this.loadDepartments();
                        this.notify.showTranslateMessage(
                            'AddedSuccessfully',
                            false,
                        );
                    } else {
                        this.notify.showTranslateMessage('CancelAdd');
                    }
                },
                (error) => {
                    this.notify.showTranslateMessage('ErrorOnAdd');
                },
            );
    }

    getNewChildNumber(node): TreeItemNode {
        const newNode = new TreeItemNode();
        newNode.id = 0;
        newNode.nodeLevelNumber = node.nodeLevelNumber + 1;
        newNode.parentId = node.id;
        newNode.nodeNumber = '0';
        newNode.nodeMain = '0';
        newNode.fatawaDepartmentTranslateId = 0;
        newNode.languageId = this.userService.getLanguageId();
        return newNode;
    }

    editNode(nodeToEdit: TreeItemNode) {
        nodeToEdit.languageId = this.userService.getLanguageId();
        this.dialog
            .open(addDepartmentComponent, this.getConfigDialog(nodeToEdit))
            .afterClosed()
            .subscribe(
                (result) => {
                    if (result) {
                        // this line will be update
                        this.dataTreeService.IsExpandAllTree = true;
                        this.loadDepartments();
                        this.notify.showTranslateMessage(
                            'UpdatedSuccessfully',
                            false,
                        );
                    } else {
                        this.notify.showTranslateMessage('CancelUpdate');
                    }
                },
                (error) => {
                    this.notify.showTranslateMessage('ErrorOnUpdate');
                },
            );
    }

    deleteNode(nodeToDelete: TreeItemNode) {
        if (nodeToDelete.children.length > 0) {
            this.notify.showTranslateMessage('CannotDeleteNodeContainChildren');
            return;
        } else if (nodeToDelete.nodeLevelNumber === 0) {
            this.notify.showTranslateMessage('CannotDeleteNodeLevelZero');
            return;
        }

        return this.dialog
            .open(ConfirmDialogComponent, {
                width: '28em',
                height: '11em',
                panelClass: 'confirm-dialog-container',
                position: { top: '5em' },
                disableClose: true,
                data: {
                    messageList: ['SureWantDelete'],
                    action: 'Delete',
                    showCancel: true,
                },
            })
            .afterClosed()
            .subscribe((result) => {
                if (result) {
                    this.fatawaDepartmentService
                        .deleteDepartment(nodeToDelete.id)
                        .subscribe(
                            (data) => {
                                if (data) {
                                    // this line will be update
                                    this.dataTreeService.IsExpandAllTree = true;
                                    this.loadDepartments();
                                    this.notify.showTranslateMessage(
                                        'DeletedSuccessfully',
                                        false,
                                    );
                                } else {
                                    this.notify.showTranslateMessage(
                                        'CancelDelete',
                                    );
                                }
                            },
                            (error) => {
                                this.notify.showTranslateMessage(
                                    'ErrorOnDelete',
                                );
                            },
                        );
                }
            });
    }

    nodeMovedUpDown(nodeMoved: TreeItemNode, isMovedToDown: boolean) {
        const lastNodeMovedId = nodeMoved.id;
        const nodeLevel = nodeMoved.nodeLevelNumber;
        const nodeMovedParentId = nodeMoved.parentId;

        this.checkOnAcceptTree = this.dragDropTreeData.filter(
            (x) =>
                x.nodeLevelNumber === nodeLevel &&
                x.parentId === nodeMovedParentId,
        );

        // to check if node (first cant move up)  or (last cant node move down)
        if (
            (nodeMoved.id === this.checkOnAcceptTree[0].id && !isMovedToDown) ||
            (nodeMoved.id ===
                this.checkOnAcceptTree[this.checkOnAcceptTree.length - 1].id &&
                isMovedToDown)
        ) {
            this.notify.showTranslateMessage('MustSameLevel');
            return;
        }

        // get node index in array
        const newIndexNode = this.checkOnAcceptTree.findIndex(
            (x) => x.id === lastNodeMovedId,
        );

        const newNodeId = this.checkOnAcceptTree[
            isMovedToDown ? newIndexNode + 1 : newIndexNode - 1
        ].id;
        this.isMoveToDown = isMovedToDown;

        const dearhDropModel = new DragDropModel();
        dearhDropModel.previousNodeId = lastNodeMovedId;
        dearhDropModel.currentNodeId = newNodeId;
        dearhDropModel.isMoveToDown = this.isMoveToDown;

        this.fatawaDepartmentService
            .updateDropDepartment(dearhDropModel)
            .pipe(
                tap((isDropUpdated) => {
                    if (isDropUpdated) {
                        this.dataTreeService.IsExpandAllTree = true;
                        this.loadDepartments();
                    }
                }),
            )
            .subscribe((result) => {
                // add this to refresh tree with latest data
                this.cdr.detectChanges();
            });
    }
}
