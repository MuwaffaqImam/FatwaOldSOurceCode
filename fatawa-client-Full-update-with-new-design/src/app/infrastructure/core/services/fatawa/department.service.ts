import { Injectable } from '@angular/core';
import { ApiService } from '../api/api.service';
import { Observable } from 'rxjs/internal/Observable';
import {
    DepartmentNodeModel,
    TreeItemNode,
} from '@app/infrastructure/models/project/department/departmentModel';
import { environment } from '@env/environment';
import { HttpParams } from '@angular/common/http';
import { DragDropModel } from '@app/infrastructure/models/project/dragDropModel';

@Injectable({
    providedIn: 'root',
})
export class FatawaDepartmentService {
    constructor(private apiService: ApiService) {}

    getAllDepartmentsByLanguageId(): Observable<TreeItemNode[]> {
        return this.apiService.get(
            `${environment.apiRoute}/FatawaDepartments/GetDaprtmentsByLanguageId`,
        );
    }

    getDepartmentByLanguageId(
        departmentId: number,
        languageId: number,
    ): Observable<DepartmentNodeModel> {
        return this.apiService.get(
            `${environment.apiRoute}/FatawaDepartments/GetFatawaDepartmentBylanguage?departmentId=${departmentId}&languageId=${languageId}`,
        );
    }

    addDepartment(
        departmentNodeModel: DepartmentNodeModel,
    ): Observable<TreeItemNode> {
        return this.apiService.post(
            `${environment.apiRoute}/FatawaDepartments/AddDepartment`,
            departmentNodeModel,
        );
    }

    updateDepartment(
        fatawaDepartmentModel: DepartmentNodeModel,
    ): Observable<any> {
        return this.apiService.put(
            `${environment.apiRoute}/FatawaDepartments/UpdateDepartment`,
            fatawaDepartmentModel,
        );
    }

    deleteDepartment(id: number): Observable<any> {
        return this.apiService.delete(
            `${environment.apiRoute}/FatawaDepartments/DeleteDepartment?id=` +
                id,
        );
    }

    getDepartmentsByLevelId(levelNo: number): Observable<any> {
        return this.apiService.get(
            `${environment.apiRoute}/FatawaDepartments/GetDepartmentsByLevelId?levelNo=` +
                levelNo,
        );
    }

    updateDropDepartment(draegDropModel: DragDropModel): Observable<any> {
        return this.apiService.put(
            `${environment.apiRoute}/FatawaDepartments/UpdateDropDepartment`,
            draegDropModel,
        );
    }
}
