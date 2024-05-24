export interface UserModel {
    userId: number;
    userName: string;
    email: string;
    phoneNumber: string;
    isActive: boolean;
    dateOfBirth: Date;
    roleId: number;
    roleName: string;
    parentId: number;
    fullName: string;
}
export class UserTree {
    userId: number;
    userName: string;
    email: string;
    parentId: number;
}
