import { PageSize } from 'src/app/shared/enums/page-size';

export class BaseFilterModel {
    searchString?: string;
    sortState?: number;
    sortType?: number;
    pageSize: number;
    page = 1;
}
