import { Injectable } from '@angular/core';
import { CanActivate, Router, UrlTree } from '@angular/router';
import { UserType } from '@app/infrastructure/models/user';
import { Observable } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { UserService } from '../services/auth/user.service';
import { TokenService } from '../services/token.service';

@Injectable({
    providedIn: 'root',
})
export class RedirectRouteGuard implements CanActivate {
    constructor(
        private router: Router,
        private tokenService: TokenService,
        private userService: UserService,
    ) {}

    public canActivate(): Observable<UrlTree> {
        return this.tokenService.getAuthToken().pipe(
            take(1),
            map((user) => {
                if (!user.role) {
                    return this.router.parseUrl('/client');
                }
                switch (user.role) {
                    case UserType.UsersAdmin:
                    case UserType.SuperAdmin:
                    case UserType.Admin:
                    case UserType.AdminGroup:
                        return this.router.parseUrl('/landing');
                    case UserType.StudentAdmin:
                        return this.router.parseUrl('/landing');
                    case UserType.Student:
                        return this.router.parseUrl('/landing');
                    case UserType.Guest:
                        return this.router.parseUrl('/client');
                }
            }),
        );
    }
}
