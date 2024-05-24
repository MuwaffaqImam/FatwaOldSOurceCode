import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '@app/infrastructure/shared/shared.module';
import { ClientRoutingModule } from './client-routing.module';
import { FatawaCardsListComponent } from './fatawa-cards-list/fatawa-cards-list.component';
import { DataTreeService } from '@app/infrastructure/models/nodeTree';
import { AddQuestionComponent } from './add-question/add-question.component';
import { FatawaDescriptionComponent } from './fatawa-description/fatawa-description.component';
import { FatawaFullListComponent } from './fatawa-full-list/fatawa-full-list.component';
import { FatawaDepartmentsComponent } from './fatawa-departments/fatawa-departments.component';

@NgModule({
    declarations: [
        FatawaCardsListComponent,
        AddQuestionComponent,
        FatawaDescriptionComponent,
        FatawaFullListComponent,
        FatawaDepartmentsComponent,
    ],
    imports: [CommonModule, ClientRoutingModule, SharedModule],
    providers: [DataTreeService],
    exports: [AddQuestionComponent],
})
export class ClientModule {}
