import {
    Component,
    OnInit,
    ChangeDetectionStrategy,
    Input,
    ChangeDetectorRef,
    ElementRef,
    AfterViewChecked,
    ViewChild,
    Inject,
} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { QuestionDiscussionService } from '@app/infrastructure/core/services/fatawa/question-discussion.service';
import { NotificationService } from '@app/infrastructure/core/services/notification.service';
import { Message } from '@app/infrastructure/models/message';
import { of } from 'rxjs';
import { catchError, mergeMap, map } from 'rxjs/operators';
import { TokenService } from '@app/infrastructure/core/services/token.service';

@Component({
    selector: 'app-chatting',
    templateUrl: './chatting.component.html',
    styleUrls: ['./chatting.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ChattingComponent implements OnInit, AfterViewChecked {
    @Input() receiverId: number;
    messages: Message[] = [];
    frmAddNew: FormGroup;
    @ViewChild('chatBox') chatBox: ElementRef<any>;
    questionId: number;
    currentUserType: string;

    constructor(
        private questionDiscussionService: QuestionDiscussionService,
        private notificationService: NotificationService,
        private cdr: ChangeDetectorRef,
        private formBuilder: FormBuilder,
        private tokenService: TokenService,
        @Inject(MAT_DIALOG_DATA) public questionInfo: any,
    ) {
        this.receiverId = Number(this.questionInfo.createdBy);
        this.questionId = !Number(this.questionInfo.questionId)
            ? Number(this.questionInfo.id)
            : Number(this.questionInfo.questionId);

        this.notificationService.dataChange
            .pipe(
                mergeMap((data) => {
                    if (data) {
                        this.messages.push(data);
                        this.cdr.markForCheck();
                    }
                    return of({});
                }),
            )
            .subscribe((data) => {});
    }

    ngAfterViewChecked(): void {
        this.chatBox.nativeElement.scrollTop = this.chatBox.nativeElement.scrollHeight;
    }

    ngOnInit(): void {
        this.GetCurrentUserType();
        this.ngInitialControlForm();
        this.loadMessages();
    }

    ngInitialControlForm(): void {
        this.frmAddNew = this.formBuilder.group({
            MessageText: ['', Validators.required],
            RepliedId: [this.receiverId],
            QuestionId: [this.questionId],
        });
    }
    private GetCurrentUserType(): void {
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
    private loadMessages(): void {
        this.questionDiscussionService
            .getConversation(this.receiverId, this.questionId)
            .subscribe(
                (data: Array<Message>) => {
                    this.messages = data.reverse();
                    this.cdr.detectChanges();
                },
                (error) => {
                    this.notificationService.showTranslateMessage(
                        'ErrorLoadingConversations',
                    );
                },
            );
    }

    OnSubmit() {
        this.questionDiscussionService
            .sendMessage(this.frmAddNew.value)
            .subscribe(
                (data: Message) => {
                    this.messages.push(data);
                    this.notificationService.invokeNewMessage(
                        data,
                        this.receiverId,
                    );
                    this.resetFormBuilder();
                },
                (error) => {
                    this.notificationService.showTranslateMessage(
                        'ErrorSendMessage',
                    );
                },
            );
    }
    public OnPublish() {
        this.questionDiscussionService
            .OnPublishQuestion(this.questionId)
            .subscribe((data) => {
                this.notificationService.showTranslateMessage(
                    'PublishQuestionDiscussion',
                );
            });
    }
    resetFormBuilder() {
        this.frmAddNew.reset();
        this.frmAddNew.controls.RepliedId.setValue(this.receiverId);
        this.frmAddNew.controls.QuestionId.setValue(this.questionId);
    }
}
