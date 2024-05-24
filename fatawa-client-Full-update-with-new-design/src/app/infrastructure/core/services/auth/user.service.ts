import { Injectable } from '@angular/core';
import {
    UserNodeModel,
    UserTreeItemNode,
} from '@app/infrastructure/models/project/fatawa/MuftiTreeModal';
import { LanguageModel } from '@app/infrastructure/models/project/LanguageModel';
import { UserModel } from '@app/infrastructure/models/project/UserModel';
import { Constants } from '@app/infrastructure/utils/constants';
import { environment } from '@env/environment';
import { BehaviorSubject, Observable } from 'rxjs';
import { ApiService } from '../api/api.service';

@Injectable({
    providedIn: 'root',
})
export class UserService {
    languageChangedSubject$ = new BehaviorSubject<string>('1');

    constructor(private apiService: ApiService) {}

    getUsers(pageIndex: number, pageSize: number): Observable<any> {
        return this.apiService.get(
            `${environment.apiRoute}/User/Get?pageIndex=${pageIndex}&pageSize=${pageSize}`,
        );
    }

    GetUserFiltered(
        searchText: string,
        pageIndex: number,
        pageSize: number,
    ): Observable<any> {
        return this.apiService.get(
            `${environment.apiRoute}/User/GetFilteredUsers?searchText=${searchText}&pageIndex=${pageIndex}&pageSize=${pageSize}`,
        );
    }
    getAllMufti(): Observable<UserTreeItemNode[]> {
        return this.apiService.get(`${environment.apiRoute}/User/getAllMufti`);
    }
    getAllMuftiByLanguageId(langId: number): Observable<UserTreeItemNode[]> {
        return this.apiService.get(
            `${environment.apiRoute}/User/getAllMuftiByLanguageId?langId=` +
                langId,
        );
    }
    getMuftiListByUserId(userId: number): Observable<UserTreeItemNode[]> {
        return this.apiService.get(
            `${environment.apiRoute}/User/getMuftiListByUserId?userId=` +
                userId,
        );
    }
    deleteMuftiFromTree(id: number): Observable<any> {
        return this.apiService.delete(
            `${environment.apiRoute}/User/DeleteMuftiTree?id=` + id,
        );
    }
    addUser(usersForm: UserModel): Observable<any> {
        return this.apiService.post(`${environment.apiRoute}/User`, usersForm);
    }

    updateUser(id: number, usersForm: any): Observable<any> {
        return this.apiService.put(
            `${environment.apiRoute}/User?id=${id}`,
            usersForm,
        );
    }

    deleteUser(userId: number): Observable<any> {
        return this.apiService.delete(
            `${environment.apiRoute}/User?userId=${userId}`,
        );
    }

    getAllAdminsAsync(): Observable<UserModel[]> {
        return this.apiService.get(`${environment.apiRoute}/User/GetAllAdmins`);
    }
    getAllResearchHelperAsync(): Observable<UserModel[]> {
        return this.apiService.get(
            `${environment.apiRoute}/User/getAllResearchHelper`,
        );
    }
    getAllSuperAdminsAsync(): Observable<UserModel[]> {
        return this.apiService.get(
            `${environment.apiRoute}/User/GetAllSuperAdmins`,
        );
    }
    adduserMufti(usertreeModel: UserTreeItemNode): Observable<UserNodeModel> {
        return this.apiService.post(
            `${environment.apiRoute}/User/AddUserMufti`,
            usertreeModel,
        );
    }
    TransferQuestionToMufti(
        userId: number,
        muftiId: number,
        questionId: number,
    ): Observable<any> {
        return this.apiService.put(
            `${environment.apiRoute}/User/TransferQuestionToMufti?userId=${userId}&muftiId=${muftiId}&questionId=${questionId}`,
            null,
        );
    }
    updateUserLanguage(languageId: number): Observable<any> {
        return this.apiService.put(
            `${environment.apiRoute}/User/UpdateUserLanguage?languageId=${languageId}`,
            languageId,
        );
    }

    getLanguageInformations(): Observable<LanguageModel> {
        return this.apiService.get(
            `${environment.apiRoute}/User/GetLanguageInformations`,
        );
    }

    getAllRoles(): Observable<any> {
        return this.apiService.get(`${environment.apiRoute}/User/GetAllRoles`);
    }

    updateRoleUser(userModel): Observable<any> {
        return this.apiService.put(
            `${environment.apiRoute}/User/UpdateUserRole`,
            userModel,
        );
    }

    isTokenExist(): boolean {
        const authToken = sessionStorage.getItem('authToken');
        return authToken ? true : false;
    }

    getLanguageId(): number {
        const languageId = sessionStorage.getItem('languageId');
        return languageId ? Number(languageId) : Constants.DefaultLanguageId;
    }

    setLanguageId(languageId: string): void {
        sessionStorage.setItem('languageId', languageId);
        this.languageChangedSubject$.next(languageId);
    }

    getLanguageDir(): any {
        const languageDir = sessionStorage.getItem('languageDir');
        return languageDir ?? Constants.DefaultLanguageDirection;
    }

    setLanguageDir(languageDir: string): void {
        sessionStorage.setItem('languageDir', languageDir);
    }
}
