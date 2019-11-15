import { Component, OnInit } from '@angular/core';
import { OrderItemModel } from 'src/app/shared/models/order-item/OrderItemModel';
import { OrderService } from 'src/app/shared/services/order.service';
import { PrintingEditionsParametrs } from 'src/app/shared/constants/printing-editions-parametrs';
import { OrderItemModelItem } from 'src/app/shared/models/order-item/OrderItemModelItem';
import { PrintingEditionsComponent } from 'src/app/printing-edition/printing-editions/printing-editions.component';
import { MatDialogRef, MatDialog } from '@angular/material';
import { PaymentService } from '../../services/payment.service';
import { CartTransactionComponent } from '../cart-transaction/cart-transaction.component';
import { Currency } from '../../enums/currency';
import { ConverterModel } from '../../models/ConverterModel';
import { PrintingEditionDetailsComponent } from 'src/app/printing-edition/printing-edition-details/printing-edition-details.component';
import { CartService } from '../../services/cart.service';
import { DataService } from '../../services/data.service';
import { OrderModelItem } from '../../models/order/OrderModelItem';

@Component({
  selector: 'app-cart-items',
  templateUrl: './cart-items.component.html',
  styleUrls: ['./cart-items.component.scss']
})
export class CartItemsComponent implements OnInit {

  isEmptyCart = false;
  quantities = Array<number>();
  quantity: number;

  converterModel = new ConverterModel();

  currencyTypes = this.parametrs.currencyTypes;
  orders = new OrderModelItem();
  // orderItemModel = new OrderModelItem();

  displayedColumns = ['product', 'price', 'qty', 'amount', ' '];
  invalidError: any;

  constructor(public dialogRef: MatDialogRef<PrintingEditionDetailsComponent>, private cartService: CartService,
              private parametrs: PrintingEditionsParametrs, private orderService: OrderService, private dataService: DataService) {
    for (let i = 1; i < 10; i++) {
      this.quantities.push(i);
    }
    this.converterModel.currencyTo = Currency.USD;
    this.converterModel.currencyFrom = Currency.USD;
  }

  ngOnInit() {
    this.getOrders();
    this.getOrdersAmount();
  }
  convertAmount() {
    this.cartService.convertToCart(this.converterModel).then((x) => {
      this.converterModel.price = x;
    });
    this.converterModel.currencyFrom = this.converterModel.currencyTo;
    return this.converterModel.price;
  }
  pay(orders: OrderModelItem) {
    this.dialogRef.close();
    
    this.orderService.createOrder(this.dataService.getLocalStorage('userRole'), orders).subscribe((data) => {
      if (data.errors.length <= 0) {
        this.dataService.deleteItemLocalStorage('cartItems');
        this.cartService.cartSource.next([]);
      }
    });

    // this.orderItemModel.orderItems.push()
    let handler = (window as any).StripeCheckout.configure({
      key: 'pk_test_tlcMD8vu8ttNtVSH6RF3OAkp004sTIYGEr',
      locale: 'auto',
      token: (token: any) => {
        // orders.
      }
    });

    handler.open({
      name: 'Localhost',
      description: 'Payment description',
      order: orders
    });

  }
  getOrdersAmount() {
    this.converterModel.price = 0;
    this.orders.orderItems.forEach((x) => {
      this.converterModel.price += (x.count * x.price);
    });

    return this.converterModel.price;
  }
  getOrders() {
    let orders = this.cartService.getAllPurchases();
    if (!orders || orders.orderItems.length === 0) {
      this.isEmptyCart = true;
      return;
    }
    this.orders = orders;
  }
  removeOrderItem(printingEditionId: number) {
    this.cartService.removeOrderItem(printingEditionId);
    this.getOrders();
    this.getOrdersAmount();
  }
  close() {
    this.dialogRef.close();
  }
}
