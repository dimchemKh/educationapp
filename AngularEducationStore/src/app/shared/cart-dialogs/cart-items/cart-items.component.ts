import { Component, OnInit } from '@angular/core';
import { OrderItemModel } from 'src/app/shared/models/order-item/OrderItemModel';
import { OrderService } from 'src/app/shared/services/order.service';
import { PrintingEditionsParametrs } from 'src/app/shared/constants/printing-editions-parametrs';
import { OrderItemModelItem } from 'src/app/shared/models/order-item/OrderItemModelItem';
import { PrintingEditionsComponent } from 'src/app/printing-edition/printing-editions/printing-editions.component';
import { MatDialogRef, MatDialog } from '@angular/material';
import { PaymentService } from '../../services/payment.service';
import { CartTransactionComponent } from '../cart-transaction/cart-transaction.component';

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
  invalidError: any;

  constructor(private dialog: MatDialog,
              public dialogRef: MatDialogRef<PrintingEditionsComponent>, private orderService: OrderService,
              private parametrs: PrintingEditionsParametrs, private paymentService: PaymentService) {
    for (let i = 1; i < 10; i++) {
      this.quantities.push(i);
    }
  }

  ngOnInit() {
    this.getOrders();
  }
  buy(orders: OrderItemModel) {
    this.dialogRef.close();
    this.dialog.open(CartTransactionComponent, {
      data: { orders }
    });
  }
  pay(amount = 10) {    
 
    var handler = (<any>window).StripeCheckout.configure({
      key: 'pk_test_tlcMD8vu8ttNtVSH6RF3OAkp004sTIYGEr',
      locale: 'auto',
      token: function (token: any) {
        // You can access the token ID with `token.id`.
        // Get the token ID to your server-side code for use.
        console.log(token)
        alert('Token Created!!');
      }
    });
 
    handler.open({
      name: 'Demo Site',
      description: '2 widgets',
      amount: amount * 100
    });
 
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
