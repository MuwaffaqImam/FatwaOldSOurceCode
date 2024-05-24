import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IncommingQuestionsComponent } from './incomming-questions/incomming-questions.component';
import { AssignMuftiComponent } from './assign-mufti/assign-mufti.component';

export const routes: Routes = [
    {
        path: '',
        component: IncommingQuestionsComponent,
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class IncommingQuestionsRoutingModule {}

export const components = [IncommingQuestionsComponent, AssignMuftiComponent];
