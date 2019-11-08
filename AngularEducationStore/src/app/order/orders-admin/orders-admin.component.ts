import { Component, OnInit } from '@angular/core';
import { FilterOrderModel } from 'src/app/shared/models/filter/filter-order-model';
import { OrderModel } from 'src/app/shared/models/order/OrderModel';
import { OrderParametrs } from 'src/app/shared/constants/order-parametrs';

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

  constructor(private parametrs: OrderParametrs) {

  }

  ngOnInit() {
    
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
