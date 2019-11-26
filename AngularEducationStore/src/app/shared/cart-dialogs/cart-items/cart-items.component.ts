import { Component, OnInit } from '@angular/core';
import { OrderService } from 'src/app/shared/services/order.service';
import { PrintingEditionsParametrs } from 'src/app/shared/constants/printing-editions-parametrs';
import { MatDialogRef } from '@angular/material';
import { PaymentService } from '../../services/payment.service';
import { Currency } from '../../enums/currency';
import { ConverterModel } from '../../models/ConverterModel';
import { PrintingEditionDetailsComponent } from 'src/app/printing-edition/printing-edition-details/printing-edition-details.component';
import { CartService } from '../../services/cart.service';
import { DataService } from '../../services/data.service';
import { OrderModelItem } from '../../models/order/OrderModelItem';
import { PaymentModel } from '../../models/payment/PaymentModel';

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

  displayedColumns = ['product', 'price', 'qty', 'amount', ' '];
  invalidError: any;

  constructor(public dialogRef: MatDialogRef<PrintingEditionDetailsComponent>, private cartService: CartService,
              private parametrs: PrintingEditionsParametrs, private orderService: OrderService, private dataService: DataService,
              private paymentService: PaymentService) {
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
  pay(order: OrderModelItem) {
    let payment = new PaymentModel();

    this.dialogRef.close();
    
    this.orderService.createOrder(this.dataService.getLocalStorage('userRole'), order).then((data) => {
      if (data.errors.length <= 0) {
        this.dataService.deleteItemLocalStorage('cartItems');
        this.cartService.cartSource.next([]);
        payment.orderId = data.items[0].id;
      }
    });
    
    this.paymentService.openStripeDialog(payment);
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
