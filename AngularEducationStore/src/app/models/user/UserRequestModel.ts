import { BaseModel } from '../base/BaseModel';

export class UserRequestModel extends BaseModel {
    userName?: string;
    userId?: string;
    userRole?: string;
}
