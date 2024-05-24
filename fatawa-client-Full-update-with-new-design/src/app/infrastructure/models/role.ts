import { UserType } from './user';

export interface IRoleDTO {
    name: UserType;
}

export class RoleDTO implements IRoleDTO {
    name: UserType = UserType.None;

    static isRoleType(role: UserType): role is UserType {
        const roleTypeList: UserType[] = [
            UserType.UsersAdmin,
            UserType.SuperAdmin,
            UserType.Admin,
            UserType.AdminGroup,
            UserType.Student,
            UserType.Guest,
            UserType.StudentAdmin,
        ];
        return roleTypeList.includes(role);
    }

    static isUsersAdminRoleType(role: UserType): role is UserType {
        const typeList: UserType[] = [UserType.UsersAdmin];
        return typeList.includes(role);
    }

    static isStudentsAdminRoleType(role: UserType): role is UserType {
        const typeList: UserType[] = [UserType.StudentAdmin];
        return typeList.includes(role);
    }

    static isSuperAdminRoleType(role: UserType): role is UserType {
        const typeList: UserType[] = [UserType.SuperAdmin];
        return typeList.includes(role);
    }

    static isAdminRoleType(role: UserType): role is UserType {
        const typeList: UserType[] = [UserType.Admin];
        return typeList.includes(role);
    }

    static isAdminGroupRoleType(role: UserType): role is UserType {
        const typeList: UserType[] = [UserType.AdminGroup];
        return typeList.includes(role);
    }

    static isStudentRoleType(role: UserType): role is UserType {
        const typeList: UserType[] = [UserType.Student];
        return typeList.includes(role);
    }

    static isGuestRoleType(role: UserType): role is UserType {
        const typeList: UserType[] = [UserType.Guest];
        return typeList.includes(role);
    }

    static isMatchRoleType(
        role: UserType,
        roleMatch: UserType[],
    ): role is UserType {
        return roleMatch.includes(role);
    }

    constructor(configOverride?: IRoleDTO) {
        if (configOverride) {
            Object.assign(this, configOverride);
        }
    }
}
