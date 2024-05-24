import {
    ChangeDetectionStrategy,
    Component,
    EventEmitter,
    Input,
    Output,
} from '@angular/core';

import { ICaseRequest, ICaseDTO, statusType } from '@models/project/case';
import { ITableColumns } from '@models/table';

@Component({
    selector: 'app-case-list-presentation',
    templateUrl: './case-list-presentation.component.html',
    styleUrls: ['./case-list-presentation.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CaseListPresentationComponent {
    @Input() public caseList: ICaseDTO;
    @Input() public isLoaded: boolean = false;
    @Input() public isLoadingResults: boolean = false;

    @Output() public emitDecision = new EventEmitter<ICaseRequest>();

    public displayedColumns: ITableColumns[] = [
        {
            columnIndex: 0,
            columnId: 'name',
            columnName: 'Inmate Name',
        },
        {
            columnIndex: 3,
            columnId: 'gender',
            columnName: 'Gender',
        },
    ];

    public columnIdList = this.displayedColumns.map(
        (column) => column.columnId,
    );

    constructor() {
        const elements = ['dob', 'status', 'decision'];
        this.columnIdList.push(...elements);
    }

    public makeDecision(event: ICaseRequest): void {
        this.emitDecision.emit(event);
    }
}
