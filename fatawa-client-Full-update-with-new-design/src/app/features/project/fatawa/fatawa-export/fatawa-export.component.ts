import {
    ChangeDetectionStrategy,
    Component,
    ElementRef,
    OnInit,
    ViewChild,
} from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { UserService } from '@app/infrastructure/core/services/auth/user.service';
import { DocumentService } from '@app/infrastructure/core/services/documents/document.service';
import { FatawaService } from '@app/infrastructure/core/services/fatawa/fatawa.service';
import { TreeItemNode } from '@app/infrastructure/models/project/department/departmentModel';
import { FatawaModel } from '@app/infrastructure/models/project/fatawa/FatawaModel';
import { DataGridViewComponent } from '@app/infrastructure/shared/components/data-grid-view/data-grid-view.component';
import { NotificationService } from '@core/services/notification.service';
import { catchError, mergeMap, switchMap, tap, map } from 'rxjs/operators';
@Component({
    selector: 'app-fatawa-export',
    templateUrl: './fatawa-export.component.html',
    styleUrls: ['./fatawa-export.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FatawaExportComponent implements OnInit {
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
    selecteddepartment: any;
    selectedtype: any;
    selectedmatthab: any;
    selectedmufti: any;
    fileName = 'ExcelSheet.xlsx';
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
        // this.editorForm = this.formBuilder.group({
        //     fatawaDepartmentId: ['', [Validators.required]],
        //     fatawaMathhabId: ['', [Validators.required]],
        //     muftiId: ['', [Validators.required]],
        //     fatawaTypeId: ['', [Validators.required]],
        // });
        this.userService.languageChangedSubject$
            .pipe(
                switchMap((languageId: string) => this.getFatawaDepartments()),
            )
            .subscribe((data) => {
                this.fatawaDepartments = this.selectedDepartments = data;
            });
    }

    // changeMathhabs(e) {
    //     this.editorForm.controls.fatawaMathhabId.setValue(e.target.value);
    // }
    // changeFatawaDepartmens(e) {
    //     this.editorForm.controls.fatawaDepartmentId.setValue(e.target.value);
    // }
    // changeFatawaType(e) {
    //     this.editorForm.controls.fatawaTypeId.setValue(e.target.value);
    // }
    get SearchInputDepartmentValue() {
        return this.searchInputDepartment.nativeElement.value;
    }

    ngOnInit(): void {
        this.getFatawaMuftiList();
        this.getMathhabs();
        this.getFatwaTypes();
        this.SearchFatawa(1, 1, 1, 1, this.pageIndex, this.pageSize);
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

    SearchFatawa(
        departId: number,
        typeId: number,
        mathhabId: number,
        muftiId: number,
        pageIndex: number,
        pageSize: number,
    ): void {
        if (departId === undefined) {
            departId = 0;
        }
        if (typeId === undefined) {
            typeId = 0;
        }
        if (mathhabId === undefined) {
            mathhabId = 0;
        }
        if (muftiId === undefined) {
            muftiId = 0;
        }
        this.fatawaService
            .getFatawaWithMathabandMuftiFiltered(
                departId,
                typeId,
                mathhabId,
                muftiId,
                pageIndex,
                pageSize,
            )
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

    srchFatawa(): void {
        this.SearchFatawa(
            this.selecteddepartment,
            this.selectedtype,
            this.selectedmatthab,
            this.selectedmufti,
            this.pageIndex,
            this.pageSize,
        );
    }
}
