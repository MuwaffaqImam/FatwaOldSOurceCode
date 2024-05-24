import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
    FatawaArchivedRoutingModule,
    components as fatawaArchivedComponent,
} from './fatawa-archived-routing.module';
import { SharedModule } from '@app/infrastructure/shared/shared.module';

@NgModule({
    declarations: [fatawaArchivedComponent],
    imports: [CommonModule, SharedModule, FatawaArchivedRoutingModule],
})
export class FatawaArchivedModule {}
