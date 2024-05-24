import { Injectable } from '@angular/core';
import {
    HttpEvent,
    HttpInterceptor,
    HttpHandler,
    HttpRequest,
} from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, throwError as observableThrowError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from '@core/services/auth/auth.service';

@Injectable()
export class ApiInterceptorService implements HttpInterceptor {
    private AUTH_HEADER = 'Authorization';
    private LANG_HEADER = 'Accept-Language';

    constructor(private authService: AuthService, private router: Router) {}

    intercept<T>(
        req: HttpRequest<T>,
        next: HttpHandler,
    ): Observable<HttpEvent<T>> {
        req = this.addHeaderParameters(req);

        return next.handle(req).pipe(
            catchError((error) => {
                switch (error.status) {
                    case 401:
                    case 403:
                        this.logoutOnAuthError();
                        break;
                }
                return observableThrowError(error);
            }),
        );
    }

    private addHeaderParameters<T>(request: HttpRequest<T>): HttpRequest<T> {
        const authToken = sessionStorage.getItem('authToken');
        const userLanguage = sessionStorage.getItem('languageId');
        // If we do not have a token yet then we should not set the header.
        // Here we could first retrieve the token from where we store it.

        // TODO: Consider this flow.
        if (!authToken && !userLanguage) {
            return request;
        } else if (!userLanguage) {
            return request.clone({
                headers: request.headers.set(
                    this.AUTH_HEADER,
                    `Bearer ${authToken}`,
                ),
            });
        } else if (!authToken) {
            return request.clone({
                headers: request.headers.set(this.LANG_HEADER, userLanguage),
            });
        }

        return request.clone({
            headers: request.headers
                .set(this.AUTH_HEADER, `Bearer ${authToken}`)
                .set(this.LANG_HEADER, userLanguage),
        });
    }
    /**
     * Initial native behavior for 403s.
     */
    private logoutOnAuthError(): void {
        this.authService.loggedOut();
        this.router.navigateByUrl('/auth');
    }
}
