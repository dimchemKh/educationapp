import { BaseFilterModel } from 'src/app/shared/models/filter/base/base-filter-model';
import { PageSize } from 'src/app/shared/enums/page-size';

export class FilterPrintingEditionModel extends BaseFilterModel {
    currency = 1;
    PriceMinValue = 0;
    PriceMaxValue = 10000;
    PrintingEditionTypes = new Array<number>();
    pageSize = PageSize.Six;
}
