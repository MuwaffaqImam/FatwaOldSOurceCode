import {
    ChangeDetectionStrategy,
    ChangeDetectorRef,
    Component,
    EventEmitter,
    Input,
    OnDestroy,
    OnInit,
    Output,
} from '@angular/core';
import { ChatMessageModel, IChatMessageModel } from '@models/chat-message';
import { Observable, Subscription } from 'rxjs';
import { map } from 'rxjs/operators';

@Component({
    selector: 'app-chat-presentation',
    templateUrl: './chat-presentation.component.html',
    styleUrls: ['./chat-presentation.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ChatPresentationComponent implements OnInit, OnDestroy {
    @Input() public messageReceived: Observable<ChatMessageModel>;
    @Input() public isLoaded: boolean = false;
    @Input() public isLoadingResults: boolean = false;
    @Input() public currentUserID: number;
    @Output() public emitSendChat = new EventEmitter<IChatMessageModel>();

    txtMessage: string = '';
    messages = new Array<ChatMessageModel>();
    subscription: Subscription;

    constructor(private changeDetectorRef: ChangeDetectorRef) {}

    public ngOnInit(): void {
        this.initialReceiveMessage();
    }

    ngOnDestroy(): void {
        this.subscription.unsubscribe();
    }

    initialReceiveMessage(): void {
        this.subscription = this.messageReceived
            .pipe(
                map((receivedMessage: ChatMessageModel) => {
                    receivedMessage.type =
                        receivedMessage.toUserId === this.currentUserID
                            ? 'received'
                            : 'sent';
                    return receivedMessage;
                }),
            )
            .subscribe((receivedMessage: ChatMessageModel) => {
                this.messages.push(receivedMessage);

                this.changeDetectorRef.detectChanges();
            });
    }

    sendMessage(): void {
        const newMessage = new ChatMessageModel();
        newMessage.createdBy = this.currentUserID;
        newMessage.createdDate = newMessage.updatedDate = new Date();
        newMessage.toUserId = this.currentUserID === 1 ? 2 : 1; // Update this line with real project...
        newMessage.message = this.txtMessage;
        this.emitSendChat.emit(newMessage);
    }
}
