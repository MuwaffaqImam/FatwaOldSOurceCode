import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddJobComponent } from './addJob/add-job.component';
import { JobsListComponent } from './jobsList/jobs-list.component';

const routes: Routes = [
    {
        path: '',
        component: JobsListComponent,
        children: [],
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class JobRoutingModule {}

export const components = [JobsListComponent, AddJobComponent];
