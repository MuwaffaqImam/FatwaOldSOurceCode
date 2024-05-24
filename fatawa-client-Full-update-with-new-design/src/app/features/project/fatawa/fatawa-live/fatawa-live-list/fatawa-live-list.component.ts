import {
    Component,
    OnInit,
    ChangeDetectionStrategy,
    ViewChild,
    ElementRef,
} from '@angular/core';
import {
    State,
    ActionRowGrid,
    FatawaType,
} from '@app/infrastructure/shared/Services/CommonMemmber';
import { MatTableDataSource } from '@angular/material/table';
import { PageEvent } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '@app/infrastructure/shared/components/confirm-dialog/confirm-dialog.component';
import { NotificationService } from '@core/services/notification.service';
import { FatawaModel } from '@app/infrastructure/models/project/fatawa/FatawaModel';
import { FatawaService } from '@app/infrastructure/core/services/fatawa/fatawa.service';
import { Router } from '@angular/router';
import { catchError, mergeMap, switchMap, tap } from 'rxjs/operators';
import { TreeItemNode } from '@app/infrastructure/models/project/department/departmentModel';
import { UserService } from '@app/infrastructure/core/services/auth/user.service';
import { DataGridViewComponent } from '@app/infrastructure/shared/components/data-grid-view/data-grid-view.component';
import { MatCheckboxChange } from '@angular/material/checkbox';

@Component({
    selector: 'app-fatawa-live-list',
    templateUrl: './fatawa-live-list.component.html',
    styleUrls: ['./fatawa-live-list.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FatawaLiveListComponent implements OnInit {
    public paginationIndex = 0;
    public pageIndex = 1;
    public pageSize = 10;
    public length = 0;
    public isChecked = true;
    public dataSource = new MatTableDataSource<FatawaModel>([]);
    selectedDepartments: TreeItemNode[] = [];
    fatawaDepartments: TreeItemNode[] = [];
    departmentId: number = 0;
    searchText: string = '';
    @ViewChild('searchInputDepartment') searchInputDepartment: ElementRef;
    @ViewChild(DataGridViewComponent) sharedDataGridView: DataGridViewComponent;

    constructor(
        private router: Router,
        private fatawaService: FatawaService,
        private dialog: MatDialog,
        private notify: NotificationService,
        private userService: UserService,
    ) {
        this.userService.languageChangedSubject$
            .pipe(
                switchMap((languageId: string) => this.getFatawaDepartments()),
            )
            .subscribe((data) => {
                this.fatawaDepartments = this.selectedDepartments = data;
            });
    }

    get SearchInputDepartmentValue() {
        return this.searchInputDepartment.nativeElement.value;
    }

    get SearchInputSearchBox() {
        return this.searchText;
    }

    ngOnInit(): void {
        this.getAllFatawasLive(this.pageIndex, this.pageSize);
    }

    onActionRowGrid(actionGrid: ActionRowGrid) {
        switch (actionGrid.type) {
            case State.Edit:
                this.router.navigateByUrl(
                    'fatawa/editFatwa/' + actionGrid.row.id,
                );
                break;
            case State.Delete:
                this.deleteFatawa(actionGrid.row.id);
                break;
            case State.Pagination:
                this.handlePage(actionGrid.row);
                break;
        }
    }

    getAllFatawasLive(pageIndex: number, pageSize: number) {
        this.fatawaService
            .getFatawasLive(pageIndex, pageSize)
            .pipe(
                mergeMap((paginationRecord) => {
                    this.dataSource.data = paginationRecord.dataRecord;
                    this.length = paginationRecord.countRecord;
                    return this.getFatawaDepartments();
                }),
                tap((data) => {
                    this.fatawaDepartments = this.selectedDepartments = data;
                }),
                catchError((): any => {
                    this.notify.showTranslateMessage('ErrorOnLoadData');
                }),
            )
            .subscribe((result) => {});
    }

    getFatawaDepartments() {
        return this.fatawaService.getFatawaDeparments();
    }

    searchNodeName(searchNodeName: string) {
        let result = this.filterDepartments(searchNodeName);
        this.selectedDepartments = result;
    }

    filterDepartments(searchNodeName: string): TreeItemNode[] {
        return this.fatawaDepartments.filter((department: TreeItemNode) => {
            return (
                department.nodeName?.toLowerCase().indexOf(searchNodeName) > -1
            );
        });
    }

    addControl(resultClick: State) {
        switch (resultClick) {
            case State.Add:
                this.router.navigateByUrl('fatawa/addFatwa');
                break;
            case State.ClearSearchBox:
                this.clearSearch();
                break;
        }
    }

    deleteFatawa(id) {
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
                    this.fatawaService.deleteFatawa(id).subscribe(
                        (data) => {
                            if (data) {
                                this.getAllFatawasLive(
                                    this.pageIndex,
                                    this.pageSize,
                                );
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
        this.pageIndex = 1;
        this.searchText = searchKey.trim().toLocaleLowerCase();
        this.getSearchFatwa();
        this.sharedDataGridView.onFirstPage();
    }
    // applyFilterCheckbox(ob: MatCheckboxChange) {
    //     this.pageIndex = 1;
    //     //this.searchText = searchKey.trim().toLocaleLowerCase();
    //     this.getSearchFatwa(ob.checked);
    //     this.sharedDataGridView.onFirstPage();
    // }
    clearSearchBox(searchKey: string) {
        this.clearSearch();
    }

    getFatawaFiltered(filterDepartmentId: number) {
        this.departmentId = filterDepartmentId;
        this.getSearchFatwa();
        this.sharedDataGridView.onFirstPage();

        return this.fatawaDepartments.find(
            (depart) => depart.id == filterDepartmentId,
        ).nodeName;
    }

    getSearchFatwa() {
        this.fatawaService
            .getLiveAndArchiveFatawaFilteredWithValue(
                this.departmentId,
                this.searchText,
                FatawaType.FatawaLive,
                this.pageIndex,
                this.pageSize,
                this.isChecked,
            )
            .subscribe(
                (data) => {
                    this.dataSource.data = data.dataRecord;
                    this.length = data.countRecord;
                },
                () => {
                    this.notify.showTranslateMessage('ErrorOnGittingFatawas');
                },
            );
    }

    handlePage(event?: PageEvent) {
        this.pageIndex = event.pageIndex;
        this.pageSize = event.pageSize;

        !(this.SearchInputDepartmentValue || this.SearchInputSearchBox)
            ? this.getAllFatawasLive(this.pageIndex, this.pageSize)
            : this.getSearchFatwa();
    }

    clearSearch() {
        this.pageIndex = 1;
        this.departmentId = 0;
        this.searchText = '';
        this.searchInputDepartment.nativeElement.value = '';

        this.getAllFatawasLive(this.pageIndex, this.pageSize);
        this.sharedDataGridView.onFirstPage();
    }
}
