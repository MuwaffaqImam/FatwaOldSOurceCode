import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { Observable } from 'rxjs';
import { GeneralSettingsModel } from '@models/project/general-settings';
import { ApiService } from './api/api.service';

@Injectable({
    providedIn: 'root',
})
export class GeneralSettingsService {
    constructor(private apiService: ApiService) {}

    getGeneralSettings(): Observable<GeneralSettingsModel[]> {
        return this.apiService.get(`${environment.apiRoute}/generalSettings`);
    }

    setGeneralSettings(
        generalSettings: GeneralSettingsModel[],
    ): Observable<any> {
        return this.apiService.post(
            `${environment.apiRoute}/generalSettings`,
            generalSettings,
        );
    }
}
