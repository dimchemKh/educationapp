import { BaseModel } from 'src/app/shared/models/base/BaseModel';
import { AuthorModelItem } from './AuthorModelItem';

export class AuthorModel extends BaseModel {

    public items = new Array<AuthorModelItem>();
}
