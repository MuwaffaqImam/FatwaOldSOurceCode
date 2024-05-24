import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { AuthService } from '@app/infrastructure/core/services/auth/auth.service';
import { UserService } from '@app/infrastructure/core/services/auth/user.service';
import { QuestionDiscussionService } from '@app/infrastructure/core/services/fatawa/question-discussion.service';
import { LanguagesService } from '@app/infrastructure/core/services/language/language.service';
import { NotificationService } from '@app/infrastructure/core/services/notification.service';
import { LanguageModel } from '@app/infrastructure/models/project/LanguageModel';
import { NotificationType } from '@app/infrastructure/models/SystemEnum';
import { ChattingComponent } from '@app/infrastructure/shared/components/chatting/chatting.component';
import { SystemNotification } from '@app/infrastructure/shared/Services/CommonMemmber';
import { TranslateService } from '@ngx-translate/core';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { BehaviorSubject, of } from 'rxjs';
import { catchError, mergeMap, tap, map } from 'rxjs/operators';
import { TokenService } from '@app/infrastructure/core/services/token.service';

@Component({
    selector: 'app-header',
    templateUrl: './header.component.html',
    styleUrls: ['header.component.scss'],
})
export class AppHeaderComponent implements OnInit {
    @Output()
    public emitUserLanguageChanged = new EventEmitter<LanguageModel>();

    public config: PerfectScrollbarConfigInterface = {};
    public languages: LanguageModel[] = [];
    public languageSelected: LanguageModel;
    public selectLangId: number;
    public fullname: string;
    constructor(
        private authService: AuthService,
        private router: Router,
        private notificationService: NotificationService,
        private dialog: MatDialog,
        private questionDiscussionService: QuestionDiscussionService,
        private languagesService: LanguagesService,
        private userService: UserService,
        public translate: TranslateService,
        private tokenService: TokenService,
    ) {}

    getConfigDialog(data: any): MatDialogConfig {
        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = true;
        dialogConfig.autoFocus = true;
        dialogConfig.position = { top: '4.5em' };
        dialogConfig.width = '50em';
        dialogConfig.height = '36em';
        dialogConfig.data = data;
        return dialogConfig;
    }

    ngOnInit(): void {
        this.GetCurrentFullname();
        this.getAllLanguagesAndNotifications();
    }
    public GetCurrentFullname(): void {
        this.tokenService
            .getUserFullName()
            .pipe(
                map((data) => {
                    this.fullname = data;
                }),
                catchError((): any => {
                    // this.notify.showTranslateMessage('ErrorLoadLanguages');
                }),
            )
            .subscribe();
    }
    getAllLanguagesAndNotifications(): void {
        this.languagesService
            .getAllLanguages()
            .pipe(
                mergeMap((languagesModel: LanguageModel[]) => {
                    this.languages = languagesModel;
                    if (!this.userService.isTokenExist()) {
                        this.selectLangId = Number(
                            this.userService.getLanguageId(),
                        );
                        this.languageSelected = languagesModel.filter(
                            (lang) => lang.id === this.selectLangId,
                        )[0];
                        this.emitUserLanguageChanged.emit(
                            this.languageSelected,
                        );
                        return of(null);
                    } else {
                        return this.userService.getLanguageInformations();
                    }
                }),
                tap((languageInfo: LanguageModel) => {
                    if (languageInfo) {
                        this.userService.setLanguageId(
                            languageInfo.id.toString(),
                        );
                        this.userService.setLanguageDir(
                            languageInfo.languageDirection,
                        );
                        this.languageSelected = this.languages.filter(
                            (lang) =>
                                lang.id === this.userService.getLanguageId(),
                        )[0];
                        this.selectLangId = Number(
                            this.userService.getLanguageId(),
                        );
                        this.emitUserLanguageChanged.emit(
                            this.languageSelected,
                        );
                    }
                }),
                mergeMap(() => {
                    if (this.userService.isTokenExist()) {
                        return this.notificationService.loadAllNotifications();
                    }
                    return of({});
                }),
                catchError((): any => {
                    this.notificationService.showTranslateMessage(
                        'ErrorLoadLanguages',
                    );
                }),
            )
            .subscribe((result) => {});
    }

    get notificationsList(): BehaviorSubject<SystemNotification[]> {
        return this.notificationService.notificationSubject;
    }

    public logout(): void {
        this.authService.loggedOut();
        this.router.navigateByUrl('/auth');
    }

    public redirectToSourcePage(notificationInfo: SystemNotification): void {
        if (
            NotificationType.Fatawa === notificationInfo.notificationTypeId ||
            NotificationType.Questions === notificationInfo.notificationTypeId
        ) {
            this.notificationService
                .updateReadNewQuestionsAndFatwa(notificationInfo)
                .pipe(
                    mergeMap((data: any): any => {
                        if (data) {
                            void this.router.navigateByUrl(
                                NotificationType.Fatawa ===
                                    notificationInfo.notificationTypeId
                                    ? 'fatawa/fatawa-live'
                                    : 'fatawa/incomming-questions',
                            );
                            return this.notificationService.loadAllNotifications();
                        }
                    }),
                )
                .subscribe((result) => {});
        } else if (
            NotificationType.Chatting === notificationInfo.notificationTypeId
        ) {
            this.notificationService
                .updateReadNewNotification(notificationInfo)
                .pipe(
                    mergeMap(() => {
                        return this.notificationService.loadAllNotifications();
                    }),
                    mergeMap(() =>
                        this.questionDiscussionService.getQuestionDiscussion(
                            notificationInfo.referenceMassageId,
                        ),
                    ),
                    mergeMap((data) => {
                        const dialog = this.dialog.open(
                            ChattingComponent,
                            this.getConfigDialog(data),
                        );
                        return dialog.afterClosed();
                    }),
                    mergeMap(() => of('')),
                    catchError((): any =>
                        this.notificationService.showTranslateMessage(
                            'ErrorOnOpenChattingDialog',
                        ),
                    ),
                )
                .subscribe((result) => {});
        }
    }

    public getLanguageImageResource(language: LanguageModel): string {
        return language
            ? `assets/images/${language.languageCode}.png`
            : 'assets/images/AR.png';
    }

    updateSelectedLanguage(languageId: number): void {
        const languageModel: LanguageModel = this.languages.filter(
            (l) => l.id === languageId,
        )[0];
        this.languageSelected = languageModel; //add this to change language drop after selected
        this.userService.setLanguageId(languageModel.id.toString());
        this.userService.setLanguageDir(languageModel.languageDirection);
        const languageCode: string = languageModel.languageCode;
        this.translate.use(languageCode);
        this.emitUserLanguageChanged.emit(languageModel);

        if (this.userService.isTokenExist()) {
            this.userService
                .updateUserLanguage(languageId)
                .subscribe((result) => {});
            catchError((): any => {
                this.notificationService.showError([
                    'Error when change language',
                ]);
            });
        }
    }
}
