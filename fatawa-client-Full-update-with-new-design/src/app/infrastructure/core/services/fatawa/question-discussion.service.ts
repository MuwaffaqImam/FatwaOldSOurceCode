import { Injectable } from '@angular/core';
import { QuestionDiscussion } from '@app/infrastructure/models/project/fatawa/questionDiscussionModel';
import { environment } from '@env/environment';
import { Observable } from 'rxjs';
import { ApiService } from '../api/api.service';
import { HttpParams } from '@angular/common/http';

@Injectable({
    providedIn: 'root',
})
export class QuestionDiscussionService {
    constructor(private apiService: ApiService) {}

    public getConversation(reveiverId: number, questionId: number) {
        return this.apiService.get(
            `${environment.apiRoute}/QuestionDiscussion/GetAllConversationAsync?receiverId=${reveiverId}&questionId=${questionId}`,
        );
    }

    public sendMessage(questionDiscussion: QuestionDiscussion) {
        return this.apiService.post(
            `${environment.apiRoute}/QuestionDiscussion/AddQuestionDiscussion`,
            questionDiscussion,
        );
    }
    public OnPublishQuestion(questionId: number) {
        return this.apiService.put(
            `${environment.apiRoute}/QuestionDiscussion/PublishQuestiondiscussion/`,
            null,
            {
                params: new HttpParams().set(
                    'questionId',
                    questionId.toString(),
                ),
            },
        );
    }
    getQuestionDiscussion(messageId: number): Observable<any> {
        return this.apiService.get(
            `${environment.apiRoute}/QuestionDiscussion/QuestionDiscussion?id=` +
                messageId,
        );
    }
}
