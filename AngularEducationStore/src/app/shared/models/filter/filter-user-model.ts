import { BaseFilterModel } from 'src/app/shared/models/filter/base/base-filter-model';
import { PageSize } from 'src/app/shared/enums/page-size';
import { IsBlocked } from 'src/app/shared/enums/is-blocked';

export class FilterUserModel extends BaseFilterModel {
    isBlocked: IsBlocked;
}
