import { Injectable } from '@angular/core';
import { CanActivate, CanActivateChild, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { take, tap } from 'rxjs/operators';
import { NotificationService } from '../services/notification.service';
import { TokenService } from '../services/token.service';

@Injectable({
    providedIn: 'root',
})
export class SuperAdminAuthGuard implements CanActivate, CanActivateChild {
    constructor(
        private notificationService: NotificationService,
        private router: Router,
        private tokenService: TokenService,
    ) {}

    public canActivate(): Observable<boolean> {
        return this.tokenService.isSuperAdmin().pipe(
            take(1),
            tap((isLoggedInUser) => {
                if (!isLoggedInUser) {
                    this.notificationService.showTranslateMessage(
                        'BackToLoginPage',
                    );
                    this.router.navigateByUrl('/auth');
                }
            }),
        );
    }

    public canActivateChild(): Observable<boolean> {
        return this.canActivate();
    }
}
