import { Injectable } from '@angular/core';
import { ApiService } from '../api/api.service';
import { Observable } from 'rxjs';
import { environment } from '@env/environment';
import { TagModel } from '@app/infrastructure/models/project/system-definition/tagModel';

@Injectable({
    providedIn: 'root',
})
export class TagService {
    constructor(private apiService: ApiService) {}

    GetTags(pageIndex: number, pageSize: number): Observable<any> {
        return this.apiService.get(`${environment.apiRoute}/Tag/Get`);
    }

    GetTag(id): Observable<TagModel> {
        return this.apiService.get(
            `${environment.apiRoute}/Tag/GetTag?id=` + id,
        );
    }

    AddTag(tagModel: TagModel): Observable<TagModel> {
        return this.apiService.post(
            `${environment.apiRoute}/Tag`,
            JSON.stringify(tagModel),
        );
    }

    DeleteTag(id: number): Observable<TagModel> {
        return this.apiService.delete(
            `${environment.apiRoute}/Tag/Delete?id=` + id,
        );
    }
}
