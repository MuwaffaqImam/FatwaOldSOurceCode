import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '@app/infrastructure/shared/shared.module';
import { FatawaLiveRoutingModule } from './fatawa-live-routing.module';

@NgModule({
    declarations: [FatawaLiveRoutingModule],
    imports: [CommonModule, SharedModule, FatawaLiveRoutingModule],
})
export class FatawaLiveModule {}
