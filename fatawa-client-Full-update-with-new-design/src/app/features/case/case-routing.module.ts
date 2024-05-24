import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { CaseLayoutComponent } from './case-layout/case-layout.component';
import { CaseListPresentationComponent } from './case-list/case-list-presentation.component';
import { CaseListSmartComponent } from './case-list/case-list-smart.component';
import { CaseTableComponent } from './case-list/case-table/case-table.component';

const routes: Routes = [
    {
        path: '',
        component: CaseLayoutComponent,
        children: [
            {
                path: '',
                redirectTo: 'case-list',
                pathMatch: 'full',
            },
            {
                path: 'case-list',
                component: CaseListSmartComponent,
                data: { title: 'Case List' },
            },
        ],
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CaseRoutingModule {}

export const components = [
    CaseLayoutComponent,
    CaseListPresentationComponent,
    CaseListSmartComponent,
    CaseTableComponent,
];
