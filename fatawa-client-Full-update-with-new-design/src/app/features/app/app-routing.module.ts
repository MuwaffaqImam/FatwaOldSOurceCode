import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {
    AuthGuard,
    SuperAdminAuthGuard,
    UsersAdminAuthGuard,
    StudentAdminAuthGuard,
} from '@app/infrastructure/core/guards';
import { RedirectRouteGuard } from '@app/infrastructure/core/guards/redirect-route.guard';
import { NotFoundComponent } from './not-found/not-found.component';
import { RedirectRouteComponent } from './redirectRoute/redirectRoute.component';

const routes: Routes = [
    {
        path: '',
        component: RedirectRouteComponent,
        canActivate: [RedirectRouteGuard],
    },
    {
        path: 'client',
        loadChildren: () =>
            import('../project/fatawa/client/client.module').then(
                (m) => m.ClientModule,
            ),
    },
    {
        path: 'auth',
        loadChildren: () =>
            import('../auth/auth.module').then((m) => m.AuthModule),
    },
    {
        path: 'landing',
        canActivate: [AuthGuard],
        canActivateChild: [AuthGuard],
        loadChildren: () =>
            import('../landing/landing.module').then((m) => m.LandingModule),
    },
    {
        path: 'fatawa',
        loadChildren: () =>
            import('../project/fatawa/fatawa.module').then(
                (m) => m.FatawaModule,
            ),
    },
    {
        path: 'system-definition',
        canActivate: [SuperAdminAuthGuard, StudentAdminAuthGuard],
        canActivateChild: [SuperAdminAuthGuard, StudentAdminAuthGuard],
        loadChildren: () =>
            import(
                '../project/system-definition/system-definition.module'
            ).then((m) => m.SystemDefinitionModule),
    },
    {
        path: 'users',
        canActivate: [UsersAdminAuthGuard],
        canActivateChild: [UsersAdminAuthGuard],
        loadChildren: () =>
            import('../user-managments/users/users.module').then(
                (m) => m.UsersModule,
            ),
    },
    {
        path: 'language-settings',
        canActivate: [SuperAdminAuthGuard],
        canActivateChild: [SuperAdminAuthGuard],
        loadChildren: () =>
            import(
                '../project/system-definition/languages-settings/language.module'
            ).then((m) => m.LanguageModule),
    },
    {
        path: '**',
        component: NotFoundComponent,
        data: { title: 'Not Found' },
    },
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
})
export class AppRoutingModule {}

export const components = [NotFoundComponent];
