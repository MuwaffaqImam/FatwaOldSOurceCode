import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApiService } from '../api/api.service';
import { environment } from '@env/environment';
import {
    CaseDTO,
    CaseUpdated,
    ICase,
    ICaseDTO,
    ICaseUpdated,
    IQueryStatus,
} from '@models/project/case';

@Injectable({
    providedIn: 'root',
})
export class CaseService {
    private url: string = `${environment.apiRoute}`;

    constructor(private apiService: ApiService) {}

    public getCaseList(queryParams?: IQueryStatus): Observable<ICaseDTO> {
        const endpoint = queryParams?.status
            ? `${this.url}/status/${queryParams.status}`
            : `${this.url}/users`;

        return this.apiService.get<ICase[]>(endpoint).pipe(
            map((response) =>
                response.map((caseInfo) =>
                    this.mapCaseInfoToReadonlyView(caseInfo),
                ),
            ),
            map((mappedResponse) =>
                this.mapResultsToNestedStructure(mappedResponse),
            ),
        );
    }

    private mapCaseInfoToReadonlyView(caseInfo: ICase): ICaseUpdated {
        const formattedCase = new CaseUpdated();
        for (const property in formattedCase) {
            if (formattedCase.hasOwnProperty(property)) {
                formattedCase[property] =
                    property === 'name'
                        ? this.formatInmateName(caseInfo)
                        : caseInfo[property];
            }
        }

        return formattedCase;
    }

    private formatInmateName(caseInfo: ICase): string {
        return caseInfo.middleName
            ? `${caseInfo.lastName}, ${caseInfo.firstName} ${caseInfo.middleName}`
            : `${caseInfo.lastName}, ${caseInfo.firstName}`;
    }

    /**
     * Temporary shim; transform this data client side to have more control over its display.
     */
    private mapResultsToNestedStructure(caseList: ICaseUpdated[]): ICaseDTO {
        return new CaseDTO({ results: caseList });
    }
}
