import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NotificationService } from '@app/infrastructure/core/services/notification.service';
import { AuthService } from '@core/services/auth/auth.service';

@Component({
    selector: 'app-reset-password',
    templateUrl: './reset-password.component.html',
    styleUrls: ['./reset-password.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ResetPasswordComponent implements OnInit {
    public resetPasswordForm: FormGroup;
    public isInProgress: boolean = false;
    constructor(
        private formBuilder: FormBuilder,
        private authService: AuthService,
        private router: Router,
        private notify: NotificationService,
    ) {}

    ngOnInit(): void {
        localStorage.clear();
        this.ngInitialControlForm();
    }

    ngInitialControlForm(): void {
        this.resetPasswordForm = this.formBuilder.group({
            email: [
                null,
                Validators.compose([Validators.required, Validators.email]),
            ],
        });
    }

    reset() {
        this.isInProgress = true;
        this.authService
            .resetPassword(this.resetPasswordForm.controls['email'].value)
            .subscribe(
                (next) => {
                    this.notify.showTranslateMessage('ResetPasswordSent');
                    this.router.navigateByUrl('auth/login');
                },
                (error) => {
                    this.isInProgress = false;
                    this.notify.showTranslateMessage('ResetPasswordFailed');
                },
                () => {
                    this.resetPasswordForm.reset();
                    this.isInProgress = false;
                },
            );
    }
}
