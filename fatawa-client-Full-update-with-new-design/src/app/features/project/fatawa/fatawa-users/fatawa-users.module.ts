import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FatawaUserRoutingModule } from './fatawa-users-routing.module';
import { SharedModule } from '@app/infrastructure/shared/shared.module';

@NgModule({
    declarations: [],
    imports: [CommonModule, FatawaUserRoutingModule, SharedModule],
})
export class FatawausersModule {}
