import {
    Component,
    OnInit,
    ChangeDetectionStrategy,
    ViewChild,
    AfterViewInit,
} from '@angular/core';
import {
    State,
    ActionRowGrid,
} from '@app/infrastructure/shared/Services/CommonMemmber';
import { MatTableDataSource, MatTable } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { LanguagesService } from '@core/services/language/language.service';
import { MatPaginator } from '@angular/material/paginator';
import { LanguageModel } from '@models/project/LanguageModel';
import { AddLanguageComponent } from '../addLanguage/add-language.component';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '@app/infrastructure/shared/components/confirm-dialog/confirm-dialog.component';
import { NotificationService } from '@core/services/notification.service';
import { catchError, switchMap } from 'rxjs/operators';
import { of } from 'rxjs';

@Component({
    templateUrl: './languages-list.component.html',
    styleUrls: ['./languages-list.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LanguagesListComponent implements OnInit {
    public paginationIndex = 0;
    public pageIndex = 1;
    public pageSize = 10;
    public length = 0;
    public dataSource = new MatTableDataSource<LanguageModel>([]);
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;

    constructor(
        private languagesService: LanguagesService,
        private matDialog: MatDialog,
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
        this.LoadLanguages(this.pageIndex, this.pageSize);
    }

    LoadLanguages(pageIndex: number, pageSize: number) {
        this.languagesService.getLanguages(pageIndex, pageSize).subscribe(
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
                this.LoadLanguages(
                    ActionGrid.row.pageIndex,
                    ActionGrid.row.pageSize,
                );
                break;
        }
    }

    AddNewRecord() {
        const dialog = this.matDialog.open(
            AddLanguageComponent,
            this.getConfigDialog(null),
        );
        return dialog
            .afterClosed()
            .pipe(
                switchMap((dialogResult: string) => {
                    if (dialogResult) {
                        this.LoadLanguages(1, this.pageSize);
                        this.dataSource.paginator = this.paginator;
                        this.notify.showTranslateMessage(
                            'AddedSuccessfully',
                            false,
                        );
                        return of('');
                    } else {
                        this.notify.showTranslateMessage('CancelAdd');
                        return of('');
                    }
                }),
                catchError((): any => {
                    this.notify.showTranslateMessage('ErrorOnAdd');
                }),
            )
            .subscribe((result) => {});
    }

    onEdit(languageModel: LanguageModel) {
        this.matDialog
            .open(AddLanguageComponent, this.getConfigDialog(languageModel))
            .afterClosed()
            .pipe(
                switchMap((dialogResult: string) => {
                    if (dialogResult) {
                        this.LoadLanguages(1, this.pageSize);
                        this.notify.showTranslateMessage(
                            'UpdatedSuccessfully',
                            false,
                        );
                        return of('');
                    } else {
                        this.notify.showTranslateMessage('CancelUpdate');
                        return of('');
                    }
                }),
                catchError((): any => {
                    this.notify.showTranslateMessage('ErrorOnUpdate');
                }),
            )
            .subscribe((result) => {});
    }

    onDelete(row: LanguageModel) {
        return this.matDialog
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
                switchMap((dialogResult: string) => {
                    if (dialogResult) {
                        return this.languagesService.deleteLanguage(row.id);
                    } else {
                        this.notify.showTranslateMessage('CancelDelete');
                        return of('');
                    }
                }),
                catchError((): any => {
                    this.notify.showTranslateMessage('ErrorOnDelete');
                }),
            )
            .subscribe((result) => {
                this.LoadLanguages(1, this.pageSize);
                this.notify.showTranslateMessage('DeletedSuccessfully', false);
            });
    }
}
