import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { ChatService } from '@app/infrastructure/core/services/signalR/chat/chat.service';
import { TokenService } from '@app/infrastructure/core/services/token.service';
import { ChatMessageModel } from '@app/infrastructure/models/chat-message';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';

@Component({
    template: `
        <app-chat-presentation
            [messageReceived]="messageReceived$"
            [isLoadingResults]="isLoadingResults"
            [isLoaded]="isLoaded"
            [currentUserID]="this.currentUserID$ | async"
            (emitSendChat)="sendMessage($event)"
        ></app-chat-presentation>
    `,
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ChatSmartComponent implements OnInit {
    messageReceived$: Observable<ChatMessageModel>;
    currentUserID$: Observable<number>;
    public isLoaded: boolean = false;
    public isLoadingResults: boolean = false;

    constructor(
        private chatService: ChatService,
        private tokenService: TokenService,
    ) {
        this.subscribeToEvents();
    }

    public ngOnInit(): void {
        this.currentUserID$ = this.tokenService.getUserId();
    }

    public sendMessage(newMessage: ChatMessageModel): void {
        this.chatService.sendMessage(newMessage);
    }

    private subscribeToEvents(): void {
        this.messageReceived$ = this.chatService.messageReceived;
    }
}
