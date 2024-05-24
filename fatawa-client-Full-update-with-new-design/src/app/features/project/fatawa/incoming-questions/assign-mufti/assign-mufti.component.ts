import {
    Component,
    OnInit,
    ChangeDetectionStrategy,
    Inject,
    Input,
} from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { UserService } from '@app/infrastructure/core/services/auth/user.service';
import { LanguagesService } from '@app/infrastructure/core/services/language/language.service';
import { catchError, map } from 'rxjs/operators';
import { NotificationService } from '@app/infrastructure/core/services/notification.service';
import { UserNodeModel } from '@app/infrastructure/models/project/fatawa/MuftiTreeModal';
import { Observable } from 'rxjs';
import { mergeMap } from 'rxjs/operators';
import { TokenService } from '@app/infrastructure/core/services/token.service';
import { QuestionModel } from '@app/infrastructure/models/project/fatawa/QuestionModel';
import { UserType } from '@app/infrastructure/models/user';

@Component({
    selector: 'app-assign-mufti',
    templateUrl: './assign-mufti.component.html',
    styleUrls: ['./assign-mufti.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AssignMuftiComponent implements OnInit {
    public isInProgress = false;
    currentUserID: number;
    currentUserType: UserType;

    mufitUserId: number;
    curruser: number;
    muftiList = [];
    questionId: number = 0;
    constructor(
        @Inject(MAT_DIALOG_DATA)
        public userTreeModel: UserNodeModel,
        private formBuilder: FormBuilder,
        private userService: UserService,
        private languagesService: LanguagesService,
        private notify: NotificationService,
        private tokenService: TokenService,
        private dialogRef: MatDialogRef<AssignMuftiComponent>,
        @Inject(MAT_DIALOG_DATA) public questionModel: any,
    ) {
        this.questionId = questionModel.id;
    }
    ngOnInit(): void {
        this.GetCurrentUserId();
        this.GetCurrentUserType();
        this.getFatawaMuftiListByUserId(this.currentUserID);
    }
    GetCurrentUserId(): void {
        this.tokenService
            .getUserId()
            .pipe(
                map((data) => {
                    this.currentUserID = data;
                }),
                catchError((): any => {
                    // this.notify.showTranslateMessage('ErrorLoadLanguages');
                }),
            )
            .subscribe();
    }
    GetCurrentUserType(): void {
        this.tokenService
            .getAuthToken()
            .pipe(
                map((user) => {
                    if (user.role) {
                        this.currentUserType = user.role;
                    }
                }),
                catchError((): any => {
                    // this.notify.showTranslateMessage('ErrorLoadLanguages');
                }),
            )
            .subscribe();
    }
    getFatawaMuftiListByUserId(userId: number) {
        const AllowedUserType = ['SuperAdmin', 'Admin'];
        if (AllowedUserType.includes(this.currentUserType)) {
            return this.userService
                .getAllSuperAdminsAsync()
                .pipe(
                    map((data) => {
                        this.muftiList = data;
                    }),
                    catchError((): any => {
                        this.notify.showTranslateMessage('ErrorLoadLanguages');
                    }),
                )
                .subscribe((result) => {});
        } else {
            return this.userService
                .getMuftiListByUserId(userId)
                .pipe(
                    map((data) => {
                        this.muftiList = data;
                    }),
                    catchError((): any => {
                        this.notify.showTranslateMessage('ErrorLoadLanguages');
                    }),
                )
                .subscribe((result) => {});
        }
    }
    OnSubmit(mufitUserId: number) {
        this.isInProgress = true;

        this.userService
            .TransferQuestionToMufti(
                this.currentUserID,
                mufitUserId,
                this.questionId,
            )
            .subscribe(
                (result) => {
                    if (result) {
                        this.dialogRef.close();
                    }
                },
                () => {
                    this.dialogRef.close();
                },
            );

        // this.resetFormBuilder();
    }

    // resetFormBuilder() {
    //     this.frmAddNew.reset();
    //     this.isInProgress = false;
    // }
}
