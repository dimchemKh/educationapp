import { BaseModel } from 'src/app/shared/models/base/BaseModel';
import { OrderItemModelItem } from 'src/app/shared/models/order-item/OrderItemModelItem';

export class OrderModelItem extends BaseModel {
    public id: number;
        public date: Date; 
        public userName: string;
        public email: string;
        public amount: number;
        public currency: number; 
        public transactionStatus: number;
        public paymentId: number;
        public orderItems = new Array<OrderItemModelItem>();
}
