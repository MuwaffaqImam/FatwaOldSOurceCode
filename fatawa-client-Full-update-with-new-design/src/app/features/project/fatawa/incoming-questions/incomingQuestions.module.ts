import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '@app/infrastructure/shared/shared.module';
import { IncommingQuestionsRoutingModule } from './incomingQuestions-routing.module';

@NgModule({
    declarations: [],
    imports: [CommonModule, IncommingQuestionsRoutingModule, SharedModule],
})
export class QuestionsModule {}
