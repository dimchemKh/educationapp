import { Component, OnInit } from '@angular/core';
import { PrintingEditionsParameters } from 'src/app/shared/constants/printing-editions-parameters';
import { MatDialogRef } from '@angular/material';
import { Currency } from '../../../enums/currency';
import { ConverterModel, PaymentModel, OrderModelItem } from 'src/app/shared/models';
import { PrintingEditionDetailsComponent } from 'src/app/printing-edition/printing-edition-details/printing-edition-details.component';
import { OrderService, PaymentService, CartService, DataService } from 'src/app/shared/services';
import { ColumnsTitles } from 'src/app/shared/constants/columns-titles';
import { CurrencyPresentationModel } from 'src/app/shared/models/presentation/CurrencyPresentationModel';


@Component({
  selector: 'app-cart-items',
  templateUrl: './cart-items.component.html',
  styleUrls: ['./cart-items.component.scss']
})
export class CartItemsComponent implements OnInit {

  isEmptyCart: boolean;
  quantities = Array<number>();
  quantity: number;
  columnsCart: string[];

  converterModel: ConverterModel;
  orders: OrderModelItem;

  currencyPresentationModels: CurrencyPresentationModel[];

  constructor(public dialogRef: MatDialogRef<PrintingEditionDetailsComponent>,
    private cartService: CartService,
    private parametrs: PrintingEditionsParameters,
    private orderService: OrderService,
    private dataService: DataService,
    private paymentService: PaymentService,
    private columnsTitles: ColumnsTitles) {
    this.initQuantities();
    this.isEmptyCart = false;
    this.orders = new OrderModelItem();
    this.converterModel = new ConverterModel();
    this.currencyPresentationModels = this.parametrs.currencyPresentationModels;
    this.columnsCart = this.columnsTitles.columnsCart;
    this.converterModel.currencyTo = Currency.USD;
    this.converterModel.currencyFrom = Currency.USD;
  }

  initQuantities(): void {
    for (let i = 1; i < 10; i++) {
      this.quantities.push(i);
    }
  }

  ngOnInit(): void {
    this.getOrders();
    this.getOrdersAmount();
  }

  convertAmount(): number {
    this.cartService.convertToCart(this.converterModel).then((x) => {
      this.converterModel.price = x;
    });

    this.converterModel.currencyFrom = this.converterModel.currencyTo;

    return this.converterModel.price;
  }

  pay(order: OrderModelItem): void {
    let payment = new PaymentModel();

    this.dialogRef.close();

    this.orderService.createOrder(this.dataService.getLocalStorage('userRole'), order).then((data) => {
      if (data.errors.length <= 0) {
        this.dataService.deleteItemLocalStorage('cartItems');

        this.cartService.nextCartSource([]);

        payment.orderId = data.items[0].id;
      }
    });

    this.paymentService.openStripeDialog(payment);
  }

  getOrdersAmount(): number {
    this.converterModel.price = 0;

    this.orders.orderItems.forEach((x) => {
      this.converterModel.price += (x.count * x.price);
    });

    return this.converterModel.price;
  }

  getOrders(): void {
    let orders = this.cartService.getAllPurchases();

    if (!orders || orders.orderItems.length === 0) {
      this.isEmptyCart = true;
      return;
    }

    this.orders = orders;
  }

  removeOrderItem(printingEditionId: number): void {
    this.cartService.removeOrderItem(printingEditionId);

    this.getOrders();

    this.getOrdersAmount();
  }

  close(): void {
    this.dialogRef.close();
  }
}
