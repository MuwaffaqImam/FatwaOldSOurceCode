import { NgModule } from '@angular/core';
import {
    UserRoutingModule,
    components as usersComponents,
} from './users-routing.module';
import { SharedModule } from '@app/infrastructure/shared/shared.module';
import { CommonModule } from '@angular/common';
import { DataUserTreeService } from '@app/infrastructure/models/usernodeTree';
import { AddUserComponent } from './add-user/add-user.component';

@NgModule({
    declarations: [usersComponents, AddUserComponent],
    imports: [CommonModule, SharedModule, UserRoutingModule],
    providers: [DataUserTreeService],
})
export class UsersModule {}
