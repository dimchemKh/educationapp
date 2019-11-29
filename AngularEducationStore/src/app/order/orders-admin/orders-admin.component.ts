import { Component, OnInit } from '@angular/core';
import { FilterOrderModel, OrderModel } from 'src/app/shared/models';
import { OrderParameters } from 'src/app/shared/constants/order-parameters';
import { OrderService, DataService } from 'src/app/shared/services';
import { PrintingEditionsParameters } from 'src/app/shared/constants/printing-editions-parameters';
import { MatSort, PageEvent, MatSelectChange } from '@angular/material';
import { ColumnsTitles } from 'src/app/shared/constants/columns-titles';
import { PageSize, TransactionStatus } from 'src/app/shared/enums';
import { ProductPresentationModel } from 'src/app/shared/models/presentation/ProductPresenatationModel';
import { SortStatesPresentationModel } from 'src/app/shared/models/presentation/SortStatesPresentationModel';
import { SortTypesPresentationModel } from 'src/app/shared/models/presentation/SortTypesPresentationModel';
import { OrderStatusPresentationModel } from 'src/app/shared/models/presentation/OrderStatusPresentationModel';

@Component({
  selector: 'app-orders-admin',
  templateUrl: './orders-admin.component.html',
  styleUrls: ['./orders-admin.component.scss']
})
export class OrdersAdminComponent implements OnInit {

  filterModel: FilterOrderModel;
  orderModel: OrderModel;
  isRequire: number;
  columnsOrders: string[];
  pageSizes: PageSize[];
  productPresentationModels: Array<ProductPresentationModel>;
  orderStatusModels: Array<OrderStatusPresentationModel>;
  sortStatesModels: Array<SortStatesPresentationModel>;
  sortTypesModels: Array<SortTypesPresentationModel>;
  
  transactionStatus: TransactionStatus[];

  constructor(public orderService: OrderService,
    public dataService: DataService,
    public printingEditionParams: PrintingEditionsParameters,
    public orderParametrs: OrderParameters,
    public columnsTitles: ColumnsTitles
    ) {
    this.filterModel = new FilterOrderModel();
    this.orderModel = new OrderModel();
    this.columnsOrders = this.columnsTitles.columnsOrdersAdmin;
    this.pageSizes = this.orderParametrs.pageSizes;
    this.productPresentationModels = this.printingEditionParams.productPresentationModels;
    this.orderStatusModels = this.orderParametrs.orderStatusPresentationModels;
    this.sortStatesModels = this.printingEditionParams.sortStateModels;
    this.sortTypesModels = this.orderParametrs.sortTypesModels;
    this.transactionStatus = [TransactionStatus.Paid, TransactionStatus.UnPaid];
  }

  ngOnInit(): void {
    this.submit();
  }
  
  sortData(event: MatSort): void {
    let sortState = this.sortStatesModels.find(x => x.direction.toLowerCase() === event.direction.toLowerCase());

    this.filterModel.sortState = sortState.value;

    let sortTypes = this.sortTypesModels.find(x => x.name.toLowerCase() === event.active.toLowerCase());

    this.filterModel.sortType = sortTypes.value;

    this.submit();
  }

  changeTypeSelect(event: MatSelectChange): void {
    if (event.value.length === 1) {
      this.isRequire = event.value[0];
    }

    if (event.value.length === 0) {
      event.source.value = [this.isRequire];
      this.transactionStatus = [this.isRequire];
    }
  }

  closedTypeSelect(event: boolean): void {
    if (!event) {
      this.submit();
    }
  }

  pageEvent(event: PageEvent): void {
    let page = event.pageIndex + 1;

    if (event.pageSize !== this.filterModel.pageSize) {
      page = 1;
      this.filterModel.pageSize = event.pageSize;
    }

    this.submit(page);
  }

  submit(page: number = 1): void {
    this.filterModel.page = page;

    this.filterModel.transactionStatus = this.transactionStatus.length > 1 ? TransactionStatus.All : this.transactionStatus[0];

    this.orderService.getOrders(this.dataService.getLocalStorage('userRole'), this.filterModel)
    .subscribe((data) => {
      this.orderModel = data;
    });
  }
}
