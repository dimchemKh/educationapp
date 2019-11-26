import { BaseModel } from 'src/app/shared/models/base/BaseModel';

export class UserRequestModel extends BaseModel {
    userName?: string;
    userId?: string;
    userRole?: string;
    image?: string;
}
