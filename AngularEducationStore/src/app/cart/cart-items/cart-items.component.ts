import { Component, OnInit } from '@angular/core';
import { OrderItemModel } from 'src/app/shared/models/order-item/OrderItemModel';
import { OrderService } from 'src/app/shared/services/order.service';
import { PrintingEditionsParametrs } from 'src/app/shared/constants/printing-editions-parametrs';
import { OrderItemModelItem } from 'src/app/shared/models/order-item/OrderItemModelItem';
import { PrintingEditionsComponent } from 'src/app/printing-edition/printing-editions/printing-editions.component';
import { MatDialogRef } from '@angular/material';

@Component({
  selector: 'app-cart-items',
  templateUrl: './cart-items.component.html',
  styleUrls: ['./cart-items.component.scss']
})
export class CartItemsComponent implements OnInit {

  isEmptyCart = false;
  quantities = Array<number>();
  quantity: number;

  currencyTypes = this.parametrs.currencyTypes;
  orders = new OrderItemModel();

  displayedColumns = ['product', 'price', 'qty', 'amount', ' '];

  constructor(public dialogRef: MatDialogRef<PrintingEditionsComponent>, private orderService: OrderService,
              private parametrs: PrintingEditionsParametrs) {
    for (let i = 1; i < 10; i++) {
      this.quantities.push(i);
    }
  }

  ngOnInit() {
    this.getOrders();
  }
  buy(orders: OrderItemModel) {
    console.log(orders);
  }
  getOrdersAmount() {
    let amount = 0;
    this.orders.items.forEach((x: OrderItemModelItem) => {
      amount += (x.count * x.price);
    });
    return amount;
  }
  getOrders() {
    let orders = this.orderService.getAllPurchases();
    if (!orders || orders.items.length === 0) {
      this.isEmptyCart = true;
      return;
    }
    this.orders = orders;
  }
  removeOrderItem(printingEditionId: number) {
    this.orderService.removeOrderItem(printingEditionId);
    this.getOrders();
  }
  close() {
    this.dialogRef.close();
  }
}
