import { BaseFilterModel } from 'src/app/shared/models/filter/base/base-filter-model';
import { PageSize } from '../../enums/page-size';

export class FilterOrderModel extends BaseFilterModel {
    pageSize = PageSize.Six;
    public transactionStatus: number;
}
