import { ViewportScroller } from '@angular/common';
import {
    ChangeDetectionStrategy,
    Component,
    HostListener,
    OnInit,
} from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
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
import { Constants } from '@app/infrastructure/utils/constants';
import { TranslateService } from '@ngx-translate/core';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
@Component({
    selector: 'app-shared-header',
    templateUrl: './shared-header.component.html',
    styleUrls: ['./shared-header.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SharedHeaderComponent implements OnInit {
    public languages: LanguageModel[] = [];
    public languageSelected: LanguageModel;
    public selectLangId: number;
    public dir: string = Constants.DefaultLanguageDirection;
    langForm = new FormGroup({
        lang: new FormControl(''),
    });
    scrollPosition: number = 600;
    scrollYoffSet: number = 0;
    @HostListener('window:scroll', ['$event']) onScroll(event) {
        this.scrollYoffSet = window.pageYOffset;
    }
    public config: PerfectScrollbarConfigInterface = {};
    constructor(
        public userService: UserService,
        public translate: TranslateService,
        private notificationService: NotificationService,
        private languagesService: LanguagesService,
        private authService: AuthService,
        private router: Router,
        private scroll: ViewportScroller,
        private dialog: MatDialog,
        private questionDiscussionService: QuestionDiscussionService,
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
        this.getAllLanguagesAndNotifications();
    }

    get notificationsList(): any {
        return this.notificationService.notificationSubject;
    }
    getAllLanguagesAndNotifications() {
        this.languagesService
            .getAllLanguages()
            .pipe(
                mergeMap((data) => {
                    this.languages = data;
                    if (this.userService.isTokenExist()) {
                        return this.notificationService.loadAllNotifications();
                    }
                    return of({});
                }),
                mergeMap(() => {
                    if (this.userService.isTokenExist()) {
                        return this.userService.getLanguageInformations();
                    } else {
                        this.setPageLanguage(this.userService.getLanguageId());
                    }
                    return of('');
                }),
                mergeMap((languageInfo: LanguageModel) => {
                    if (languageInfo) {
                        this.setPageLanguage(languageInfo.id);
                    }
                    return of({});
                }),
                catchError((): any => {
                    this.setPageLanguage(this.userService.getLanguageId());
                    this.notificationService.showError([
                        'Error when loading user language',
                    ]);
                }),
            )
            .subscribe((result) => {});
    }
    public getLanguageImageResource(language: LanguageModel): string {
        return language
            ? `assets/images/${language.languageCode}.png`
            : 'assets/images/AR.png';
    }

    setPageLanguage(langId) {
        this.languageSelected = this.languages.find(
            (lang) => lang.id == langId,
        );
        this.dir = this.languageSelected.languageDirection;
        this.langForm.controls.lang.patchValue(this.languageSelected.id);
        this.translate.use(this.languageSelected.languageCode);
    }
    public logout(): void {
        this.authService.loggedOut();
        this.router.navigateByUrl('/auth');
    }
    public redirectToSourcePage(notificationInfo: SystemNotification): void {
        if (NotificationType.Chatting === notificationInfo.notificationTypeId) {
            this.notificationService
                .updateReadNewNotification(notificationInfo)
                .pipe(
                    mergeMap(() => {
                        if (this.userService.isTokenExist()) {
                            return this.notificationService.loadAllNotifications();
                        }
                        return of({});
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
                    catchError((): any =>
                        this.notificationService.showTranslateMessage(
                            'ErrorOnOpenChattingDialog',
                        ),
                    ),
                )
                .subscribe((result) => {});
        } else if (
            NotificationType.Fatawa === notificationInfo.notificationTypeId
        ) {
            this.notificationService
                .updateReadNewNotification(notificationInfo)
                .pipe(
                    map((data: any): any => {
                        if (data) {
                            this.notificationService.loadAllNotifications();
                            this.router.navigateByUrl(
                                `/client/FatawaDescription/${notificationInfo.referenceMassageId}`,
                            );
                        }
                    }),
                )
                .subscribe((result) => {});
        }
    }
}
