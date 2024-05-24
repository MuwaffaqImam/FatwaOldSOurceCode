import { Injectable } from '@angular/core';
import {
    ActivatedRouteSnapshot,
    Resolve,
    RouterStateSnapshot,
} from '@angular/router';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { QuestionService } from './question.service';

@Injectable({
    providedIn: 'root',
})
export class QuestionToFatwaResolveService implements Resolve<string> {
    constructor(private questionService: QuestionService) {}
    resolve(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot,
    ): Observable<string> {
        return this.questionService
            .getQuestionById(route.params.id)
            .pipe(take(1));
    }
}
