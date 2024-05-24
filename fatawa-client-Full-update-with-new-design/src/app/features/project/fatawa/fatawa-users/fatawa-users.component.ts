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
import { AssignMuftiComponent } from './assign-mufti/assign-mufti.component';
import {
    DataUserTreeService,
    ActionUserNodeTree,
} from '@app/infrastructure/models/usernodeTree';
import { UserTreeItemNode } from '@app/infrastructure/models/project/fatawa/MuftiTreeModal';
import { catchError, map, tap } from 'rxjs/operators';
import { UserService } from '@app/infrastructure/core/services/auth/user.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
    selector: 'app-fatawa-users',
    templateUrl: './fatawa-users.component.html',
    styleUrls: ['./fatawa-users.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FatawaUsersComponent implements OnInit {
    searchTreeData: UserTreeItemNode[];
    dragDropTreeData: UserTreeItemNode[] = [];
    checkOnAcceptTree: UserTreeItemNode[];
    buildtreeData: UserTreeItemNode[];
    isMoveToDown: boolean = false;
    constructor(
        private dialog: MatDialog,
        private notify: NotificationService,
        private dataTreeService: DataUserTreeService,
        private userService: UserService,
        private cdr: ChangeDetectorRef,
        private translate: TranslateService,
    ) {
        this.translate.onLangChange.subscribe((e: any) => {
            this.loadAllMufti();
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

    onActionNodeClick(actionNodeTree: ActionUserNodeTree) {
        switch (actionNodeTree.type) {
            case State.Add:
                this.addNode(actionNodeTree.nodeInfo);
                break;
            // case State.Edit:
            //     this.editNode(actionNodeTree.nodeInfo);
            //     break;
            case State.Delete:
                this.deleteNode(actionNodeTree.nodeInfo);
                break;
        }
    }

    ngOnInit(): void {
        this.loadAllMufti();
    }

    loadAllMufti() {
        this.userService
            .getAllMufti()
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
                this.buildtreeData = this.buildUsersTree(
                    this.buildtreeData,
                    '0',
                    0,
                );
                this.dataTreeService.dataChange.next(this.buildtreeData);
            });
    }
    loadAllMuftiByLanguage(langId: number) {
        this.userService
            .getAllMuftiByLanguageId(langId)
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
                this.buildtreeData = this.buildUsersTree(
                    this.buildtreeData,
                    '0',
                    0,
                );
                this.dataTreeService.dataChange.next(this.buildtreeData);
            });
    }
    buildUsersTree(
        obj: any[],
        parent: string,
        level: number,
    ): UserTreeItemNode[] {
        return obj
            .filter((o) => o.nodeMain === parent && o.nodeLevelNumber == level)
            .map((o) => {
                const node = new UserTreeItemNode();
                node.id = o.id;
                node.parentId = o.parentId;
                node.mufitUserId = o.mufitUserId;
                node.nodeNumber = o.nodeNumber;
                node.nodeMain = o.nodeMain;
                node.nodeName = o.nodeName;
                node.nodeLevelNumber = o.nodeLevelNumber;
                node.sort = o.sort;
                const children = obj.filter(
                    (so) => so.nodeLevelNumber >= o.nodeLevelNumber,
                );
                this.dragDropTreeData.push(node);
                if (children && children.length > 0) {
                    node.children = this.buildUsersTree(
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

        const data = this.buildUsersTree(filteredTreeData, '0', 0);
        filterText != ''
            ? (this.dataTreeService.IsExpandAllTree = true)
            : (this.dataTreeService.IsExpandAllTree = false);
        this.dataTreeService.dataChange.next(data);
    }

    addNode(nodeToAdd: UserTreeItemNode) {
        const initNewNode = this.getNewChildNumber(nodeToAdd);
        this.dialog
            .open(AssignMuftiComponent, this.getConfigDialog(initNewNode))
            .afterClosed()
            .subscribe(
                (result) => {
                    if (result) {
                        // this line will be update
                        this.dataTreeService.IsExpandAllTree = true;
                        this.loadAllMufti();
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

    getNewChildNumber(node): UserTreeItemNode {
        const newNode = new UserTreeItemNode();
        newNode.id = 0;
        newNode.parentId = node.id;
        newNode.nodeLevelNumber = node.nodeLevelNumber + 1;
        newNode.mufitUserId = node.id;
        newNode.nodeNumber = '0';
        newNode.nodeMain = '0';
        return newNode;
    }

    // editNode(nodeToEdit: UserTreeItemNode) {
    //     this.dialog
    //         .open(AssignMuftiComponent, this.getConfigDialog(nodeToEdit))
    //         .afterClosed()
    //         .subscribe(
    //             (result) => {
    //                 if (result) {
    //                     // this line will be update
    //                     this.dataTreeService.IsExpandAllTree = true;
    //                     this.loadAllMufti();
    //                     this.notify.showTranslateMessage(
    //                         'UpdatedSuccessfully',
    //                         false,
    //                     );
    //                 } else {
    //                     this.notify.showTranslateMessage('CancelUpdate');
    //                 }
    //             },
    //             (error) => {
    //                 this.notify.showTranslateMessage('ErrorOnUpdate');
    //             },
    //         );
    // }

    deleteNode(nodeToDelete: UserTreeItemNode) {
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
                    this.userService
                        .deleteMuftiFromTree(nodeToDelete.id)
                        .subscribe(
                            (data) => {
                                if (data) {
                                    // this line will be update
                                    this.dataTreeService.IsExpandAllTree = true;
                                    this.loadAllMufti();
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
    applyLanguageFilter(filterText: number) {
        this.loadAllMuftiByLanguage(filterText);
    }
}
