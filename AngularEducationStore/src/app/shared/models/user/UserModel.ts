import { BaseModel } from 'src/app/shared/models/base/BaseModel';
import { UserModelItem } from 'src/app/shared/models/user/UserModelItem';

export class UserModel extends BaseModel {
  [x: string]: any;
    public items = Array<UserModelItem>();
    public itemsCount: number;
}
