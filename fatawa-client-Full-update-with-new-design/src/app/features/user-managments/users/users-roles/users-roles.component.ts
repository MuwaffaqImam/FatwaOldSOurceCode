import {
    Component,
    OnInit,
    ChangeDetectionStrategy,
    Inject,
} from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { RoleModel } from '@app/infrastructure/models/project/RoleModel';
import { UserModel } from '@app/infrastructure/models/project/UserModel';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
    selector: 'app-users-roles',
    templateUrl: './users-roles.component.html',
    styleUrls: ['./users-roles.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UsersRolesComponent implements OnInit {
    userModel: UserModel;
    roles: RoleModel[] = [];
    currentRoleUser: number = 0;
    changeRoleId: number = 0;
    constructor(
        private dialogRef: MatDialogRef<UsersRolesComponent>,
        @Inject(MAT_DIALOG_DATA) public roleModel: any,
    ) {
        this.roles = roleModel.allRoles;
        this.currentRoleUser = roleModel.roleId;
    }

    ngOnInit(): void {
        this.changeRoleId = this.currentRoleUser;
    }

    updateRole() {
        this.dialogRef.close(this.changeRoleId);
    }

    changeRole(roleId: number) {
        this.changeRoleId = roleId;
    }

    Close() {
        this.dialogRef.close(this.currentRoleUser);
    }
}
