import {
    Component,
    OnInit,
    ChangeDetectionStrategy,
    ViewChild,
} from '@angular/core';
import {
    State,
    ActionRowGrid,
} from '@app/infrastructure/shared/Services/CommonMemmber';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { JobsService } from '@core/services/job/job.service';
import { MatPaginator } from '@angular/material/paginator';
import { JobModel } from '@models/project/JobModel';
import { AddJobComponent } from '../addJob/add-job.component';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '@app/infrastructure/shared/components/confirm-dialog/confirm-dialog.component';
import { NotificationService } from '@core/services/notification.service';

@Component({
    selector: 'app-jobs-list',
    templateUrl: './jobs-list.component.html',
    styleUrls: ['./jobs-list.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class JobsListComponent implements OnInit {
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    public paginationIndex = 0;
    public pageIndex = 1;
    public pageSize = 10;
    public length = 0;
    public dataSource = new MatTableDataSource<JobModel>([]);

    constructor(
        private jobsService: JobsService,
        private dialog: MatDialog,
        private notify: NotificationService,
    ) {}

    getConfigDialog(data, isAddGridHeader?: boolean): MatDialogConfig {
        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = true;
        dialogConfig.autoFocus = true;
        dialogConfig.position = { top: '80px' };
        dialogConfig.width = '60%';
        dialogConfig.data = data;
        return dialogConfig;
    }

    ngOnInit(): void {
        this.LoadJobs(this.pageIndex, this.pageSize);
    }

    LoadJobs(pageIndex: number, pageSize: number) {
        this.jobsService.getJobs(pageIndex, pageSize).subscribe(
            (paginationRecord) => {
                this.dataSource.data = paginationRecord.dataRecord;
                this.length = paginationRecord.countRecord;
            },
            (error) => {
                this.notify.showTranslateMessage('ErrorOnLoadData');
            },
        );
    }

    onEditControlClick(resultClick: State) {
        switch (resultClick) {
            case State.Add:
                this.AddNewRecord();
                break;
        }
    }

    applyFilter(searchKey: string) {
        this.dataSource.filter = searchKey
            ? searchKey.trim().toLocaleLowerCase()
            : '';
    }

    onActionRowGrid(ActionGrid: ActionRowGrid) {
        switch (ActionGrid.type) {
            case State.Edit:
                this.onEdit(ActionGrid.row);
                break;
            case State.Delete:
                this.onDelete(ActionGrid.row);
                break;
            case State.Pagination:
                this.LoadJobs(
                    ActionGrid.row.pageIndex,
                    ActionGrid.row.pageSize,
                );
                break;
        }
    }

    AddNewRecord() {
        this.dialog
            .open(AddJobComponent, this.getConfigDialog(null))
            .afterClosed()
            .subscribe(
                (result) => {
                    if (result) {
                        this.LoadJobs(1, this.pageSize);
                        this.dataSource.paginator = this.paginator;
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

    onEdit(jobModel: JobModel) {
        this.dialog
            .open(AddJobComponent, this.getConfigDialog(jobModel))
            .afterClosed()
            .subscribe(
                (result) => {
                    if (result) {
                        this.LoadJobs(1, this.pageSize);
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

    onDelete(row: { [key: string]: any }) {
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
                    this.jobsService.Delete(row.id).subscribe(
                        (data) => {
                            if (data) {
                                this.LoadJobs(1, this.pageSize);
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
}
