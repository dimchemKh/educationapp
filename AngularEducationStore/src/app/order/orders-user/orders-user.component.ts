import { Component, OnInit } from '@angular/core';
import { FilterOrderModel } from 'src/app/shared/models/filter/filter-order-model';
import { OrderModel } from 'src/app/shared/models/order/OrderModel';
import { TransactionStatus } from 'src/app/shared/enums/transaction-status';
import { OrderParametrs } from 'src/app/shared/constants/order-parametrs';
import { OrderService } from 'src/app/shared/services/order.service';
import { DataService } from 'src/app/shared/services/data.service';
import { PrintingEditionsParametrs } from 'src/app/shared/constants/printing-editions-parametrs';
import { OrdersAdminComponent } from '../orders-admin/orders-admin.component';
import { OrderModelItem } from 'src/app/shared/models/order/OrderModelItem';
import { Model } from 'src/app/shared/models/model';

@Component({
  selector: 'app-orders-user',
  templateUrl: './orders-user.component.html',
  styleUrls: ['./orders-user.component.scss']
})



export class OrdersUserComponent extends OrdersAdminComponent implements OnInit {

  filterModel = new FilterOrderModel();
  orderModel = new OrderModel();

  displayedColumns = ['id', 'date', 'printingEditionType', 'quantity', 'amount', 'transactionStatus'];

  constructor(public parametrs: OrderParametrs, public orderService: OrderService, public dataService: DataService,
    public printingEditionParams: PrintingEditionsParametrs, public orderParametrs: OrderParametrs) {

    super(parametrs, orderService, dataService, printingEditionParams, orderParametrs);
  }
  pay(order) {
    let model = new Model();
    model.order = order.id;
    model.transaction = 'tok_q1qw';

    this.orderService.updateOrder(model).subscribe(() => {
      debugger
    });
    
    // let handler = (window as any).StripeCheckout.configure({
    //   key: 'pk_test_tlcMD8vu8ttNtVSH6RF3OAkp004sTIYGEr',
    //   locale: 'auto',
    //   token: (token: any) => {
    //     // console.log(token);
    //     this.orderService.updateOrder(order.id, token.id).then(() => {
    //       let qw = token.id;
    //       debugger
    //     });
    //   }
    // });

    // handler.open({
    //   name: 'Localhost',
    //   description: 'Payment description'
    // });

  }
  ngOnInit() {
    this.orderService.getOrders(this.dataService.getLocalStorage('userRole'), this.filterModel)
      .subscribe((data) => {
        this.orderModel = data;
      });
  }
}
