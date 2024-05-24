import { Injectable, NgZone } from '@angular/core';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material/snack-bar';
import { Message } from '@app/infrastructure/models/message';
import { SystemNotification } from '@app/infrastructure/shared/Services/CommonMemmber';
import { Constants } from '@app/infrastructure/utils/constants';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { environment } from '@env/environment';
import { TranslateService } from '@ngx-translate/core';
import { MessageSnackbarComponent } from '@shared/components/message-snackbar/message-snackbar.component';
import { BehaviorSubject, combineLatest, Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { ApiService } from './api/api.service';

@Injectable({
    providedIn: 'root',
})
export class NotificationService {
    public hubConnection: HubConnection;
    dataChange = new BehaviorSubject<Message>(null);
    notificationSubject = new BehaviorSubject<SystemNotification[]>([]);

    constructor(
        private snackBar: MatSnackBar,
        private zone: NgZone,
        private apiService: ApiService,
        private translate: TranslateService,
    ) {
        this.initialConnectionsForUser();
    }
    public initialConnectionsForUser(): void {
        const authToken = sessionStorage.getItem('authToken');
        if (authToken) {
            this.createConnection(authToken);
            this.registerOnServerEvents();
            this.startConnection();
        }
    }

    showTranslateMessage(
        messageKey: string,
        isErrorMessage: boolean = true,
    ): void {
        void this.translate
            .get(messageKey)
            .toPromise()
            .then((message: string) =>
                isErrorMessage
                    ? this.showError([message])
                    : this.showSuccess([message]),
            );
    }

    public showSuccess(message: string[]): void {
        const configSuccess: MatSnackBarConfig<MessageSnackbarComponent> = {
            panelClass: 'success',
            verticalPosition: 'top',
            duration: Constants.NOTIFICATION_MESSAGE_TIMEOUT,
        };
        this.openSnackbar(message, configSuccess);
    }

    public showError(message: string[]): void {
        const configError: MatSnackBarConfig<MessageSnackbarComponent> = {
            panelClass: 'error',
            verticalPosition: 'top',
            duration: Constants.NOTIFICATION_MESSAGE_TIMEOUT,
        };
        this.openSnackbar(message, configError);
    }

    public openSnackbar(
        message: string[],
        config: MatSnackBarConfig<MessageSnackbarComponent>,
    ): void {
        // Wrap snackbar call in zone invocation to fix rendering inconsistencies
        this.zone.run(() => {
            this.snackBar.openFromComponent(MessageSnackbarComponent, {
                data: message,
                ...config,
            });
        });
    }

    public getConversation(recipientId: number) {
        return this.apiService.get(
            `${environment.apiRoute}/Chat/GetAllChattingMassage?recipientId=` +
                recipientId,
        );
    }

    public sendMessage(messageModel: Message) {
        return this.apiService.post(
            `${environment.apiRoute}/Chat/AddNewMessage`,
            messageModel,
        );
    }

    public createConnection(authToken: string): void {
        this.hubConnection = new HubConnectionBuilder()
            .withUrl(environment.hubRoute + '/chathub', {
                accessTokenFactory: () => authToken,
            })
            .build();
    }

    public startConnection(): void {
        this.hubConnection
            .start()
            .then(() => {
                console.log('Hub connection started');
            })
            .catch((err) => {
                console.log(
                    'Error while establishing connection, retrying...' +
                        JSON.stringify(err),
                );
                setTimeout(() => {
                    this.startConnection();
                }, 5000);
            });
    }

    public registerOnServerEvents(): void {
        this.hubConnection.on('SendNewMessageRefresh', (data: Message) => {
            this.dataChange.next(data);
        });

        this.hubConnection.on('UnreadChattingMessages', () => {
            this.loadUnreadNotification().subscribe();
        });

        this.hubConnection.on('AddedNewQuestionRefresh', () => {
            this.loadNewQuestionsAndFatwa().subscribe();
        });

        this.hubConnection.on('AddedNewFatwaRefresh', () => {
            this.loadNewQuestionsAndFatwa().subscribe();
        });

        this.hubConnection.on('PublishedNewFatwaRefresh', () => {
            this.loadUnreadNotification().subscribe();
        });
    }

    public loadUnreadNotification(): Observable<SystemNotification[]> {
        return this.apiService
            .get(`${environment.apiRoute}/Notification/GetUnreadNotification`)
            .pipe(
                tap((data: SystemNotification[]) => {
                    this.notificationSubject.next(data.reverse());
                }),
            );
    }

    public loadNewQuestionsAndFatwa(): Observable<SystemNotification[]> {
        return this.apiService
            .get(`${environment.apiRoute}/Notification/GetNewQuestionsAndFatwa`)
            .pipe(
                tap((data: SystemNotification[]) => {
                    this.notificationSubject.next(data.reverse());
                }),
            );
    }

    public loadAllNotifications(): Observable<
        [SystemNotification[], SystemNotification[]]
    > {
        return combineLatest([
            this.loadUnreadNotification(),
            this.loadNewQuestionsAndFatwa(),
        ]).pipe(
            tap(
                ([chattingData, questionsAndFatawaData]: [
                    SystemNotification[],
                    SystemNotification[],
                ]) => {
                    this.notificationSubject.next(
                        [...chattingData, ...questionsAndFatawaData].reverse(),
                    );
                },
            ),
        );
    }

    public invokeNewMessage(data: Message, receivedId: number): Promise<any> {
        return Promise.all([
            //this (SendNewMessageRefresh) invoke from sever,same function name in ChatHub class.
            this.hubConnection.invoke('SendNewMessageRefresh', data),
            this.hubConnection.invoke('UnreadChattingMessages', receivedId),
        ]);
    }

    public invokeAddedNewQuestion(): any {
        return this.hubConnection.invoke('AddedNewQuestionRefresh');
    }

    public invokeAddedNewFatwa(): any {
        return this.hubConnection.invoke('AddedNewFatwaRefresh');
    }

    public invokePublishedNewFatwa(userId: number): any {
        return this.hubConnection.invoke('PublishedNewFatwaRefresh', userId);
    }

    updateReadNewNotification(
        notificationInfo: Partial<SystemNotification>,
    ): Observable<any> {
        return this.apiService.post(
            `${environment.apiRoute}/Notification/UpdateReadNewNotification`,
            notificationInfo,
        );
    }

    updateReadNewQuestionsAndFatwa(notificationInfo: any): Observable<any> {
        return this.apiService.post(
            `${environment.apiRoute}/Notification/UpdateReadNewQuestionsAndFatwa`,
            notificationInfo,
        );
    }
}
