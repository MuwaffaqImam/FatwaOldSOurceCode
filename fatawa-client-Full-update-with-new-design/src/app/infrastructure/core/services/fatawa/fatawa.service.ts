import { Injectable } from '@angular/core';
import {
    DepartmentNodeModel,
    TreeItemNode,
} from '@app/infrastructure/models/project/department/departmentModel';
import { FatawaModel } from '@app/infrastructure/models/project/fatawa/FatawaModel';
import { FatawaImportModel } from '@app/infrastructure/models/project/fatawa/FatawaImportModel';
import { LanguageModel } from '@app/infrastructure/models/project/LanguageModel';
import { environment } from '@env/environment';
import { Observable } from 'rxjs';
import { ApiService } from '../api/api.service';

@Injectable({
    providedIn: 'root',
})
export class FatawaService {
    constructor(private apiService: ApiService) {}

    getFatawasLive(pageIndex: number, pageSize: number): Observable<any> {
        return this.apiService.get(
            `${environment.apiRoute}/Fatawa/GetFatawasLive?pageIndex=${pageIndex}&pageSize= ${pageSize}`,
        );
    }

    getAllFatawasDefaultSettings(
        pageIndex: number,
        pageSize: number,
    ): Observable<any> {
        return this.apiService.get(
            `${environment.apiRoute}/Fatawa/getAllFatawasDefaultSettings`,
        );
    }

    getClientFatawaFiltered(
        departmentId: number,
        typeId: number,
        pageIndex: number,
        pageSize: number,
    ): Observable<any> {
        return this.apiService.get(
            `${environment.apiRoute}/Client/GetFatawaFiltered?departmentId=${departmentId}&typeId=${typeId}&pageIndex=${pageIndex}&pageSize=${pageSize}`,
        );
    }
    getDepartmentByLanguageId(
        departmentId: number,
        languageId: number,
    ): Observable<DepartmentNodeModel> {
        return this.apiService.get(
            `${environment.apiRoute}/Client/GetFatawaDepartmentBylanguage?departmentId=${departmentId}&languageId=${languageId}`,
        );
    }
    getLiveAndArchiveFatawaFiltered(
        departmentId: number,
        searchText: string,
        typeId: number,
        pageIndex: number,
        pageSize: number,
    ): Observable<any> {
        return this.apiService.get(
            `${environment.apiRoute}/Fatawa/GetFatawaLiveAndArchivedFiltered?departmentId=${departmentId}&searchText=${searchText}&typeId=${typeId}&pageIndex=${pageIndex}&pageSize=${pageSize}`,
        );
    }
    getLiveAndArchiveFatawaFilteredWithValue(
        departmentId: number,
        searchText: string,
        typeId: number,
        pageIndex: number,
        pageSize: number,
        sametitle: boolean,
    ): Observable<any> {
        return this.apiService.get(
            `${environment.apiRoute}/Fatawa/GetFatawaLiveAndArchivedFiltered?departmentId=${departmentId}&searchText=${searchText}&typeId=${typeId}&pageIndex=${pageIndex}&pageSize=${pageSize}&SameTitle=${sametitle}`,
        );
    }
    getFatawaWithMathabandMuftiFiltered(
        departmentId: number,
        typeId: number,
        mathhabId: number,
        muftiId: number,
        pageIndex: number,
        pageSize: number,
    ): Observable<any> {
        return this.apiService.get(
            `${environment.apiRoute}/Fatawa/getFatawaFilteredWithMathabandMufti?departmentId=${departmentId}&typeId=${typeId}&mathhabId=${mathhabId}&muftiId=${muftiId}&pageIndex=${pageIndex}&pageSize=${pageSize}`,
        );
    }

    getFatawasArchived(pageIndex: number, pageSize: number): Observable<any> {
        return this.apiService.get(
            `${environment.apiRoute}/Fatawa/GetFatawasArchived?pageIndex=${pageIndex}&pageSize= ${pageSize}`,
        );
    }
    getFatawasImported(pageIndex: number, pageSize: number): Observable<any> {
        return this.apiService.get(
            `${environment.apiRoute}/Fatawa/GetFatawasImported?pageIndex=${pageIndex}&pageSize= ${pageSize}`,
        );
    }
    getFatawaTypes(): Observable<any> {
        return this.apiService.get(
            `${environment.apiRoute}/Client/GetFatawaTypes`,
        );
    }

    getFatawaDeparments(): Observable<TreeItemNode[]> {
        return this.apiService.get(
            `${environment.apiRoute}/Client/GetFatawaDeparments`,
        );
    }
    getDepartmentsByLevelId(levelNo: number): Observable<any[]> {
        return this.apiService.get(
            `${environment.apiRoute}/Client/GetDepartmentsByLevelId?levelNo=` +
                levelNo,
        );
    }
    GetDepartmentsByDepartmentIdLevelId(
        departmentId: number,
        levelNo: number,
    ): Observable<any[]> {
        return this.apiService.get(
            `${environment.apiRoute}/Client/GetDepartmentsByDepartmentIdLevelId?departmentId=${departmentId}&levelNo=${levelNo}`,
        );
    }
    getFatawaStatuses(): Observable<any> {
        return this.apiService.get(
            `${environment.apiRoute}/Fatawa/GetFatawaStatuses`,
        );
    }

    getFatawaMathhabs(): Observable<any> {
        return this.apiService.get(
            `${environment.apiRoute}/Client/GetFatawaMathhabs`,
        );
    }

    getFatawa(id: number): Observable<FatawaModel> {
        return this.apiService.get(
            `${environment.apiRoute}/Fatawa/GetFatawa?id=${id}`,
        );
    }

    getFatawaByLanguage(
        id: number,
        languageId: number,
    ): Observable<FatawaModel> {
        return this.apiService.get(
            `${environment.apiRoute}/Client/GetFatawaByLanguage?id=${id}&languageId=${languageId}`,
        );
    }

    getFatawaDefaultSettings(id: number): Observable<FatawaModel> {
        return this.apiService.get(
            `${environment.apiRoute}/Fatawa/GetFatawaDefaultSettings?id=${id}`,
        );
    }

    getFatawaDefaultSettingsByUser(): Observable<FatawaModel> {
        return this.apiService.get(
            `${environment.apiRoute}/Fatawa/GetFatawaDefaultSettingsByUser`,
        );
    }

    deleteFatawa(id: number): Observable<FatawaModel> {
        return this.apiService.delete(
            `${environment.apiRoute}/Fatawa/Delete?id=${id}`,
        );
    }

    deleteFatawaDefaultSettings(id: number): Observable<FatawaModel> {
        return this.apiService.delete(
            `${environment.apiRoute}/Fatawa/DeleteFatawaDefaultSetting?id=${id}`,
        );
    }

    saveFatawa(model: FatawaModel): Observable<number> {
        return this.apiService.post(
            `${environment.apiRoute}/Fatawa/SaveFatawa`,
            model,
        );
    }
    updateImportedFatawa(model: FatawaImportModel): Observable<number> {
        return this.apiService.post(
            `${environment.apiRoute}/Fatawa/updateImportedFatawa`,
            model,
        );
    }

    saveFatawaDefaultSettings(model: FatawaModel): Observable<number> {
        return this.apiService.post(
            `${environment.apiRoute}/Fatawa/saveFatawaDefaultSettings`,
            model,
        );
    }

    getTranslatorAutoCompleteValues(): Observable<string[]> {
        return this.apiService.get(
            `${environment.apiRoute}/Fatawa/GetTranslators`,
        );
    }

    getSearchFatwa(
        clientSearchText: string,
        mathhabId: number,
        fatwaTypeId: number,
        pageIndex: number,
        pageSize: number,
    ): Observable<any> {
        return this.apiService.get(
            `${environment.apiRoute}/Client/GetClientFatawaFiltered?clientSearchText=${clientSearchText}&mathhabId=${mathhabId}&fatwaTypeId=${fatwaTypeId}&pageIndex=${pageIndex}&pageSize=${pageSize}`,
        );
    }

    getFatawaLanguages(fatwaId: number): Observable<LanguageModel[]> {
        return this.apiService.get(
            `${environment.apiRoute}/Client/getFatawaLanguages?fatwaId=${fatwaId}`,
        );
    }

    getFatawaRelations(
        fatwaId: number,
        languageId: number,
    ): Observable<FatawaModel[]> {
        return this.apiService.get(
            `${environment.apiRoute}/Client/getFatawaRelations?fatwaId=${fatwaId}&languageId=${languageId}`,
        );
    }
}
