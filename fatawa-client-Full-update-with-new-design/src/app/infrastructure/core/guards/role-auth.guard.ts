import { Injectable } from '@angular/core';
import {
    ActivatedRouteSnapshot,
    CanActivate,
    CanActivateChild,
    Router,
} from '@angular/router';
import { UserType } from '@app/infrastructure/models/user';
import { Observable } from 'rxjs';
import { take, tap } from 'rxjs/operators';
import { NotificationService } from '../services/notification.service';
import { TokenService } from '../services/token.service';

@Injectable({
    providedIn: 'root',
})
export class RoleAuthGuard implements CanActivate, CanActivateChild {
    constructor(
        private notificationService: NotificationService,
        private router: Router,
        private tokenService: TokenService,
    ) {}

    public canActivate(next: ActivatedRouteSnapshot): Observable<boolean> {
        const roles = next.data.roles as UserType[];
        return this.tokenService.isRolesMatch(roles).pipe(
            take(1),
            tap((isLoggedInUser) => {
                if (!isLoggedInUser) {
                    this.notificationService.showTranslateMessage(
                        'BackToDashboard',
                    );
                    this.router.navigateByUrl('');
                }
            }),
        );
    }

    public canActivateChild(next: ActivatedRouteSnapshot): Observable<boolean> {
        return this.canActivate(next);
    }
}
