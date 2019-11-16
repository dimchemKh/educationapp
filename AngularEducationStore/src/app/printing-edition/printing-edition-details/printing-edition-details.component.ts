import { Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { PrintingEditionModelItem } from 'src/app/shared/models/printing-editions/PrintingEditionModelItem';
import { faBook } from '@fortawesome/free-solid-svg-icons';
import { ActivatedRoute } from '@angular/router';
import { PrintingEditionService } from 'src/app/shared/services/printing-edition.service';
import { faShoppingCart } from '@fortawesome/free-solid-svg-icons';
import { PrintingEditionsParametrs } from 'src/app/shared/constants/printing-editions-parametrs';
import { OrderItemModelItem } from 'src/app/shared/models/order-item/OrderItemModelItem';
import { OrderItemModel } from 'src/app/shared/models/order-item/OrderItemModel';
import { OrderService } from 'src/app/shared/services/order.service';
import { Currency } from 'src/app/shared/enums/currency';
import { Subscription } from 'rxjs';
import { ConverterModel } from 'src/app/shared/models/ConverterModel';
import { DataService } from 'src/app/shared/services/data.service';
import { CartService } from 'src/app/shared/services/cart.service';
import { MatDialog } from '@angular/material';
import { OrderModelItem } from 'src/app/shared/models/order/OrderModelItem';

@Component({
  selector: 'app-printing-edition-details',
  templateUrl: './printing-edition-details.component.html',
  styleUrls: ['./printing-edition-details.component.scss']
})
export class PrintingEditionDetailsComponent implements OnInit, OnDestroy {


  printingEdition = new PrintingEditionModelItem();
  printingEditionIcon = faBook;
  cartIcon = faShoppingCart;

  currencyTypes = this.prinringEditionParametrs.currencyTypes;

  cartSubscription: Subscription;
  isPurchase = false;

  quantity = 1;
  currency: number;

  quantities = Array<number>();

  constructor(private route: ActivatedRoute, private printingEditionService: PrintingEditionService,
              private prinringEditionParametrs: PrintingEditionsParametrs, private orderService: OrderService,
              private cartService: CartService) {
    for (let i = 1; i < 10; i++) {
      this.quantities.push(i);
    }

  }

  ngOnInit() {
    debugger
    this.cartSubscription = this.cartService.cartSource.subscribe((numbers) => {
      if (!this.cartService.checkTheSame(numbers, +this.route.snapshot.paramMap.get('id'))) {
        this.isPurchase = false;
      }
    });

    let data = history.state.data;
    data === undefined ? this.getDetails() : this.getDetails(data._currency);
  }
  convertPrice(currency: number) {
    this.quantity = 1;
    this.getDetails(currency);
  }
  getDetails(currency: number = 1) {
    debugger
    let printingEditionId = +this.route.snapshot.paramMap.get('id');
    this.currency = currency;
    this.printingEditionService.getPrintingEditionDetails(printingEditionId, currency).subscribe((data) => {
      this.printingEdition = data.items[0];
    });
  }
  async addPurchase(printingEdition: PrintingEditionModelItem) {
    debugger

    let orders = this.cartService.getAllPurchases();

    let cartSource = this.cartService.cartSource.value;
    cartSource.push(printingEdition.id);

    this.cartService.cartSource.next(cartSource);

    if (cartSource && this.cartService.checkTheSame(cartSource, printingEdition.id)) {
      this.isPurchase = true;
    }

    let localPrintingEdition = new OrderItemModelItem();
    debugger
    if (this.currency !== Currency.USD) {
      let converterModel = new ConverterModel();
      converterModel.currencyFrom = this.currency;

      converterModel.price = printingEdition.price;

      await this.cartService.convertToCart(converterModel).then((x) => {
        printingEdition.price = x;
      });
    }

    localPrintingEdition.printingEditionId = printingEdition.id;
    localPrintingEdition.title = printingEdition.title;
    localPrintingEdition.price = printingEdition.price;
    localPrintingEdition.count = this.quantity;
    localPrintingEdition.currency = Currency.USD;

    if (!orders) {
      orders = new OrderModelItem();
      orders.orderItems.push(localPrintingEdition);
      await this.cartService.addOrder(orders);
      this.isPurchase = true;
      return;
    }

    orders.orderItems.push(localPrintingEdition);
    await this.cartService.addOrder(orders);
    this.isPurchase = true;
  }
  ngOnDestroy() {
    this.cartSubscription.unsubscribe();
  }
}
