import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LandingLayoutComponent } from '@app/features/landing';
import { UsersListComponent } from './users-list/users-list.component';
import { UsersRolesComponent } from './users-roles/users-roles.component';
import { AddUserComponent } from './add-user/add-user.component';
import {
    components as fatwaUserscomponent,
    routes as fatawaUsersRoutes,
} from '../../project/fatawa/fatawa-users/fatawa-users-routing.module';
const routes: Routes = [
    {
        path: '',
        component: LandingLayoutComponent,
        children: [
            {
                path: '',
                redirectTo: 'users-list',
                pathMatch: 'full',
            },
            {
                path: 'users-list',
                component: UsersListComponent,
            },
            {
                path: 'fatwa-users',
                children: fatawaUsersRoutes,
            },
            {
                path: 'adduser',
                component: AddUserComponent,
            },
        ],
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class UserRoutingModule {}

export const components = [
    UsersListComponent,
    UsersRolesComponent,
    fatwaUserscomponent,
];
