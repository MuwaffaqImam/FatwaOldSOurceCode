import { NgModule } from '@angular/core';

import {
    LanguageRoutingModule,
    components as languageComponents,
} from './language-routing.module';
import { CommonModule } from '@angular/common';
import { SharedModule } from '@app/infrastructure/shared/shared.module';

@NgModule({
    declarations: [ languageComponents ],
    imports: [
        CommonModule,
        SharedModule,
        LanguageRoutingModule,
    ],
})
export class LanguageModule { }
