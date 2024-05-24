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
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { Router } from '@angular/router';

@Component({
    selector: 'app-fatawa-full-list',
    templateUrl: './fatawa-full-list.component.html',
    styleUrls: ['./fatawa-full-list.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FatawaFullListComponent implements OnInit {
    public dataSource: any;
    public pageSize = 10;
    public pageIndex = 0;
    public totalSize = 0;

    @ViewChild(MatPaginator) paginator: MatPaginator;
    fatawaDepartments: TreeItemNode[] = [];
    mathhabs = [];
    fatawaTypes = [];
    departmenId: number = 0;
    mathhabId: number = 0;
    fatwaTypeId: number = 0;
    searchTreeData: TreeItemNode[] = [];
    @ViewChild('searchInputDepartment') searchInputDepartment: ElementRef;
    @ViewChild('searchInputFatwa') searchInputFatwa: ElementRef;
    defaultSelectedMathhab: number = 0;
    fatwaType = Object.keys(FatwaType)
        .filter((f) => !isNaN(Number(f)))
        .map((key) => FatwaType[key]);
    selectedFatwaTypeId = 0;
    constructor(
        private fatawaService: FatawaService,
        private notify: NotificationService,
        private changeDetectorRef: ChangeDetectorRef,
        private dataTreeService: DataTreeService,
        public _router: Router,
    ) {}
    get SearchInputFatwaValue() {
        return this.searchInputFatwa.nativeElement.value;
    }
    set SearchInputFatwaValue(value: string) {
        this.searchInputFatwa.nativeElement.value = value;
    }
    onActionNodeClick(actionNodeTree: ActionNodeTree) {
        switch (actionNodeTree.type) {
            case State.NodeSearchFatawa:
                this.pageIndex = 0;
                this.pageSize = 10;
                this.getFatawa(
                    actionNodeTree.nodeInfo.id,
                    this.fatwaTypeId,
                    this.pageIndex,
                    this.pageSize,
                );
                this.departmenId = actionNodeTree.nodeInfo.id;
                break;
        }
    }
    ngOnInit(): void {
        this.dataSource = new MatTableDataSource<FatawaModel>([]);
        this.dataSource.paginator = this.paginator;
        this.getFatawa(
            this.departmenId,
            this.fatwaTypeId,
            this.pageIndex,
            this.pageSize,
        );
    }

    getFatawaTypes() {
        this.fatawaService
            .getFatawaTypes()
            .pipe(
                mergeMap((data) => {
                    this.fatawaTypes = data;
                    return this.fatawaService.getFatawaMathhabs();
                }),
                mergeMap((data) => {
                    this.mathhabs = data;
                    return of({});
                }),
                catchError((): any => {
                    return this.notify.showTranslateMessage(
                        'ErrorOnFatawaTypes',
                    );
                }),
            )
            .subscribe((result) => {});
    }

    getFatawaDepartmens() {
        this.fatawaService
            .getFatawaDeparments()
            .pipe(
                map((fatawaDepartment) => {
                    this.fatawaDepartments = fatawaDepartment;
                }),
                catchError((): any => {
                    this.notify.showTranslateMessage('ErrorOnLoadData');
                }),
            )
            .subscribe((result) => {
                this.searchTreeData = this.fatawaDepartments;
                this.fatawaDepartments = this.buildDepartmentsTree(
                    this.fatawaDepartments,
                    '0',
                    0,
                );
                this.dataTreeService.dataChange.next(this.fatawaDepartments);
            });
    }

    buildDepartmentsTree(
        treeNodes: TreeItemNode[],
        parent: string,
        level: number,
    ): TreeItemNode[] {
        return treeNodes
            .filter((o) => o.nodeMain === parent && o.nodeLevelNumber == level)
            .map((o) => {
                const node = new TreeItemNode();
                Object.assign(node, o);
                const children = treeNodes.filter(
                    (so) => so.nodeLevelNumber >= o.nodeLevelNumber,
                );
                if (children?.length > 0) {
                    node.children = this.buildDepartmentsTree(
                        children,
                        o.nodeNumber,
                        o.nodeLevelNumber + 1,
                    );
                }
                return node;
            });
    }

    applyFilter() {
        let filteredTreeData;
        const filterText = this.searchInputDepartment.nativeElement.value;
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

        const data = this.buildDepartmentsTree(filteredTreeData, '0', 0);
        this.dataTreeService.IsExpandAllTree = !!filterText;
        this.dataTreeService.dataChange.next(data);
    }

    clearSearchDepartment() {
        this.searchInputDepartment.nativeElement.value = '';
        this.applyFilter();
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

    handlePage(event?: PageEvent) {
        this.pageIndex = event.pageIndex;
        this.pageSize = event.pageSize;

        !this.SearchInputFatwaValue
            ? this.getFatawa(
                  this.departmenId,
                  this.fatwaTypeId,
                  this.pageIndex,
                  this.pageSize,
              )
            : this.getSearchFatwa();
    }

    clearSearchFatawa() {
        this.SearchInputFatwaValue = '';
        this.defaultSelectedMathhab = 0;
        this.selectedFatwaTypeId = 0;

        this.getFatawa(
            this.departmenId,
            this.fatwaTypeId,
            this.pageIndex,
            this.pageSize,
        );
    }
    updateSelectedMathhab(mathhabId: number) {
        this.mathhabId = mathhabId;
    }

    getSearchFatwa() {
        const searchInputFatwa = this.SearchInputFatwaValue;

        this.fatawaService
            .getSearchFatwa(
                searchInputFatwa,
                this.mathhabId,
                this.selectedFatwaTypeId,
                this.pageIndex + 1,
                12,
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

    resetPaginationbeforeSearch() {
        this.pageIndex = 0;
        this.getSearchFatwa();
    }
}
