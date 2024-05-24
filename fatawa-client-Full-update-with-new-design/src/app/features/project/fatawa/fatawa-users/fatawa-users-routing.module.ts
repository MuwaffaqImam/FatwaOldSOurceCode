import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AssignMuftiComponent } from './assign-mufti/assign-mufti.component';
import { FatawaUsersComponent } from './fatawa-users.component';

export const routes: Routes = [
    {
        path: '',
        component: FatawaUsersComponent,
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class FatawaUserRoutingModule {}

export const components = [FatawaUsersComponent, AssignMuftiComponent];
