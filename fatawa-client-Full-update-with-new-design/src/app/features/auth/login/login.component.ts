import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NotificationService } from '@app/infrastructure/core/services/notification.service';
import { AuthService } from '@core/services/auth/auth.service';
import {
    FacebookLoginProvider,
    GoogleLoginProvider,
    SocialAuthService,
    SocialUser,
} from 'angularx-social-login';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LoginComponent implements OnInit {
    public loginForm: FormGroup;
    public isInProgress: boolean = false;
    public isHidePassword: boolean = true;
    constructor(
        private formBuilder: FormBuilder,
        private authService: AuthService,
        private router: Router,
        private socialAuthService: SocialAuthService,
        private notify: NotificationService,
    ) {}

    ngOnInit(): void {
        localStorage.clear();
        this.ngInitialControlForm();
    }

    ngInitialControlForm() {
        this.loginForm = this.formBuilder.group({
            email: [
                null,
                Validators.compose([Validators.required, Validators.email]),
            ],
            password: [null, Validators.compose([Validators.required])],
        });
    }

    logIn() {
        this.isInProgress = true;
        this.authService.login(this.loginForm.value).subscribe(
            (next) => {
                this.notify.initialConnectionsForUser();
                void this.router.navigateByUrl('');
            },
            (error) => {
                this.isInProgress = false;
                this.notify.showTranslateMessage('InvalidEmailMessage');
            },
            () => {
                this.loginForm.reset();
                this.isInProgress = false;
            },
        );
    }

    registerRedirect() {
        this.router.navigateByUrl('/auth/register');
    }

    public socialSignIn(socialProvider: string): void {
        let socialPlatformProvider: Promise<SocialUser>;
        if (socialProvider === 'facebook') {
            socialPlatformProvider = this.socialAuthService.signIn(
                FacebookLoginProvider.PROVIDER_ID,
            );
        } else if (socialProvider === 'google') {
            socialPlatformProvider = this.socialAuthService.signIn(
                GoogleLoginProvider.PROVIDER_ID,
            );
        }

        void socialPlatformProvider.then((socialUser: SocialUser) => {
            this.proceedSocialResponse(socialUser);
        });
    }

    proceedSocialResponse(socialUser: SocialUser): void {
        this.authService.socialLogin(socialUser).subscribe(
            (next) => {
                this.notify.initialConnectionsForUser();
                void this.router.navigateByUrl('');
            },
            (error) => {
                this.isInProgress = false;
                this.notify.showTranslateMessage('InvalidEmailMessage');
            },
            () => {
                this.loginForm.reset();
                this.isInProgress = false;
            },
        );
    }
}
