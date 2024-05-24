import {
    ChangeDetectionStrategy,
    Component,
    EventEmitter,
    Input,
    Output,
} from '@angular/core';

import {
    decisionType,
    CaseRequest,
    ICaseDTO,
    ICaseRequest,
    ICaseUpdated,
} from '@models/project/case';
import { ITableColumns } from '@models/table';

@Component({
    selector: 'app-case-table',
    templateUrl: './case-table.component.html',
    styleUrls: ['./case-table.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CaseTableComponent {
    @Input() public columnIdList: string[] = [];
    @Input() public displayedColumns: ITableColumns[];
    @Input() public isLoadingResults: boolean = false;
    @Input() public tableData: ICaseDTO;

    @Output() public emitDecision = new EventEmitter<ICaseRequest>();

    public trackByColumnId(index: number, item: ITableColumns): number | null {
        return item ? item.columnIndex : null;
    }

    public getDecision(caseInfo: ICaseUpdated): boolean {
        return !!caseInfo.decision;
    }

    public getStatus(caseInfo: ICaseUpdated): boolean {
        return caseInfo.status === 'pending' && !caseInfo.decision;
    }

    public makeDecision(id: number, decisionValue: decisionType): void {
        const decision = new CaseRequest({
            id,
            decision: decisionValue,
        });
        this.emitDecision.emit(decision);
    }

    /** Using `*ngIf` destroys the `ViewChild` references */
    public showGrid(): string {
        return !!this.tableData.results && !!this.tableData.results.length
            ? 'inherit'
            : 'none';
    }
}
