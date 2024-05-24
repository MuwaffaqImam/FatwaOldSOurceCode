import { NgModule } from '@angular/core';

import { FatawaRoutingModule, components } from './fatawa-routing.module';
import { SharedModule } from '@app/infrastructure/shared/shared.module';
import { FatawaLayoutComponent } from './fatawa-layout/fatawa-layout.component';
import { CommonModule } from '@angular/common';
import { DataTreeService } from '@app/infrastructure/models/nodeTree';
import { FatawaTranslationComponent } from './fatawa-translation/fatawa-translation.component';
import { FatawaImportComponent } from './fatawa-import/fatawa-import.component';
import { FormsModule } from '@angular/forms';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import { FatawaExportComponent } from './fatawa-export/fatawa-export.component';
@NgModule({
    declarations: [
        components,
        FatawaLayoutComponent,
        FatawaTranslationComponent,
        FatawaImportComponent,
        FatawaExportComponent,
    ],
    imports: [
        CommonModule,
        SharedModule,
        FatawaRoutingModule,
        FormsModule,
        CKEditorModule,
    ],
    providers: [DataTreeService],
})
export class FatawaModule {}
