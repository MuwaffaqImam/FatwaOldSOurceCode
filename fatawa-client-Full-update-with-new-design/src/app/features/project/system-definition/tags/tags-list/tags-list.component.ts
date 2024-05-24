import {
    ChangeDetectionStrategy,
    Component,
    OnInit,
    ViewChild,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { TagService } from '@app/infrastructure/core/services/system-definition/tag.service';
import { TagModel } from '@app/infrastructure/models/project/system-definition/tagModel';
import { ConfirmDialogComponent } from '@app/infrastructure/shared/components/confirm-dialog/confirm-dialog.component';
import {
    ActionRowGrid,
    State,
} from '@app/infrastructure/shared/Services/CommonMemmber';
import { NotificationService } from '@core/services/notification.service';

@Component({
    selector: 'app-tags-list',
    templateUrl: './tags-list.component.html',
    styleUrls: ['./tags-list.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TagsListComponent implements OnInit {
    public paginationIndex = 0;
    public pageIndex = 1;
    public pageSize = 10;
    public length = 0;
    public dataSource = new MatTableDataSource<TagModel>([]);
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;

    constructor(
        private tagService: TagService,
        private router: Router,
        private dialog: MatDialog,
        private notify: NotificationService,
    ) {}

    ngOnInit(): void {
        this.getAllTags(this.pageIndex, this.pageSize);
    }

    onActionRowGrid(actionGrid: ActionRowGrid) {
        switch (actionGrid.type) {
            case State.Edit:
                this.router.navigateByUrl(
                    'system-definition/tags/tag-editor/' + actionGrid.row.id,
                );
                break;
            case State.Delete:
                this.deleteTag(actionGrid.row.id);
                break;
            case State.Pagination:
                this.getAllTags(
                    actionGrid.row.pageIndex,
                    actionGrid.row.pageSize,
                );
                break;
        }
    }

    getAllTags(pageIndex: number, pageSize: number) {
        this.tagService.GetTags(pageIndex, pageSize).subscribe(
            (paginationRecord) => {
                this.dataSource.data = paginationRecord.dataRecord;
                this.length = paginationRecord.countRecord;
            },
            (error) => {
                this.notify.showTranslateMessage('ErrorOnLoadData');
            },
        );
    }

    addControl(resultClick: State) {
        switch (resultClick) {
            case State.Add:
                this.router.navigateByUrl(
                    'system-definition/tags/tag-editor/' + 0,
                );
                break;
        }
    }

    deleteTag(id) {
        return this.dialog
            .open(ConfirmDialogComponent, {
                width: '27em',
                height: '9em',
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
                    this.tagService.DeleteTag(id).subscribe(
                        (data) => {
                            if (data) {
                                this.getAllTags(this.pageIndex, this.pageSize);
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
                            this.notify.showTranslateMessage('ErrorOnDelete');
                        },
                    );
                }
            });
    }

    applyFilter(searchKey: string) {
        this.dataSource.filter = searchKey
            ? searchKey.trim().toLocaleLowerCase()
            : '';
    }
}
