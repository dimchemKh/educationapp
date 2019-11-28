import { Component, OnInit } from '@angular/core';
import { FilterOrderModel, OrderModel, PaymentModel } from 'src/app/shared/models';
import { OrderParameters } from 'src/app/shared/constants/order-parameters';
import { OrderService, DataService, PaymentService } from 'src/app/shared/services';
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
    public dataService: DataService,
    public printingEditionParams: PrintingEditionsParameters,
    public orderParameters: OrderParameters,
    private paymentService: PaymentService,
    public columnsTitles: ColumnsTitles
    ) {
    super(orderService, dataService, printingEditionParams, orderParameters, columnsTitles);
    this.columnsOrders = this.columnsTitles.columnsOrdersUser;
    this.filterModel = new FilterOrderModel();
    this.orderModel = new OrderModel();
  }
  pay(order): void {
    let payment = new PaymentModel();
    
    payment.orderId = order.id;

    this.paymentService.openStripeDialog(payment);
    
    this.submit();
  }
  ngOnInit(): void {
    this.orderService.getOrders(this.dataService.getLocalStorage('userRole'), this.filterModel)
      .subscribe((data) => {
        this.orderModel = data;
      });
  }
}
