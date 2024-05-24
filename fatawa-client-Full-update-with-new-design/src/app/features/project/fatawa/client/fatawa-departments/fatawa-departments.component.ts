import {
    Component,
    OnInit,
    ChangeDetectionStrategy,
    ViewChild,
    ChangeDetectorRef,
    ElementRef,
} from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { FatawaService } from '@app/infrastructure/core/services/fatawa/fatawa.service';
import { NotificationService } from '@app/infrastructure/core/services/notification.service';
import {
    ActionNodeTree,
    DataTreeService,
} from '@app/infrastructure/models/nodeTree';
import { TreeItemNode } from '@app/infrastructure/models/project/department/departmentModel';
import { FatawaModel } from '@app/infrastructure/models/project/fatawa/FatawaModel';
import { FatwaType } from '@app/infrastructure/models/SystemEnum';
import { State } from '@app/infrastructure/shared/Services/CommonMemmber';
import { Observable, of } from 'rxjs';
import { catchError, map, mergeMap, switchMap, take } from 'rxjs/operators';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '@app/infrastructure/core/services/auth/user.service';

@Component({
    selector: 'app-fatawa-departments',
    templateUrl: './fatawa-departments.component.html',
    styleUrls: ['./fatawa-departments.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FatawaDepartmentsComponent implements OnInit {
    public dataSource: any;
    public pageSize = 10;
    public pageIndex = 0;
    public totalSize = 0;
    fatawaDepartments: any[] = [];
    selectedDepartments: any[] = [];
    mathhabs = [];
    fatawaTypes = [];
    departmenId: number = 0;
    departmentname: string = '';
    departmentinnername: string = '';
    mathhabId: number = 0;
    fatwaTypeId: number = 0;
    searchTreeData: TreeItemNode[] = [];
    defaultSelectedMathhab: number = 0;
    routeSubscription;
    departmentId: number;
    constructor(
        private fatawaService: FatawaService,
        private notify: NotificationService,
        private changeDetectorRef: ChangeDetectorRef,
        private dataTreeService: DataTreeService,
        public _router: Router,
        private route: ActivatedRoute,
        private userService: UserService,
    ) {}

    ngOnInit(): void {
        this.routeSubscription = this.route.params.subscribe((params) => {
            this.departmentId = +params['id'];
        });

        this.userService.languageChangedSubject$
            .pipe(switchMap(() => this.getFatawaDepartments(this.departmentId)))
            .subscribe((data) => {
                this.fatawaDepartments = this.selectedDepartments = data;
            });

        this.getdepartmentName(this.departmentId);
        this.dataSource = new MatTableDataSource<FatawaModel>([]);
        // this.dataSource.paginator = this.paginator;
        this.getFatawa(
            this.departmentId,
            this.fatwaTypeId,
            this.pageIndex,
            this.pageSize,
        );
    }

    getFatawaDepartments(deptId: number) {
        return this.fatawaService.GetDepartmentsByDepartmentIdLevelId(
            deptId,
            3,
        );
    }
    getdepartment(): void {
        this.fatawaService.getFatawaDeparments().subscribe(
            (data) => {
                this.fatawaDepartments = data;

                console.log(data.length);
            },
            () => {
                this.notify.showTranslateMessage('ErrorOnGittingFatawas');
            },
        );
    }
    getFatawa(
        departmenId: number,
        typeId: number,
        pageIndex: number,
        pageSize: number,
    ) {
        this.fatawaService
            .getClientFatawaFiltered(
                departmenId,
                typeId,
                pageIndex + 1,
                pageSize,
            )
            .subscribe(
                (data) => {
                    this.dataSource.data = data.dataRecord;
                    this.totalSize = data.countRecord;
                    this.changeDetectorRef.detectChanges();
                },
                () => {
                    this.notify.showTranslateMessage('ErrorOnGittingFatawas');
                },
            );
    }

    public getdepartmentName(depId: number): void {
        this.fatawaService
            .getDepartmentByLanguageId(depId, 1)
            .pipe(take(1))
            .subscribe((data) => {
                this.departmentname = data.nodeName;
            });
    }

    gotoDepartment(depId: number) {
        this._router.navigate(['/client/department/' + depId]);
        this.getFatawaDepartments(depId);
        this.getFatawa(depId, this.fatwaTypeId, this.pageIndex, this.pageSize);
        this.getdepartmentName(depId);
    }
}
