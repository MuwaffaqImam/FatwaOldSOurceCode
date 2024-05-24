import { NgModule } from '@angular/core';

import {
    AuthRoutingModule,
    components as authRoutedComponents,
} from './auth-routing.module';
import { SharedModule } from '@shared/shared.module';

@NgModule({
    declarations: [authRoutedComponents],
    imports: [SharedModule, AuthRoutingModule],
})
export class AuthModule {}
