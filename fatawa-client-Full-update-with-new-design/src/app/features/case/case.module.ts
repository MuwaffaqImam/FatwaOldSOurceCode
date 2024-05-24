import { NgModule } from '@angular/core';

import {
    CaseRoutingModule,
    components as caseComponents,
} from './case-routing.module';
import { SharedModule } from '@shared/shared.module';

@NgModule({
    declarations: [caseComponents],
    imports: [SharedModule, CaseRoutingModule],
})
export class CaseModule {}
