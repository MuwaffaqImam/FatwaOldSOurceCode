import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Constants } from '@app/infrastructure/utils/constants';
import { AuthService } from '@core/services/auth/auth.service';
import { NotificationService } from '@core/services/notification.service';
import { of } from 'rxjs';
import { catchError, mergeMap } from 'rxjs/operators';

const passwordPattern: string = '(?=.*d)(?=.*[a-z]).{8}';

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RegisterComponent implements OnInit {
    registerForm: FormGroup;
    public isInProgress = false;
    public isHidePassword: boolean = true;
    public isHidePasswordConfirm: boolean = true;
    public passwordPattern: RegExp = Constants.patterns.PASSWORD_REGEX;

    constructor(
        private formBuilder: FormBuilder,
        private authService: AuthService,
        private router: Router,
        private notify: NotificationService,
    ) {}

    get Email() {
        return this.registerForm.controls.Email.value;
    }

    ngOnInit(): void {
        this.ngInitialControlForm();
    }
    ngInitialControlForm() {
        this.registerForm = this.formBuilder.group({
            Fullname: [null, Validators.required],
            Email: [
                null,
                Validators.compose([Validators.required, Validators.email]),
            ],
            PhoneNumber: [null, Validators.required],
            PasswordHash: [null, Validators.required],
            ConfirmPassword: [null, Validators.required],
            LanguageId: [1, Validators.required],
        });
    }

    register(): void {
        this.isInProgress = true;
        this.authService
            .isUserExists(this.Email)
            .pipe(
                mergeMap((data) => {
                    if (!data) {
                        return this.authService.register(
                            this.registerForm.value,
                            0,
                        );
                    } else {
                        return of(null);
                    }
                }),
                catchError((error) => {
                    this.notify.showTranslateMessage('NotAddedSuccessfully');
                    return of(null);
                }),
            )
            .subscribe((result) => {
                this.isInProgress = false;
                this.registerForm.reset();

                if (result) {
                    this.notify.showTranslateMessage(
                        'AddedSuccessfullyCheckEmail',
                        false,
                    );
                } else {
                    this.notify.showTranslateMessage('EmailAlreadyExist');
                }
            });
    }

    ResetControls(): void {
        this.registerForm.reset();
    }

    loginRedirect() {
        this.ResetControls();
        this.router.navigateByUrl('/auth/login');
    }
}
