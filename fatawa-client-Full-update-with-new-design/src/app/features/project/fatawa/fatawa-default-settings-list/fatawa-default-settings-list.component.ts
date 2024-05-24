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
import { FatawaService } from '@app/infrastructure/core/services/fatawa/fatawa.service';
import { FatawaModel } from '@app/infrastructure/models/project/fatawa/FatawaModel';
import { ConfirmDialogComponent } from '@app/infrastructure/shared/components/confirm-dialog/confirm-dialog.component';
import {
    ActionRowGrid,
    State,
} from '@app/infrastructure/shared/Services/CommonMemmber';
import { NotificationService } from '@core/services/notification.service';
import { TranslateService } from '@ngx-translate/core';
import { of } from 'rxjs';
import { mergeMap } from 'rxjs/operators';

@Component({
    selector: 'app-fatawa-default-settings-list',
    templateUrl: './fatawa-default-settings-list.component.html',
    styleUrls: ['./fatawa-default-settings-list.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FatawaDefaultSettingsListComponent implements OnInit {
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    public paginationIndex = 0;
    public pageIndex = 1;
    public pageSize = 10;
    public length = 0;
    public dataSource = new MatTableDataSource<FatawaModel>([]);

    constructor(
        private router: Router,
        private fatawaService: FatawaService,
        private dialog: MatDialog,
        private notify: NotificationService,
        private translate: TranslateService,
    ) {}

    ngOnInit(): void {
        this.getAllFatawasDefaultSettings(this.pageIndex, this.pageSize);
    }

    onActionRowGrid(actionGrid: ActionRowGrid): void {
        switch (actionGrid.type) {
            case State.Edit:
                void this.router.navigateByUrl(
                    `fatawa/fatawa-default-settings-editor/${actionGrid.row.id}`,
                );
                break;
            case State.Delete:
                this.deleteFatawa(actionGrid.row.id);
                break;
            case State.Pagination:
                this.getAllFatawasDefaultSettings(
                    actionGrid.row.pageIndex,
                    actionGrid.row.pageSize,
                );
                break;
        }
    }

    getAllFatawasDefaultSettings(pageIndex: number, pageSize: number): void {
        this.fatawaService
            .getAllFatawasDefaultSettings(pageIndex, pageSize)
            .subscribe(
                (paginationRecord) => {
                    this.dataSource.data = paginationRecord.dataRecord;
                    this.length = paginationRecord.countRecord;
                },
                (error) => {
                    this.notify.showTranslateMessage('ErrorOnLoadData');
                },
            );
    }

    addControl(resultClick: State): void {
        switch (resultClick) {
            case State.Add:
                void this.router.navigateByUrl(
                    'fatawa/fatawa-default-settings-editor',
                );
                break;
        }
    }

    deleteFatawa(id: number): void {
        this.dialog
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
            .pipe(
                mergeMap((result) => {
                    if (result) {
                        return this.fatawaService.deleteFatawaDefaultSettings(
                            id,
                        );
                    }
                    return of({});
                }),
            )
            .subscribe(
                (data) => {
                    if (data) {
                        this.getAllFatawasDefaultSettings(
                            this.pageIndex,
                            this.pageSize,
                        );
                        this.notify.showTranslateMessage(
                            'DeletedSuccessfully',
                            false,
                        );
                    } else {
                        this.notify.showTranslateMessage('Cancel Delete.');
                    }
                },
                (error) => {
                    this.notify.showTranslateMessage('ErrorOnDelete');
                },
            );
    }

    applyFilter(searchKey: string): void {
        this.dataSource.filter = searchKey
            ? searchKey.trim().toLocaleLowerCase()
            : '';
    }
}
