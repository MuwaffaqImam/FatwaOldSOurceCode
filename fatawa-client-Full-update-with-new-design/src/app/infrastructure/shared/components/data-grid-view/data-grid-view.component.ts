import {
    Component,
    OnInit,
    ChangeDetectionStrategy,
    Input,
    Output,
    AfterViewInit,
    ViewChild,
    ChangeDetectorRef,
    HostListener,
    ElementRef,
} from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { State, ActionRowGrid } from '../../Services/CommonMemmber';
import { BehaviorSubject } from 'rxjs';
import { MatSort } from '@angular/material/sort';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { DynamicColumn } from '@app/infrastructure/models/gridColumns-model';
import * as XLSX from 'xlsx';

@Component({
    selector: 'app-data-grid-view',
    templateUrl: './data-grid-view.component.html',
    styleUrls: ['./data-grid-view.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DataGridViewComponent implements OnInit, AfterViewInit {
    @Input() gridDataSource: MatTableDataSource<any>;
    @Input() displayedColumns: string[];

    @HostListener('window:popstate', ['$event'])

    // tslint:disable-next-line:no-output-on-prefix
    @Output()
    onEditRow = new BehaviorSubject<ActionRowGrid>({
        type: State.Non,
        row: '',
    });
    @Output() onLanguageChange = new BehaviorSubject<ActionRowGrid>({
        type: State.Non,
        row: '',
    });
    // tslint:disable-next-line:no-output-on-prefix
    @Output() onDeleteRow = new BehaviorSubject<ActionRowGrid>({
        type: State.Non,
        row: '',
    });
    // tslint:disable-next-line:no-output-on-prefix
    @Output() onPagination = new BehaviorSubject<ActionRowGrid>({
        type: State.Non,
        row: '',
    });
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
    @Input() lengthData: number;
    @Input() PaginatorIndex: number;
    @Input() additionalColumns: DynamicColumn[] = [];
    @Input() isShowMainActionControls = true;
    @Input() isShowExportButton = false;

    // incomming Questions
    @Output() onInStatusRow = new BehaviorSubject<ActionRowGrid>({
        type: State.Non,
        row: '',
    });
    pageIndex = 1;
    pageSize = 10;
    @ViewChild('TABLE') table: ElementRef;

    constructor(private changeDetectorRef: ChangeDetectorRef) {}
    ngOnInit(): void {}

    ngAfterViewInit(): void {
        this.gridDataSource.sort = this.sort;
    }
    onPopState(event) {
        console.log('Back button pressed');
    }
    onEditClick(rowClicked) {
        const actionGrid: ActionRowGrid = {
            type: State.Edit,
            row: rowClicked,
        };
        this.onEditRow.next(actionGrid);
    }
    onChangeLanguageClick(rowClicked) {
        const actionGrid: ActionRowGrid = {
            type: State.Language,
            row: rowClicked,
        };
        this.onLanguageChange.next(actionGrid);
    }
    onDeleteClick(rowClicked) {
        const actionGrid: ActionRowGrid = {
            type: State.Delete,
            row: rowClicked,
        };
        this.onDeleteRow.next(actionGrid);
    }

    onPaginationClick(event) {
        event.pageIndex += 1;
        const PaginationGrid: ActionRowGrid = {
            type: State.Pagination,
            row: event,
        };
        this.onPagination.next(PaginationGrid);
    }

    onStatusClick(rowClicked, state: State) {
        const actionGrid: ActionRowGrid = { type: state, row: rowClicked };
        this.onInStatusRow.next(actionGrid);
    }

    getDisplayColumns() {
        if (!this.isShowMainActionControls) {
            return this.displayedColumns.concat(
                ...this.additionalColumns.map((e) => e.headerName),
            );
        }
        return this.displayedColumns
            .concat(...this.additionalColumns.map((e) => e.headerName))
            .concat(...['Actions']);
    }

    onFirstPage() {
        this.paginator.firstPage();
        this.changeDetectorRef.detectChanges();
    }

    onLastPage() {
        this.paginator.lastPage();
        this.changeDetectorRef.detectChanges();
    }

    onSelectPageIndex(pageIndex: number) {
        this.paginator.pageIndex = pageIndex - 1;

        const event: PageEvent = {
            length: this.paginator.length,
            pageIndex: this.paginator.pageIndex,
            pageSize: this.paginator.pageSize,
        };

        this.paginator.page.next(event);
    }
    exportexcel(): void {
        const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(
            this.gridDataSource.data,
        );
        const wb: XLSX.WorkBook = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');

        /* save to file */
        XLSX.writeFile(wb, 'تصدير.xlsx');
    }
}
