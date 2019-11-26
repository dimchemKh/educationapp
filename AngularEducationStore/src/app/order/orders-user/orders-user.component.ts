import { Component, OnInit } from '@angular/core';
import { FilterOrderModel } from 'src/app/shared/models/filter/filter-order-model';
import { OrderModel } from 'src/app/shared/models/order/OrderModel';
import { OrderParametrs } from 'src/app/shared/constants/order-parametrs';
import { OrderService } from 'src/app/shared/services/order.service';
import { DataService } from 'src/app/shared/services/data.service';
import { PrintingEditionsParametrs } from 'src/app/shared/constants/printing-editions-parametrs';
import { OrdersAdminComponent } from '../orders-admin/orders-admin.component';
import { PaymentModel } from 'src/app/shared/models/payment/PaymentModel';
import { PaymentService } from 'src/app/shared/services';

@Component({
  selector: 'app-orders-user',
  templateUrl: './orders-user.component.html',
  styleUrls: ['./orders-user.component.scss']
})

export class OrdersUserComponent extends OrdersAdminComponent implements OnInit {

  filterModel = new FilterOrderModel();
  orderModel = new OrderModel();

  displayedColumns = ['id', 'date', 'printingEditionType', 'printingEditionTitle', 'quantity', 'amount', 'transactionStatus'];

  constructor(public parametrs: OrderParametrs, public orderService: OrderService, public dataService: DataService,
              public printingEditionParams: PrintingEditionsParametrs, public orderParametrs: OrderParametrs,
              private paymentService: PaymentService) {

    super(parametrs, orderService, dataService, printingEditionParams, orderParametrs);
  }
  pay(order) {
    let payment = new PaymentModel();
    payment.orderId = order.id;

    this.paymentService.openStripeDialog(payment);
    
    this.submit();
  }
  ngOnInit() {
    this.orderService.getOrders(this.dataService.getLocalStorage('userRole'), this.filterModel)
      .subscribe((data) => {
        this.orderModel = data;
      });
  }
}
