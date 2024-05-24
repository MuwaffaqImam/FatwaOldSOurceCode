import {
    ChangeDetectionStrategy,
    Component,
    EventEmitter,
    OnInit,
} from '@angular/core';
import { merge, Observable, of as observableOf } from 'rxjs';
import { catchError, startWith, switchMap, tap } from 'rxjs/operators';
import { CaseService } from '@core/services/case/case.service';
import { ICaseDTO, CaseDTO, QueryStatus } from '@models/project/case';

@Component({
    template: `
        <app-case-list-presentation
            [caseList]="caseList$ | async"
            [isLoadingResults]="isLoadingResults"
            [isLoaded]="isLoaded"
        ></app-case-list-presentation>
    `,
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CaseListSmartComponent implements OnInit {
    public caseList$: Observable<ICaseDTO>;
    public emitGetCaseList = new EventEmitter<void>();
    public isLoaded: boolean = false;
    public isLoadingResults: boolean = false;

    private filterPayload = new QueryStatus();

    constructor(private caseService: CaseService) {}

    public ngOnInit(): void {
        this.getCaseList();
    }

    private getCaseList(): void {
        this.caseList$ = merge(this.emitGetCaseList).pipe(
            startWith({}),
            switchMap(() => {
                this.isLoadingResults = true;
                return this.caseService.getCaseList(this.filterPayload);
            }),
            tap(() => (this.isLoaded = true)),
            tap(() => (this.isLoadingResults = false)),
            catchError(() => {
                this.isLoadingResults = false;
                return observableOf(new CaseDTO());
            }),
        );
    }
}
