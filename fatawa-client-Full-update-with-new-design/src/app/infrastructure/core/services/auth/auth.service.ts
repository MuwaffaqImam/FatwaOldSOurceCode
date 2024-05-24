import { HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from '@env/environment';
import { SocialUser } from 'angularx-social-login/entities/social-user';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { ApiService } from '../api/api.service';
import { TokenService } from '../token.service';

const CONTROLLER_NAME: string = 'Authentication';
@Injectable({
    providedIn: 'root',
})
export class AuthService {
    private jwtHelper: JwtHelperService;

    constructor(
        private apiService: ApiService,
        private tokenService: TokenService,
    ) {}

    setTokenAfterLogin(response: any): void {
        const user = response;
        if (user) {
            sessionStorage.setItem('authToken', user.token);
            const decodedAuthToken = this.tokenService.decodeToken(user.token);
            this.tokenService.setAuthToken(decodedAuthToken);
        }
    }

    login(logModel: any): Observable<any> {
        return this.apiService
            .post(
                `${environment.apiRoute}/${CONTROLLER_NAME}/UserLogin`,
                logModel,
            )
            .pipe(
                tap((response: any) => this.setTokenAfterLogin(response)),
                catchError((e) => throwError(e)),
            );
    }

    socialLogin(socialUser: SocialUser): Observable<any> {
        return this.apiService
            .post(
                `${environment.apiRoute}/${CONTROLLER_NAME}/SocialLogin`,
                socialUser,
            )
            .pipe(
                tap((response: any) => this.setTokenAfterLogin(response)),
                catchError((e) => throwError(e)),
            );
    }

    resetPassword(resetEmail: string): Observable<any> {
        return this.apiService
            .post(`${environment.apiRoute}/${CONTROLLER_NAME}/resetPassword`, {
                email: resetEmail,
            })
            .pipe(
                tap((response: any) => {}),
                catchError((e) => throwError(e)),
            );
    }

    isUserExists(userEmail: number): Observable<any> {
        return this.apiService.get(
            `${environment.apiRoute}/${CONTROLLER_NAME}/IsUserExists?userEmail=${userEmail}`,
        );
    }

    register(userRegister: any, fromadmin: number): Observable<any> {
        return this.apiService.post(
            `${environment.apiRoute}/${CONTROLLER_NAME}/Register?fromadmin=${fromadmin}`,
            userRegister,
        );
    }

    loadUsers(pageIndex: number, pageSize: number): Observable<any> {
        return this.apiService.get(
            `${environment.apiRoute}/${CONTROLLER_NAME}/UserLogin?pageNumber=${pageIndex}&pageSize=${pageSize}`,
        );
    }

    searchByName(userName: string): Observable<any> {
        return this.apiService.get('', {
            params: new HttpParams().set('searchUserName', userName),
        });
    }

    loggedIn(): boolean {
        try {
            const token = sessionStorage.getItem('UserToken');
            return !this.jwtHelper.isTokenExpired(token);
        } catch {
            return false;
        }
    }

    getToken(): string {
        return sessionStorage.getItem('UserToken');
    }

    isTokenExpiredDate(): boolean {
        return this.jwtHelper.isTokenExpired(this.getToken());
    }

    deleteToken(): void {
        return sessionStorage.removeItem('UserToken');
    }

    loggedOut(): void {
        sessionStorage.clear();
        this.clearSession();
    }

    private clearSession(): void {
        this.tokenService.resetAuthToken();
    }
}
