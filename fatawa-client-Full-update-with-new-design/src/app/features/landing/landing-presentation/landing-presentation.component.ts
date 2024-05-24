import { Component, ChangeDetectionStrategy } from '@angular/core';
import { IDecodedAuthToken } from '@app/infrastructure/models/auth';
import { UserType } from '@app/infrastructure/models/user';
import { TokenService } from '@core/services/token.service';
import { Observable } from 'rxjs';

/**
 * Wrapper component for all `CaseModule` routes.
 */
@Component({
    templateUrl: './landing-presentation.component.html',
    styleUrls: ['./landing-presentation.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LandingPresentationComponent {
    UserType = UserType;
    constructor(private tokenService: TokenService) {}

    public getUserInfo(): Observable<IDecodedAuthToken> {
        return this.tokenService.getAuthToken();
    }
}
