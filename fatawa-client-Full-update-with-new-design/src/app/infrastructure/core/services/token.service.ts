import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { DecodedAuthToken, IDecodedAuthToken } from '@models/auth';
import { RoleDTO } from '@models/role';
import { UserType } from '@app/infrastructure/models/user';

@Injectable({
    providedIn: 'root',
})
export class TokenService {
    public authToken$ = new BehaviorSubject<IDecodedAuthToken>(
        this.decodeToken(sessionStorage.getItem('authToken')) ||
            new DecodedAuthToken(),
    );

    constructor(private jwtHelper: JwtHelperService) {}

    public getAuthToken(): Observable<IDecodedAuthToken> {
        return this.authToken$.asObservable();
    }

    public setAuthToken(token: IDecodedAuthToken): void {
        this.authToken$.next(token);
    }

    public resetAuthToken(): void {
        this.authToken$.next(new DecodedAuthToken());
    }

    public decodeToken(token: string): IDecodedAuthToken {
        try {
            const decodedToken: IDecodedAuthToken = this.jwtHelper.decodeToken(
                token,
            );

            return decodedToken;
        } catch (error) {
            throw new Error(error);
        }
    }

    public isUsersAdmin(): Observable<boolean> {
        return this.getAuthToken().pipe(
            map((user) => user.role && RoleDTO.isUsersAdminRoleType(user.role)),
        );
    }
    public isStudentAdmin(): Observable<boolean> {
        return this.getAuthToken().pipe(
            map(
                (user) =>
                    user.role && RoleDTO.isStudentsAdminRoleType(user.role),
            ),
        );
    }
    public isSuperAdmin(): Observable<boolean> {
        return this.getAuthToken().pipe(
            map((user) => user.role && RoleDTO.isSuperAdminRoleType(user.role)),
        );
    }

    public isAdmin(): Observable<boolean> {
        return this.getAuthToken().pipe(
            map((user) => user.role && RoleDTO.isAdminRoleType(user.role)),
        );
    }

    public isStudent(): Observable<boolean> {
        return this.getAuthToken().pipe(
            map((user) => user.role && RoleDTO.isStudentRoleType(user.role)),
        );
    }

    public isGuest(): Observable<boolean> {
        return this.getAuthToken().pipe(
            map((user) => user.role && RoleDTO.isGuestRoleType(user.role)),
        );
    }

    public isRolesMatch(rolesMatch: UserType[]): Observable<boolean> {
        return this.getAuthToken().pipe(
            map(
                (user) =>
                    user.role && RoleDTO.isMatchRoleType(user.role, rolesMatch),
            ),
        );
    }

    public isLoggedInUser(): Observable<boolean> {
        return this.getAuthToken().pipe(
            map((user) => user.role && RoleDTO.isRoleType(user.role)),
        );
    }

    public isSelf(userGuid: number): Observable<boolean> {
        return this.getAuthToken().pipe(
            map((user) => user.nameid === userGuid),
        );
    }

    public getUserId(): Observable<number> {
        return this.getAuthToken().pipe(map((user) => Number(user.nameid)));
    }
    public getUserFullName(): Observable<string> {
        return this.getAuthToken().pipe(map((user) => user.unique_name));
    }
}
