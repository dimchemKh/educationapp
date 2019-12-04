import { Component, OnInit } from '@angular/core';
import { FilterOrderModel, OrderModel, PaymentModel, OrderModelItem } from 'src/app/shared/models';
import { OrderParameters } from 'src/app/shared/constants/order-parameters';
import { OrderService, PaymentService } from 'src/app/shared/services';
import { PrintingEditionsParameters } from 'src/app/shared/constants/printing-editions-parameters';
import { OrdersAdminComponent } from 'src/app/order/orders-admin/orders-admin.component';
import { ColumnsTitles } from 'src/app/shared/constants/columns-titles';

@Component({
  selector: 'app-orders-user',
  templateUrl: './orders-user.component.html',
  styleUrls: ['./orders-user.component.scss']
})

export class OrdersUserComponent extends OrdersAdminComponent implements OnInit {

  filterModel: FilterOrderModel;
  orderModel: OrderModel;

  columnsOrders: string[];

  constructor(public orderService: OrderService,
    public printingEditionParams: PrintingEditionsParameters,
    public orderParameters: OrderParameters,
    private paymentService: PaymentService,
    public columnsTitles: ColumnsTitles
    ) {
    super(orderService, printingEditionParams, orderParameters, columnsTitles);
    this.filterModel = new FilterOrderModel();
    this.orderModel = new OrderModel();
    this.columnsOrders = this.columnsTitles.columnsOrdersUser;
  }
  pay(order: OrderModelItem): void {
    let payment = new PaymentModel();
    
    payment.orderId = order.id;

    this.paymentService.openStripeDialog(payment);
    
    this.submit();
  }
  ngOnInit(): void {
    // this.orderService.getOrders(this.dataService.getLocalStorage('userRole'), this.filterModel)
    //   .subscribe((data) => {
    //     this.orderModel = data;
    //   });
  }
}
