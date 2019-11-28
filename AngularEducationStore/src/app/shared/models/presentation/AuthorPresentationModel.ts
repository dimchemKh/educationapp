import { SortType } from 'src/app/shared/enums';

export class AuthorPresentationModel {
    name: string;
    value: SortType;

    public constructor(name: string, value: SortType) {
        this.name = name;
        this.value = value;
    }
}
