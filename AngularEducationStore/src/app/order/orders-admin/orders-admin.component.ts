import { Component, OnInit, EventEmitter } from '@angular/core';
import { FilterOrderModel } from 'src/app/shared/models/filter/filter-order-model';
import { OrderModel } from 'src/app/shared/models/order/OrderModel';
import { OrderParametrs } from 'src/app/shared/constants/order-parametrs';
import { OrderService } from 'src/app/shared/services/order.service';
import { DataService } from 'src/app/shared/services/data.service';
import { PrintingEditionsParametrs } from 'src/app/shared/constants/printing-editions-parametrs';
import { TransactionStatus } from 'src/app/shared/enums/transaction-status';
import { MatSort, PageEvent, MatSelectChange } from '@angular/material';

@Component({
  selector: 'app-orders-admin',
  templateUrl: './orders-admin.component.html',
  styleUrls: ['./orders-admin.component.scss']
})
export class OrdersAdminComponent implements OnInit {

  filterModel = new FilterOrderModel();
  orderModel = new OrderModel();
  isRequire: number;
  displayedColumns = ['id', 'date', 'userName', 'userEmail', 'printingEditionType', 'quantity', 'amount', 'transactionStatus'];

  pageSizes = this.parametrs.pageSizes;
  printingEditionTypes = this.printingEditionParams.printingEditionTypes;
  transactionStatuses = this.orderParametrs.transactionStatuses;
  sortStates = this.printingEditionParams.sortStates;
  sortTypes = this.orderParametrs.sortTypes;
  
  transactionStatus = [TransactionStatus.Paid, TransactionStatus.UnPaid];

  constructor(public parametrs: OrderParametrs, public orderService: OrderService, public dataService: DataService,
              public printingEditionParams: PrintingEditionsParametrs, public orderParametrs: OrderParametrs) {
  }

  ngOnInit() {
    this.orderService.getOrders(this.dataService.getLocalStorage('userRole'), this.filterModel)
      .subscribe((data) => {
        this.orderModel = data;
      });
  }
  sortData(event: MatSort) {
    let sortState = this.sortStates.find(x => x.direction.toLowerCase() === event.direction.toLowerCase());
    this.filterModel.sortState = sortState.value;

    let sortTypes = this.sortTypes.find(x => x.name.toLowerCase() === event.active.toLowerCase());
    this.filterModel.sortType = sortTypes.value;

    this.submit();
  }
  changeTypeSelect(event: MatSelectChange) {
    if (event.value.length === 1) {
      this.isRequire = event.value[0];
    }
    if (event.value.length === 0) {
      event.source.value = [this.isRequire];
      this.transactionStatus = [this.isRequire];
    }
  }
  closedTypeSelect(event: boolean) {
    if (!event) {
      this.submit();
    }
  }
  pageEvent(event: PageEvent) {
    let page = event.pageIndex + 1;

    if (event.pageSize !== this.filterModel.pageSize) {
      page = 1;
      this.filterModel.pageSize = event.pageSize;
    }
    this.submit(page);
  }
  submit(page: number = 1) {
    this.filterModel.page = page;

    this.filterModel.transactionStatus = this.transactionStatus.length > 1 ? TransactionStatus.All : this.transactionStatus[0];

    this.orderService.getOrders(this.dataService.getLocalStorage('userRole'), this.filterModel)
    .subscribe((data) => {
      this.orderModel = data;
    });
  }
}
