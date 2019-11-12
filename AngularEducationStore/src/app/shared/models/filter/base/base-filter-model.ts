

export class BaseFilterModel {
    searchString?: string;
    sortState = 1;
    sortType?: number;
    pageSize: number;
    page = 1;
}
