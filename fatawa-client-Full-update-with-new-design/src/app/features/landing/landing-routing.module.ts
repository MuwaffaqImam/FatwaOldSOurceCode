import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LandingPresentationComponent, LandingLayoutComponent } from '.';
import { AuthGuard, RoleAuthGuard } from '@core/guards';

const routes: Routes = [
    {
        path: '',
        component: LandingLayoutComponent,
        children: [
            {
                path: '',
                redirectTo: 'landing-presentation',
                pathMatch: 'full',
            },
            {
                path: 'landing-presentation',
                component: LandingPresentationComponent,
                data: { title: 'Home Page' },
            },
            {
                path: 'jobs-list',
                canActivate: [AuthGuard],
                canActivateChild: [AuthGuard],
                loadChildren: () =>
                    import('../job/job.module').then((m) => m.JobModule),
            },
            {
                path: 'case',
                canActivate: [AuthGuard],
                canActivateChild: [AuthGuard],
                loadChildren: () =>
                    import('../case/case.module').then((m) => m.CaseModule),
            },
            {
                path: 'chat',
                canActivate: [AuthGuard],
                canActivateChild: [AuthGuard],
                loadChildren: () =>
                    import('../chat/chat.module').then((m) => m.ChatModule),
            },
        ],
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class LandingRoutingModule {}

export const components = [
    LandingLayoutComponent,
    LandingPresentationComponent,
];
