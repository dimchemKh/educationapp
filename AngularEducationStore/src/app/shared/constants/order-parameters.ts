import { BaseParameters } from 'src/app/shared/constants/base-parameters';
import { TransactionStatus } from 'src/app/shared/enums/transaction-status';
import { SortType } from 'src/app/shared/enums/sort-type';
import { OrderStatusPresentationModel } from 'src/app/shared/models/presentation/OrderStatusPresentationModel';

export class OrderParameters extends BaseParameters {

    public readonly orderStatusPresentationModels: Array<OrderStatusPresentationModel> = [
        { name: TransactionStatus[TransactionStatus.Paid], value: TransactionStatus.Paid },
        { name: TransactionStatus[TransactionStatus.UnPaid], value: TransactionStatus.UnPaid }
    ];

    public readonly sortTypesModels = [
        { name: SortType[SortType.Id], value: SortType.Id },
        { name: SortType[SortType.Date], value: SortType.Date },
        { name: SortType[SortType.Amount], value: SortType.Amount }
    ];
}
