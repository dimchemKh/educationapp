import { Component, OnInit } from '@angular/core';
import { FilterOrderModel } from 'src/app/shared/models/filter/filter-order-model';
import { OrderModel } from 'src/app/shared/models/order/OrderModel';
import { OrderParametrs } from 'src/app/shared/constants/order-parametrs';
import { OrderService } from 'src/app/shared/services/order.service';
import { DataService } from 'src/app/shared/services/data.service';

@Component({
  selector: 'app-orders-admin',
  templateUrl: './orders-admin.component.html',
  styleUrls: ['./orders-admin.component.scss']
})
export class OrdersAdminComponent implements OnInit {


  filterModel = new FilterOrderModel();
  orderModel = new OrderModel();

  displayedColumns = ['id', 'data', 'userName', 'userEmail', 'printingEditionType', 'quantity', 'amount', 'transactionStatus'];
  pageSizes = this.parametrs.pageSizes;

  constructor(private parametrs: OrderParametrs, private orderService: OrderService, private dataService: DataService) {

  }

  ngOnInit() {
    this.orderService.getOrders(this.dataService.getLocalStorage('userRole'), this.filterModel)
    .subscribe((data) => {
      this.orderModel = data;
    });
  }

  openCreateDialog() {

  }
  sortData(event) {

  }
  openEditDialog(element) {

  }
  openRemoveDialog(element) {

  }
  pageEvent(event) {

  }
}
