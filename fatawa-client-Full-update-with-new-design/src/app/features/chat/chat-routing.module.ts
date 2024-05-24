import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ChatPresentationComponent } from './chat/chat-presentation.component';
import { ChatSmartComponent } from './chat/chat-smart.component';

const routes: Routes = [
    {
        path: '',
        component: ChatSmartComponent,
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ChatRoutingModule {}

export const components = [ChatPresentationComponent, ChatSmartComponent];
