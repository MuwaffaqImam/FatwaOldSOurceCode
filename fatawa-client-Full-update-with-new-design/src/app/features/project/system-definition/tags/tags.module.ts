import { NgModule } from '@angular/core';
import { SharedModule } from '@app/infrastructure/shared/shared.module';
import {
    TagRoutingModule,
    components as tagsComponents,
} from './tags-routing.module';
import { CommonModule } from '@angular/common';

@NgModule({
    declarations: [tagsComponents],
    imports: [CommonModule, SharedModule, TagRoutingModule],
})
export class TagsModule {}
