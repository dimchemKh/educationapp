import { BaseModel } from 'src/app/shared/models/base/BaseModel';
import { OrderModelItem } from 'src/app/shared/models/order/OrderModelItem';

export class OrderModel extends BaseModel {
    public items = new Array<OrderModelItem>();
}
