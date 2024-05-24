import { Injectable } from '@angular/core';
import { ApiService } from '../api/api.service';
import { environment } from '@env/environment';
import { Observable } from 'rxjs';
import { JobModel } from '@models/project/JobModel';

@Injectable({
    providedIn: 'root',
})
export class JobsService {
    constructor(private apiService: ApiService) {}

    getJobs(pageIndex: number, PageSize: number): Observable<any> {
        return this.apiService.get(
            `${environment.apiRoute}/Jobs/Get` +
                '?pageIndex=' +
                pageIndex +
                '&pageSize=' +
                PageSize,
        );
    }

    Post(JobsForm: JobModel): Observable<any> {
        return this.apiService.post(`${environment.apiRoute}/Jobs`, JobsForm);
    }

    Put(id: number, JobsForm: any): Observable<any> {
        return this.apiService.put(
            `${environment.apiRoute}/Jobs` + '?id=' + id,
            JobsForm,
        );
    }

    Delete(id: number): Observable<any> {
        return this.apiService.delete(`${environment.apiRoute}/Jobs?id=` + id);
    }
}
