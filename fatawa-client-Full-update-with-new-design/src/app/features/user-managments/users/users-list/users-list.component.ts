/* eslint-disable @typescript-eslint/no-unsafe-member-access */
import {
    Component,
    OnInit,
    ChangeDetectionStrategy,
    ViewChild,
} from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { NotificationService } from '@app/infrastructure/core/services/notification.service';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import {
    ActionRowGrid,
    State,
} from '@app/infrastructure/shared/Services/CommonMemmber';
import { UserModel } from '@app/infrastructure/models/project/UserModel';
import { UserService } from '@app/infrastructure/core/services/auth/user.service';
import { UsersRolesComponent } from '../users-roles/users-roles.component';
import { catchError, map, mergeMap, switchMap } from 'rxjs/operators';
import { RoleModel } from '@app/infrastructure/models/project/RoleModel';
import { of } from 'rxjs';
import { ConfirmDialogComponent } from '@app/infrastructure/shared/components/confirm-dialog/confirm-dialog.component';
import { Router } from '@angular/router';
import { DataGridViewComponent } from '@app/infrastructure/shared/components/data-grid-view/data-grid-view.component';
import { PageEvent } from '@angular/material/paginator';
import { LanguagesListComponent } from '@app/features/project/system-definition/languages-settings/languagesList/languages-list.component';
import { LanguageModel } from '@app/infrastructure/models/project/LanguageModel';
import { LanguagesService } from '@app/infrastructure/core/services/language/language.service';
@Component({
    selector: 'app-users-list',
    templateUrl: './users-list.component.html',
    styleUrls: ['./users-list.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UsersListComponent implements OnInit {
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    public paginationIndex = 0;
    public pageIndex = 1;
    public pageSize = 10;
    searchText: string = '';
    public length = 0;
    public dataSource = new MatTableDataSource<UserModel>([]);
    public roles: RoleModel[] = [];
    public languages: LanguageModel[] = [];
    @ViewChild(DataGridViewComponent) sharedDataGridView: DataGridViewComponent;
    constructor(
        private userService: UserService,
        private dialog: MatDialog,
        private notify: NotificationService,
        private router: Router,
        private languagesService: LanguagesService,
    ) {}

    ngOnInit(): void {
        this.loadUsers(this.pageIndex, this.pageSize);
    }

    get SearchInputSearchBox() {
        return this.searchText;
    }

    handlePage(event?: PageEvent) {
        this.pageIndex = event.pageIndex;
        this.pageSize = event.pageSize;

        !this.SearchInputSearchBox;
        this.loadUsers(this.pageIndex, this.pageSize);
    }
    applyFilter(searchKey: string): void {
        // this.dataSource.filter = searchKey;
        this.pageIndex = 1;
        this.searchText = searchKey ? searchKey : '';
        this.loadUserSearch();
        this.sharedDataGridView.onFirstPage();
    }
    loadUserSearch() {
        this.userService
            .GetUserFiltered(this.searchText, this.pageIndex, this.pageSize)
            .subscribe(
                (paginationRecord) => {
                    this.dataSource.data = paginationRecord.dataRecord;
                    this.length = paginationRecord.countRecord;
                    return this.userService.getAllRoles();
                },
                () => {
                    this.notify.showTranslateMessage('ErrorOnGittingFatawas');
                },
            );
    }
    getConfigDialog(data, isAddGridHeader?: boolean): MatDialogConfig {
        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = true;
        dialogConfig.autoFocus = true;
        dialogConfig.position = { top: '5em' };
        dialogConfig.width = '30em';
        // eslint-disable-next-line @typescript-eslint/no-unsafe-assignment
        dialogConfig.data = data;
        return dialogConfig;
    }

    onActionRowGrid(ActionGrid: ActionRowGrid) {
        switch (ActionGrid.type) {
            case State.Pagination:
                this.pageIndex = ActionGrid.row.pageIndex;
                this.pageSize = ActionGrid.row.pageSize;
                this.loadUsers(
                    ActionGrid.row.pageIndex,
                    ActionGrid.row.pageSize,
                );
                break;
            case State.Edit:
                this.onEdit(ActionGrid.row);
                break;
            case State.Delete:
                this.onDelete(ActionGrid.row);
                break;
            case State.Language:
                this.onchangeLanguage(ActionGrid.row);
                break;
        }
    }
    addControl(resultClick: State) {
        switch (resultClick) {
            case State.Add:
                this.router.navigateByUrl('users/adduser');
                break;
            case State.ClearSearchBox:
                this.clearSearch();
                break;
        }
    }
    clearSearch() {
        // this.pageIndex = 1;
        // this.departmentId = 0;
        // this.searchText = '';
        // this.searchInputDepartment.nativeElement.value = '';

        this.loadUsers(this.pageIndex, this.pageSize);
        this.sharedDataGridView.onFirstPage();
    }
    loadUsers(pageIndex: number, pageSize: number) {
        this.userService
            .getUsers(pageIndex, pageSize)
            .pipe(
                mergeMap((paginationRecord) => {
                    this.dataSource.data = paginationRecord.dataRecord;
                    this.length = paginationRecord.countRecord;
                    return this.userService.getAllRoles();
                }),
                map((roles) => {
                    this.roles = roles;
                }),
                catchError((): any => {
                    this.notify.showTranslateMessage('ErrorOnLoadData');
                }),
            )
            .subscribe((result) => {});
    }

    onEdit(userModel: UserModel) {
        const dialog = this.dialog.open(
            UsersRolesComponent,
            this.getConfigDialog({
                allRoles: this.roles,
                roleId: userModel.roleId,
            }),
        );

        return dialog
            .afterClosed()
            .pipe(
                switchMap((newRoleId) => {
                    if (newRoleId && newRoleId != userModel.roleId) {
                        userModel.roleId = newRoleId;
                        return this.userService.updateRoleUser(userModel);
                    } else {
                        return of('');
                    }
                }),
                map((isUpdated) => {
                    if (isUpdated) {
                        this.loadUsers(this.pageIndex, this.pageSize);
                        this.notify.showTranslateMessage('UpdatedSuccessfully');
                    }
                }),
                catchError((): any => {
                    this.notify.showTranslateMessage('ErrorOnUpdate');
                }),
            )
            .subscribe((result) => {});
    }

    applyLanguageFilter(filterText: number) {}
    onchangeLanguage(languagemodal: LanguageModel) {
        const dialog = this.dialog.open(
            LanguagesListComponent,
            this.getConfigDialog({
                AllLanguages: this.languages,
                roleId: languagemodal.id,
            }),
        );

        return dialog
            .afterClosed()
            .pipe(
                switchMap((newRoleId) => {
                    if (newRoleId && newRoleId) {
                        // userModel.roleId = newRoleId;
                        // return this.languagesService.updateLanguage(
                        //     languagemodal,
                        // );
                    } else {
                        return of('');
                    }
                }),
                map((isUpdated) => {
                    if (isUpdated) {
                        this.loadUsers(this.pageIndex, this.pageSize);
                        this.notify.showTranslateMessage('UpdatedSuccessfully');
                    }
                }),
                catchError((): any => {
                    this.notify.showTranslateMessage('ErrorOnUpdate');
                }),
            )
            .subscribe((result) => {});
    }

    onDelete(userModel: UserModel) {
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
            .pipe(
                switchMap((dialogResult: string) => {
                    if (dialogResult) {
                        return this.userService.deleteUser(
                            Number(userModel.userId),
                        );
                    } else {
                        this.notify.showTranslateMessage('CancelDelete');
                        return of(null);
                    }
                }),
                catchError((): any => {
                    this.notify.showTranslateMessage('ErrorOnDelete');
                    return of(null);
                }),
            )
            .subscribe((result) => {
                if (result) {
                    this.loadUsers(1, this.pageSize);
                    this.notify.showTranslateMessage('DeletedSuccessfully');
                }
            });
    }
}
