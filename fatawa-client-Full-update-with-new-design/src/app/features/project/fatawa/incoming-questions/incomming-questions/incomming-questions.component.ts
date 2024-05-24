import {
    ChangeDetectionStrategy,
    Component,
    OnInit,
    ViewChild,
} from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { QuestionService } from '@app/infrastructure/core/services/fatawa/question.service';
import { NotificationService } from '@app/infrastructure/core/services/notification.service';
import { DynamicColumn } from '@app/infrastructure/models/gridColumns-model';
import { QuestionModel } from '@app/infrastructure/models/project/fatawa/QuestionModel';
import {
    NotificationType,
    QuestionsStatus,
} from '@app/infrastructure/models/SystemEnum';
import { ChattingComponent } from '@app/infrastructure/shared/components/chatting/chatting.component';
import {
    ActionRowGrid,
    State,
} from '@app/infrastructure/shared/Services/CommonMemmber';
import { TranslateService } from '@ngx-translate/core';
import { of, Observable } from 'rxjs';
import { catchError, mergeMap, switchMap, map } from 'rxjs/operators';
import { AssignMuftiComponent } from '../../incoming-questions/assign-mufti/assign-mufti.component';
import { TokenService } from '@app/infrastructure/core/services/token.service';
import { IDecodedAuthToken } from '@app/infrastructure/models/auth';

@Component({
    selector: 'app-incomming-questions',
    templateUrl: './incomming-questions.component.html',
    styleUrls: ['./incomming-questions.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class IncommingQuestionsComponent implements OnInit {
    @ViewChild(MatSort, { static: true }) sort: MatSort;
    @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
    public paginationIndex = 0;
    public pageIndex = 1;
    public pageSize = 10;
    public length = 0;
    public dataSource = new MatTableDataSource<QuestionModel>([]);
    public isLoading = true;
    currentUserID: number;
    currentUserType: string;

    QuastionsStatus = Object.keys(QuestionsStatus)
        .filter((f) => !isNaN(Number(f)))
        .map((key) => QuestionsStatus[key]);
    selectedStatus = 1;
    public columns: DynamicColumn[] = [];

    constructor(
        private questionService: QuestionService,
        private notify: NotificationService,
        private dialog: MatDialog,
        private router: Router,
        private translate: TranslateService,
        private tokenService: TokenService,
    ) {}

    getConfigDialog(data, isAddGridHeader?: boolean): MatDialogConfig {
        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = true;
        dialogConfig.autoFocus = true;
        dialogConfig.position = { top: '4.5em' };
        dialogConfig.width = '50em';
        // dialogConfig.height = '36em';
        dialogConfig.data = data;
        return dialogConfig;
    }

    initialAdditionalColumns(): DynamicColumn[] {
        return (this.columns = [
            {
                headerName: 'Status',
                icon: 'more_vert',
                childColumn: [
                    {
                        name: 'Available',
                        status: State.Available,
                    },
                    {
                        name: 'InProgress',
                        status: State.InProgress,
                    },
                    {
                        name: 'Duplicated',
                        status: State.Duplicated,
                    },
                    {
                        name: 'Rejected',
                        status: State.Rejected,
                    },
                ],
            },
            {
                headerName: 'Chatting',
                icon: 'chat',
                status: State.Chatting,
            },
            {
                headerName: 'Control',
                icon: 'more_vert',
                childColumn: [
                    {
                        name: 'ToFatawa',
                        status: State.ToFatawa,
                    },
                    {
                        name: 'ToMufti',
                        status: State.ToMufti,
                    },
                ],
            },
        ]);
    }
    // public getUserInfo(): Observable<IDecodedAuthToken> {
    //     return this.tokenService.getAuthToken();
    // }
    ngOnInit(): void {
        this.GetCurrentUserId();
        this.GetCurrentUserType();
        this.LoadQuestions(
            this.pageIndex,
            this.pageSize,
            QuestionsStatus.Available,
        );
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

    LoadQuestions(
        pageIndex: number,
        pageSize: number,
        questionStateId: number,
    ) {
        this.questionService
            .getAllQuestions(
                pageIndex,
                pageSize,
                questionStateId,
                this.currentUserID,
            )
            .subscribe(
                (paginationRecord) => {
                    this.dataSource.data = paginationRecord.dataRecord;
                    this.length = paginationRecord.countRecord;
                    this.isLoading = false;
                },
                (error) => {
                    this.notify.showTranslateMessage('ErrorOnLoadData');
                    this.isLoading = false;
                },
            );
    }

    changeQuastionStatusId(statusId) {
        try {
            this.questionService
                .getAllQuestionsByStatusId(
                    this.pageIndex,
                    this.pageSize,
                    statusId,
                    this.currentUserID,
                )
                .subscribe(
                    (paginationRecord) => {
                        this.dataSource.data = paginationRecord.dataRecord;
                        this.length = paginationRecord.countRecord;
                    },
                    (error) => {
                        this.notify.showTranslateMessage('ErrorOnLoadData');
                    },
                );
        } catch (error) {
            this.notify.showTranslateMessage('ErrorWhenChangeQuastions');
        }
    }

    onActionRowGrid(actionGrid: ActionRowGrid) {
        switch (actionGrid.type) {
            case State.Available:
                this.updateStatusValue(
                    actionGrid.row,
                    QuestionsStatus.Available,
                );
                break;
            case State.InProgress:
                this.updateStatusValue(
                    actionGrid.row,
                    QuestionsStatus.InProgress,
                );
                break;
            case State.Duplicated:
                this.updateStatusValue(
                    actionGrid.row,
                    QuestionsStatus.Duplicated,
                );
                break;
            case State.Rejected:
                this.updateStatusValue(
                    actionGrid.row,
                    QuestionsStatus.Rejected,
                );
                break;
            case State.ToFatawa:
                this.toFatawaQuestion(actionGrid.row);
                break;
            case State.ToMufti:
                this.MoveQuestionToMofit(actionGrid.row);
                break;
            case State.Chatting:
                this.updateNotificationThenGoChatting(actionGrid.row);
                break;
            case State.Pagination:
                this.LoadQuestions(
                    actionGrid.row.pageIndex,
                    actionGrid.row.pageSize,
                    this.selectedStatus,
                );
                break;
        }
    }

    updateStatusValue(row, state: QuestionsStatus) {
        const AllowedUserType = ['SuperAdmin', 'Admin'];
        if (AllowedUserType.includes(this.currentUserType)) {
            this.questionService
                .updateCurrentStatusQuestion(row.id, state)
                .pipe(
                    switchMap((data) => {
                        if (data) {
                            this.changeQuastionStatusId(this.selectedStatus);
                            return of(data);
                        } else {
                            this.notify.showTranslateMessage(
                                'ErrorOnChangeStatus',
                            );
                            return of(null);
                        }
                    }),
                    catchError(() => of(null)),
                )
                .subscribe((data) => {
                    if (!data) {
                        this.notify.showTranslateMessage('ErrorOnChangeStatus');
                    } else {
                        void Promise.all([
                            this.translate
                                .get('ChangeCurrentStatusTo')
                                .toPromise(),
                            this.translate
                                .get(QuestionsStatus[state])
                                .toPromise(),
                            this.translate.get('Successfully').toPromise(),
                        ]).then((messages: string[]) =>
                            this.notify.showSuccess([
                                `${messages[0]} ( ${messages[1]} ) ${messages[2]}`,
                            ]),
                        );
                    }
                });
        } else {
            this.notify.showTranslateMessage('ErrorStatus');
        }
    }

    toFatawaQuestion(row) {
        if (
            !this.acceptedStatusValueForCreatingFatwa(
                row,
                'ErrorCreateFatwaQuestionClosedOrRejectedOrDuplicated',
            )
        ) {
            this.router.navigate(['fatawa/questionToFatwa/' + row.id]);
        }
    }
    isStudentAdminUser(): Observable<boolean> {
        return this.tokenService.isStudentAdmin();
    }
    MoveQuestionToMofit(row) {
        const AllowedUserType = ['SuperAdmin', 'Admin'];

        if (AllowedUserType.includes(this.currentUserType)) {
            if (
                !this.acceptedStatusValueForCreatingFatwa(
                    row,
                    'ErrorCreateFatwaQuestionClosedOrRejectedOrDuplicated',
                )
            ) {
                this.dialog
                    .open(AssignMuftiComponent, this.getConfigDialog(row))
                    .afterClosed()
                    .subscribe(
                        (result) => {
                            if (result === undefined) {
                                this.notify.showTranslateMessage(
                                    'TransferSuccessfully',
                                    false,
                                );
                                this.LoadQuestions(
                                    this.pageIndex,
                                    this.pageSize,
                                    QuestionsStatus.Available,
                                );
                            } else {
                                this.notify.showTranslateMessage(
                                    'NoTransferSuccessfully',
                                );
                                this.LoadQuestions(
                                    this.pageIndex,
                                    this.pageSize,
                                    QuestionsStatus.Available,
                                );
                            }
                        },
                        (error) => {
                            this.notify.showTranslateMessage('ErrorOnAdd');
                        },
                    );
            }
        } else {
            this.notify.showTranslateMessage('ErrorTransfer');
        }
    }

    updateNotificationThenGoChatting(row) {
        if (
            !this.acceptedStatusValueForOpeningChatting(
                row,
                'ErrorOpenChattQuestionClosedOrRejected',
            )
        ) {
            this.notify
                .updateReadNewNotification({
                    createdBy: row.createdBy,
                    notificationTypeId: NotificationType.Chatting,
                })
                .pipe(
                    mergeMap(() => this.notify.loadUnreadNotification()),
                    mergeMap(() => {
                        const dialog = this.dialog.open(
                            ChattingComponent,
                            this.getConfigDialog(row),
                        );
                        return dialog.afterClosed();
                    }),
                    mergeMap(() => of('')),
                    catchError((): any => {
                        this.notify.showTranslateMessage(
                            'ErrorOnOpenChattingDialog',
                        );
                    }),
                )
                .subscribe((result) => {});
        }
    }

    acceptedStatusValueForCreatingFatwa(row, message: string): boolean {
        if (
            row.statusId === QuestionsStatus.Rejected ||
            row.statusId === QuestionsStatus.Closed ||
            row.statusId === QuestionsStatus.Duplicated
        ) {
            this.notify.showTranslateMessage(`${message}`);
            return true;
        } else {
            return false;
        }
    }

    acceptedStatusValueForOpeningChatting(row, message: string): boolean {
        if (
            row.statusId === QuestionsStatus.Rejected ||
            row.statusId === QuestionsStatus.Closed
        ) {
            this.notify.showTranslateMessage(`${message}`);
            return true;
        } else {
            return false;
        }
    }
}
