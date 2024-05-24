import { HttpHeaders } from '@angular/common/http';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';

import { ApiService } from '../api/api.service';
import { environment } from '@env/environment';
import { FileService } from '../file.service';
import { IUploadedDocument } from '@app/infrastructure/models/uploaded-document';

@Injectable({
    providedIn: 'root',
})
export class DocumentService {
    private url: string = `${environment.apiRoute}`;

    constructor(
        private apiService: ApiService,
        private fileService: FileService,
        private http: HttpClient,
    ) {}

    public getUploadedDocument(documentKey: string): Observable<string> {
        const endpoint = `${this.url}/Documents?filePath=${documentKey}`;

        return this.apiService.get(endpoint);
    }

    public uploadDocuments(
        fileList: FileList,
    ): Observable<IUploadedDocument[]> {
        const endpoint = `${this.url}/Documents`;
        const payload = this.fileService.buildFormData(fileList);
        const headers = new HttpHeaders();
        headers.set('Content-Type', 'multipart/form-data');

        return this.apiService.post<IUploadedDocument[], FormData>(
            endpoint,
            payload,
            { headers },
        );
    }

    UploadExcel(formData: FormData) {
        let headers1 = new HttpHeaders();

        headers1.append('Content-Type', 'multipart/form-data');
        headers1.append('Accept', 'application/json');

        const httpOptions = { headers: headers1 };

        return this.http.post(
            this.url + '/Documents/UploadExcel',
            formData,
            httpOptions,
        );
    }
}
