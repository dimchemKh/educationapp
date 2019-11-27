import { BaseParameters } from 'src/app/shared/constants/base-parameters';
import { TransactionStatus } from '../enums/transaction-status';
import { SortType } from '../enums/sort-type';

export class OrderParameters extends BaseParameters {
    public readonly transactionStatuses = [
        { name: TransactionStatus[TransactionStatus.Paid], value: TransactionStatus.Paid },
        { name: TransactionStatus[TransactionStatus.UnPaid], value: TransactionStatus.UnPaid }
    ];
    public readonly sortTypes = [
        { name: SortType[SortType.Id], value: SortType.Id },
        { name: SortType[SortType.Date], value: SortType.Date },
        { name: SortType[SortType.Amount], value: SortType.Amount }
    ];
}
