import {
    ChangeDetectionStrategy,
    Component,
    ElementRef,
    OnInit,
    ViewChild,
} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { UserService } from '@app/infrastructure/core/services/auth/user.service';
import { DocumentService } from '@app/infrastructure/core/services/documents/document.service';
import { FatawaService } from '@app/infrastructure/core/services/fatawa/fatawa.service';
import { TreeItemNode } from '@app/infrastructure/models/project/department/departmentModel';
import { FatawaModel } from '@app/infrastructure/models/project/fatawa/FatawaModel';
import { ConfirmDialogComponent } from '@app/infrastructure/shared/components/confirm-dialog/confirm-dialog.component';
import { DataGridViewComponent } from '@app/infrastructure/shared/components/data-grid-view/data-grid-view.component';
import {
    ActionRowGrid,
    FatawaType,
    State,
} from '@app/infrastructure/shared/Services/CommonMemmber';
import { NotificationService } from '@core/services/notification.service';
import { catchError, mergeMap, switchMap, tap, map } from 'rxjs/operators';
@Component({
    selector: 'app-fatawa-import',
    templateUrl: './fatawa-import.component.html',
    styleUrls: ['./fatawa-import.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FatawaImportComponent implements OnInit {
    public paginationIndex = 0;
    public pageIndex = 1;
    public pageSize = 10;
    public length = 0;
    fatawaTypes = [];
    public dataSource = new MatTableDataSource<FatawaModel>([]);
    selectedDepartments: TreeItemNode[] = [];
    fatawaDepartments: TreeItemNode[] = [];
    muftiList = [];
    mathhabs = [];
    message: string;
    departmentId: number = 0;
    searchText: string = '';
    editorForm: FormGroup;
    public isInProgress = false;
    submitted = false;

    @ViewChild('fileInput') fileInput;
    @ViewChild('searchInputDepartment') searchInputDepartment: ElementRef;
    @ViewChild(DataGridViewComponent) sharedDataGridView: DataGridViewComponent;

    constructor(
        private router: Router,
        private fatawaService: FatawaService,
        private dialog: MatDialog,
        private notify: NotificationService,
        private userService: UserService,
        private documentService: DocumentService,
        private formBuilder: FormBuilder,
    ) {
        this.editorForm = this.formBuilder.group({
            fatawaDepartmentId: ['', [Validators.required]],
            fatawaMathhabId: ['', [Validators.required]],
            muftiId: ['', [Validators.required]],
            fatawaTypeId: ['', [Validators.required]],
        });
        this.userService.languageChangedSubject$
            .pipe(
                switchMap((languageId: string) => this.getFatawaDepartments()),
            )
            .subscribe((data) => {
                this.fatawaDepartments = this.selectedDepartments = data;
            });
    }

    changeMathhabs(e) {
        this.editorForm.controls.fatawaMathhabId.setValue(e.target.value);
    }
    changeFatawaDepartmens(e) {
        this.editorForm.controls.fatawaDepartmentId.setValue(e.target.value);
    }
    changeFatawaType(e) {
        this.editorForm.controls.fatawaTypeId.setValue(e.target.value);
    }
    get SearchInputDepartmentValue() {
        return this.searchInputDepartment.nativeElement.value;
    }

    get SearchInputSearchBox() {
        return this.searchText;
    }

    ngOnInit(): void {
        this.getFatawaMuftiList();
        this.getMathhabs();
        this.getFatwaTypes();

        //  this.getAllFatawasImported(this.pageIndex, this.pageSize);
    }

    getFatawaMuftiList() {
        return this.userService
            .getAllSuperAdminsAsync()
            .pipe(
                map((data) => {
                    this.muftiList = data;
                }),
                catchError((): any => {
                    this.notify.showTranslateMessage('ErrorLoadLanguages');
                }),
            )
            .subscribe((result) => {});
    }

    getMathhabs() {
        return this.fatawaService
            .getFatawaMathhabs()
            .pipe(
                map((data) => {
                    this.mathhabs = data;
                }),
                catchError((): any => {
                    this.notify.showTranslateMessage('ErrorLoadLanguages');
                }),
            )
            .subscribe((result) => {});
    }

    getFatwaTypes() {
        return this.fatawaService
            .getFatawaTypes()
            .pipe(
                map((data) => {
                    this.fatawaTypes = data;
                }),
                catchError((): any => {
                    this.notify.showTranslateMessage('ErrorLoadLanguages');
                }),
            )
            .subscribe((result) => {});
    }
    onActionRowGrid(actionGrid: ActionRowGrid): void {
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

    getAllFatawasImported(pageIndex: number, pageSize: number): void {
        this.fatawaService
            .getLiveAndArchiveFatawaFiltered(0, '', 1, pageIndex, pageSize)
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

    addControl(resultClick: State): void {
        switch (resultClick) {
            case State.Add:
                this.router.navigateByUrl('fatawa/addFatwa');
                break;
            case State.ClearSearchBox:
                this.clearSearch();
                break;
        }
    }

    deleteFatawa(id): void {
        this.dialog
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
                                this.getAllFatawasImported(
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
            .getLiveAndArchiveFatawaFiltered(
                this.departmentId,
                this.searchText,
                FatawaType.FatawaArchived,
                this.pageIndex,
                this.pageSize,
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
            ? this.getAllFatawasImported(this.pageIndex, this.pageSize)
            : this.getSearchFatwa();
    }

    clearSearch() {
        this.pageIndex = 1;
        this.departmentId = 0;
        this.searchText = '';
        this.searchInputDepartment.nativeElement.value = '';

        this.getAllFatawasImported(this.pageIndex, this.pageSize);
        this.sharedDataGridView.onFirstPage();
    }
    uploadFile() {
        let formData = new FormData();
        formData.append('upload', this.fileInput.nativeElement.files[0]);
        this.documentService.UploadExcel(formData).subscribe((result) => {
            this.message = result.toString();
            this.getAllFatawasImported(this.pageIndex, this.pageSize);
        });
    }

    submitFatawa(isPublish: boolean): void {
        this.isInProgress = true;
        this.submitted = true;

        if (this.editorForm.invalid) {
            return;
        }

        this.saveFatawa(isPublish);
        this.getAllFatawasImported(this.pageIndex, this.pageSize);

        // observableMethod
        //     .pipe(
        //         mergeMap((data) => {
        //             if (data && !isPublish) {
        //                 //Invoke For SuperAdmin only
        //                 this.notify.invokeAddedNewFatwa();
        //                 return of(0);
        //             } else if (data && isPublish) {
        //                 return this.questionService.getUserIdAddedQuestion(
        //                     this.editorForm.controls.questionId.value,
        //                 );
        //             }
        //         }),
        //         mergeMap((userId: number) => {
        //             if (userId > 0) {
        //                 this.notify.invokePublishedNewFatwa(Number(userId));
        //             }
        //             return of(true);
        //         }),
        //         catchError(() => {
        //             this.isInProgress = false;
        //             return of(null);
        //         }),
        //     )
        //     .subscribe((value) => {
        //         this.isInProgress = false;

        //         if (value) {
        //             this.notify.showTranslateMessage('UpdatedSuccessfully');
        //         } else {
        //             this.notify.showTranslateMessage('SaveFailed');
        //         }

        //         // if (this.fatawaTypeId === FatwaType.FatawaArchived) {
        //         //     void this.router.navigate(['fatawa/fatawa-archived']);
        //         // } else {
        //         //     void this.router.navigate(['fatawa/fatawa-live']);
        //         // }
        //     });
    }

    saveFatawa(isPublish: boolean) {
        const fatawaModel = this.editorForm.value;
        this.fatawaService
            .updateImportedFatawa(fatawaModel)
            .subscribe((result) => {
                this.isInProgress = false;
                this.editorForm.reset();
                if (result) {
                    this.notify.showTranslateMessage(
                        'AddSuccessfullyImport',
                        false,
                    );
                } else {
                    this.notify.showTranslateMessage('ImportFailed');
                }
            });
    }
}
