

export class BaseFilterModel {
    searchString?: string;
    sortState = 0;
    sortType?: number;
    pageSize: number;
    page = 1;
}
