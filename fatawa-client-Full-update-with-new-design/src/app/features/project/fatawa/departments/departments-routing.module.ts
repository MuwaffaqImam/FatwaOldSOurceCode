import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { addDepartmentComponent } from './addDepartment/addDepartment.component';
import { DepartmentsListComponent } from './departmentsList/departments-list.component';

export const routes: Routes = [
    {
        path: '',
        component: DepartmentsListComponent,
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class DepartmentsRoutingModule {}

export const components = [DepartmentsListComponent, addDepartmentComponent];
