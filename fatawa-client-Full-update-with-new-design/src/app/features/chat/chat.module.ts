import { NgModule } from '@angular/core';

import {
    ChatRoutingModule,
    components as chatComponents,
} from './chat-routing.module';
import { SharedModule } from '@shared/shared.module';

@NgModule({
    declarations: [chatComponents],
    imports: [SharedModule, ChatRoutingModule],
})
export class ChatModule {}
