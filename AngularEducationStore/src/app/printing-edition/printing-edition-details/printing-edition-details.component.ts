import { Component, OnInit, OnDestroy } from '@angular/core';
import { PrintingEditionModelItem } from 'src/app/shared/models/printing-editions/PrintingEditionModelItem';
import { faBook, IconDefinition } from '@fortawesome/free-solid-svg-icons';
import { ActivatedRoute } from '@angular/router';
import { PrintingEditionService } from 'src/app/shared/services/printing-edition.service';
import { faShoppingCart } from '@fortawesome/free-solid-svg-icons';
import { PrintingEditionsParameters } from 'src/app/shared/constants/printing-editions-parameters';
import { OrderItemModelItem } from 'src/app/shared/models/order-item/OrderItemModelItem';
import { Currency } from 'src/app/shared/enums/currency';
import { Subscription } from 'rxjs';
import { ConverterModel } from 'src/app/shared/models/converter/ConverterModel';
import { CartService } from 'src/app/shared/services/cart.service';
import { OrderModelItem } from 'src/app/shared/models/order/OrderModelItem';
import { CurrencyPresentationModel } from 'src/app/shared/models/presentation/CurrencyPresentationModel';

@Component({
  selector: 'app-printing-edition-details',
  templateUrl: './printing-edition-details.component.html',
  styleUrls: ['./printing-edition-details.component.scss']
})
export class PrintingEditionDetailsComponent implements OnInit, OnDestroy {

  printingEdition: PrintingEditionModelItem;
  printingEditionIcon: IconDefinition;
  cartIcon: IconDefinition;

  currencyPresentationModels: Array<CurrencyPresentationModel>;

  cartSubscription: Subscription;
  isPurchase: boolean;

  quantity: number;
  currency: number;

  quantities = Array<number>();

  constructor(private route: ActivatedRoute,
    private printingEditionService: PrintingEditionService,
    private prinringEditionParametrs: PrintingEditionsParameters,
    private cartService: CartService
    ) {
    for (let i = 1; i < 10; i++) {
      this.quantities.push(i);
    }
    this.printingEditionIcon = faBook;
    this.cartIcon = faShoppingCart;
    this.isPurchase = false;
    this.printingEdition = new PrintingEditionModelItem();
    this.quantity = 1;
    this.currencyPresentationModels = this.prinringEditionParametrs.currencyPresentationModels;
  }

  ngOnInit(): void {
    this.cartSubscription = this.cartService.getCartSource().subscribe((numbers) => {
      if (!this.cartService.checkTheSame(numbers, +this.route.snapshot.paramMap.get('id'))) {
        this.isPurchase = false;
      }
    });

    let data = history.state.data;
    data === undefined ? this.getDetails() : this.getDetails(data._currency);
  }

  convertPrice(currency: number): void {
    this.quantity = 1;
    this.getDetails(currency);
  }

  getDetails(currency: number = 1): void {
    let printingEditionId = +this.route.snapshot.paramMap.get('id');
    
    this.currency = currency;

    this.printingEditionService.getPrintingEditionDetails(printingEditionId, currency).subscribe((data) => {
      this.printingEdition = data.items[0];
    });
  }

  async addPurchase(printingEdition: PrintingEditionModelItem): Promise<void> {

    let orders = this.cartService.getAllPurchases();

    let cartSource = this.cartService.getAllValuesSource();
    cartSource.push(printingEdition.id);

    this.cartService.nextCartSource(cartSource);

    if (cartSource && this.cartService.checkTheSame(cartSource, printingEdition.id)) {
      this.isPurchase = true;
    }

    let localPrintingEdition = new OrderItemModelItem();

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
  ngOnDestroy(): void {
    this.cartSubscription.unsubscribe();
  }
}
