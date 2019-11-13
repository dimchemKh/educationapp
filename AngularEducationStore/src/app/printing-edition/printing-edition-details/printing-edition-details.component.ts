import { Component, OnInit, AfterViewInit } from '@angular/core';
import { PrintingEditionModelItem } from 'src/app/shared/models/printing-editions/PrintingEditionModelItem';
import { faBook } from '@fortawesome/free-solid-svg-icons';
import { ActivatedRoute } from '@angular/router';
import { PrintingEditionService } from 'src/app/shared/services/printing-edition.service';
import { faShoppingCart } from '@fortawesome/free-solid-svg-icons';
import { PrintingEditionsParametrs } from 'src/app/shared/constants/printing-editions-parametrs';
import { OrderItemModelItem } from 'src/app/shared/models/order-item/OrderItemModelItem';
import { OrderItemModel } from 'src/app/shared/models/order-item/OrderItemModel';
import { OrderService } from 'src/app/shared/services/order.service';

@Component({
  selector: 'app-printing-edition-details',
  templateUrl: './printing-edition-details.component.html',
  styleUrls: ['./printing-edition-details.component.scss']
})
export class PrintingEditionDetailsComponent implements OnInit {

  orders = new OrderItemModel();
  printingEdition = new PrintingEditionModelItem();
  printingEditionIcon = faBook;
  cartIcon = faShoppingCart;

  currencyTypes = this.prinringEditionParametrs.currencyTypes;

  isPurchase = false;

  quantity = 1;
  currency = 1;
  quantities = Array<number>();

  constructor(private route: ActivatedRoute, private printingEditionService: PrintingEditionService,
    private prinringEditionParametrs: PrintingEditionsParametrs, private orderService: OrderService) {
    for (let i = 1; i < 10; i++) {
      this.quantities.push(i);
    }

  }

  ngOnInit() {
    let data = history.state.data;
    data === undefined ? this.getDetails() : this.getDetails(data._currency);
  }
  convertPrice(currency: number) {
    this.quantity = 1;
    this.getDetails(currency);
  }
  getDetails(currency: number = 1) {
    let printingEditionId = +this.route.snapshot.paramMap.get('id');
    this.printingEditionService.getPrintingEditionDetails(printingEditionId, currency).subscribe((data) => {
      this.printingEdition = data.items[0];
    });
  }
  addPurchase(printingEdition: PrintingEditionModelItem) {

    let orders = this.orderService.getAllPurchases();
    if (orders && this.orderService.checkTheSame(orders, printingEdition.id)) {
      this.isPurchase = true;
      return;
    }
    let localPrintingEdition = new OrderItemModelItem();

    localPrintingEdition.printingEditionId = printingEdition.id;
    localPrintingEdition.title = printingEdition.title;
    localPrintingEdition.price = printingEdition.price;
    localPrintingEdition.count = this.quantity;
    localPrintingEdition.currency = this.currency;

    if (!orders) {
      this.orders.items.push(localPrintingEdition);
      this.orderService.addOrder(this.orders);
      this.isPurchase = true;
      return;
    }

    orders.items.push(localPrintingEdition);
    this.orderService.addOrder(orders);
    this.isPurchase = true;
  }
}
