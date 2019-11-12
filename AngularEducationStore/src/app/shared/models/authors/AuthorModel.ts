import { BaseModel } from 'src/app/shared/models/base/BaseModel';
import { AuthorModelItem } from 'src/app/shared/models/authors/AuthorModelItem';

export class AuthorModel extends BaseModel {
    public items = new Array<AuthorModelItem>();
}
