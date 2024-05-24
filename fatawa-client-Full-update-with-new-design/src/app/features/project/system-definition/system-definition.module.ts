import { NgModule } from '@angular/core';
import {
    SystemDefinitionRoutingModule,
    components as systemDefinitionComponents,
} from './system-definition-routing.module';
import { SharedModule } from '@app/infrastructure/shared/shared.module';
import { CommonModule } from '@angular/common';

@NgModule({
    declarations: [systemDefinitionComponents],
    imports: [CommonModule, SharedModule, SystemDefinitionRoutingModule],
})
export class SystemDefinitionModule {}
