import { Injectable } from '@angular/core';
import { ApiService } from '../api/api.service';
import { QuestionModel } from '@app/infrastructure/models/project/fatawa/QuestionModel';
import { Observable } from 'rxjs';
import { environment } from '@env/environment';
import { HttpParams } from '@angular/common/http';

@Injectable({
    providedIn: 'root',
})
export class QuestionService {
    constructor(private apiService: ApiService) {}

    addQuestion(questionModel: QuestionModel): Observable<boolean> {
        return this.apiService.post(
            `${environment.apiRoute}/FatawaQuestions/AddQuestion`,
            questionModel,
        );
    }

    getAllQuestions(
        pageIndex: number,
        PageSize: number,
        questionsStatusId: number,
        muftiId: number,
    ): Observable<any> {
        return this.apiService.get(
            `${environment.apiRoute}/FatawaQuestions/GetAllQuestionsAsync` +
                '?pageIndex=' +
                pageIndex +
                '&pageSize=' +
                PageSize +
                '&questionStateId=' +
                questionsStatusId +
                '&MuftiId=' +
                muftiId,
        );
    }

    getQuestionById(questionId: number): Observable<string> {
        return this.apiService.get(
            `${environment.apiRoute}/FatawaQuestions/GetQuestionById?questionId=` +
                questionId,
        );
    }

    getAllQuestionsByStatusId(
        pageIndex: number,
        PageSize: number,
        statusId: number,
        muftiId: number,
    ): Observable<any> {
        return this.apiService.get(
            `${environment.apiRoute}/FatawaQuestions/getAllQuestionsByStatusId` +
                '?pageIndex=' +
                pageIndex +
                '&pageSize=' +
                PageSize +
                '&statusId=' +
                statusId +
                '&MuftiId=' +
                muftiId,
        );
    }

    updateCurrentStatusQuestion(
        questionId: number,
        statueId: number,
    ): Observable<boolean> {
        return this.apiService.put(
            `${environment.apiRoute}/FatawaQuestions/UpdateCurrentStatusQuestion?questionId=${questionId}&statueId=${statueId}`,
            null,
        );
    }

    getUserIdAddedQuestion(questionId: number): Observable<number> {
        return this.apiService.get(
            `${environment.apiRoute}/FatawaQuestions/GetUserIdAddedQuestion?questionId=` +
                questionId,
        );
    }
}
