import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '@app/infrastructure/shared/shared.module';
import { DepartmentsRoutingModule } from './departments-routing.module';

@NgModule({
    declarations: [],
    imports: [CommonModule, DepartmentsRoutingModule, SharedModule],
})
export class DepartmentsModule {}
