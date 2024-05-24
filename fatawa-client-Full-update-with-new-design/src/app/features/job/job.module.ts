import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';

import {
    JobRoutingModule,
    components as jobComponents,
} from './job-routing.module';

@NgModule({
    declarations: [jobComponents],
    imports: [SharedModule, JobRoutingModule],
})
export class JobModule {}
