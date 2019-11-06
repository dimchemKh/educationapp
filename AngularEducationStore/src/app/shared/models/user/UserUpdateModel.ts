import { BaseModel } from 'src/app/shared/models/base/BaseModel';

export class UserUpdateModel extends BaseModel {
    public id: number;
    public firstName: string;
    public lastName: string;
    public email: string;
    public currentPassword: string;
    public newPassword?: string;
}
