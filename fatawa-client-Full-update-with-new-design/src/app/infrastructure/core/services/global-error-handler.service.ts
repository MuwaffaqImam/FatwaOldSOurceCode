import { HttpErrorResponse } from '@angular/common/http';
import { ErrorHandler, Injectable, Injector } from '@angular/core';
import { environment } from '@env/environment';
import { ISentryConfig, SentryConfig } from '@models/sentry';
import { Logger } from '@utils/logger';
import { ErrorService } from './error.service';
import { NotificationService } from './notification.service';
import { SentryErrorHandlerService } from './sentry-error-handler.service';

@Injectable()
export class GlobalErrorHandlerService implements ErrorHandler {
    constructor(public injector: Injector) {}

    public handleError(
        error: Error | HttpErrorResponse,
        message: string = '',
        sentryConfig: ISentryConfig = new SentryConfig(),
    ): void {
        const errorService = this.injector.get(ErrorService);
        const notifier = this.injector.get(NotificationService);
        const sentryErrorHandler = this.injector.get(SentryErrorHandlerService);

        let errorMessage: string[] | string;
        if (error instanceof HttpErrorResponse) {
            // Server error
            switch (error.status) {
                case 500:
                    message = 'Server Error';
                    sentryConfig.sendToSentry = true;
                    sentryConfig.showDialog = false;
                    break;
                case 0:
                    if (navigator.onLine) {
                        message = 'Connection to servers is not available.';
                        sentryConfig.sendToSentry = true;
                        sentryConfig.showDialog = false;
                    } else {
                        message = 'No Internet Connection.';
                    }
                    break;
                case 401:
                    message = 'Invalid Email Or Password.';
                    sentryConfig.sendToSentry = false;
                    break;
                case 403:
                    message =
                        'You do not have permission to view the selected page.';
                    sentryConfig.sendToSentry = true;
                    break;
                case 404:
                    message = 'Not found.';
                    break;
            }
            errorMessage = message || errorService.getServerMessage(error);
            if (typeof errorMessage === 'string') {
                errorMessage = this.convertStringMessageToList(errorMessage);
            }
            notifier.showError(errorMessage);
            if (sentryConfig.sendToSentry) {
                this.logError(
                    sentryErrorHandler,
                    error,
                    sentryConfig,
                    errorMessage,
                );
            }
        } else {
            // Client Error
            sentryConfig.sendToSentry = true;
            sentryConfig.showDialog = false;
            errorMessage = errorService.getClientMessage(error);
            if (typeof errorMessage === 'string') {
                errorMessage = this.convertStringMessageToList(errorMessage);
            }
            notifier.showError(errorMessage);
            this.logError(
                sentryErrorHandler,
                error,
                sentryConfig,
                errorMessage,
            );
        }
    }

    private logError(
        sentryErrorHandler: SentryErrorHandlerService,
        error: Error | HttpErrorResponse,
        sentryConfig: ISentryConfig = new SentryConfig(),
        errorMessageList: string[] = [],
    ): void {
        if (!environment.production) {
            Logger.error(error);
        }
        if (environment.sentry.enabled) {
            sentryErrorHandler.handleError(
                error,
                sentryConfig.showDialog,
                errorMessageList,
            );
        }
    }

    private convertStringMessageToList(message: string): string[] {
        const messageList: string[] = [];
        messageList.push(message);
        return messageList;
    }
}
