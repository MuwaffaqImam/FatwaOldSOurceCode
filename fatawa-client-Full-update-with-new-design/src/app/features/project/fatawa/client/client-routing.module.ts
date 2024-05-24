import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LandingPresentationGuestComponent } from '@app/features/landing';
import { FatawaDescriptionResolveService } from '@app/infrastructure/core/services/fatawa/fatawa-description-resolve.service';
import { FatawaCardsListComponent } from './fatawa-cards-list/fatawa-cards-list.component';
import { FatawaDepartmentsComponent } from './fatawa-departments/fatawa-departments.component';
import { FatawaDescriptionComponent } from './fatawa-description/fatawa-description.component';
import { FatawaFullListComponent } from './fatawa-full-list/fatawa-full-list.component';

export const routes: Routes = [
    {
        path: '',
        component: LandingPresentationGuestComponent,
        children: [
            {
                path: '',
                redirectTo: 'FatawaCards',
                pathMatch: 'full',
            },
            {
                path: 'FatawaCards',
                component: FatawaCardsListComponent,
            },
            {
                path: 'FatawaDescription/:id',
                resolve: {
                    fatwaDesc: FatawaDescriptionResolveService,
                },
                component: FatawaDescriptionComponent,
            },
            {
                path: 'department/:id',
                component: FatawaDepartmentsComponent,
            },
            {
                path: 'Fatawas',
                component: FatawaFullListComponent,
            },
        ],
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ClientRoutingModule {}

export const components = [FatawaCardsListComponent];
