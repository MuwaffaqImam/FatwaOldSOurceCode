import { Injectable } from '@angular/core';
import { ApiService } from '../api/api.service';
import { environment } from '@env/environment';
import { Observable } from 'rxjs';
import { LanguageModel } from '@models/project/LanguageModel';

@Injectable({
    providedIn: 'root',
})
export class LanguagesService {
    constructor(private apiService: ApiService) {}

    getAllLanguages(): Observable<LanguageModel[]> {
        return this.apiService.get(
            `${environment.apiRoute}/Language/GetAllLanguages`,
        );
    }

    getLanguages(pageIndex: number, pageSize: number): Observable<any> {
        return this.apiService.get(
            `${environment.apiRoute}/Language/GetLanguages?pageIndex=${pageIndex}&pageSize= ${pageSize}`,
        );
    }

    getLanguageById(id: number): Observable<LanguageModel> {
        return this.apiService.get(
            `${environment.apiRoute}/Language/GetLanguageById?id=${id}`,
        );
    }

    addLanguage(languageModel: LanguageModel): Observable<number> {
        return this.apiService.post(
            `${environment.apiRoute}/Language/AddLanguage`,
            languageModel,
        );
    }

    updateLanguage(languageModel: LanguageModel): Observable<any> {
        return this.apiService.put(
            `${environment.apiRoute}/Language/UpdateLanguage`,
            languageModel,
        );
    }

    deleteLanguage(id: number): Observable<any> {
        return this.apiService.delete(
            `${environment.apiRoute}/Language/DeleteLanguage?id= ${id}`,
        );
    }
}
