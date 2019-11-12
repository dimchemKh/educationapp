import { Component, OnInit, AfterViewInit } from '@angular/core';
import { PrintingEditionModelItem } from 'src/app/shared/models/printing-editions/PrintingEditionModelItem';
import { faBook } from '@fortawesome/free-solid-svg-icons';
import { ActivatedRoute } from '@angular/router';
import { PrintingEditionService } from 'src/app/shared/services/printing-edition.service';
import { faShoppingCart } from '@fortawesome/free-solid-svg-icons';
import { PrintingEditionsParametrs } from 'src/app/shared/constants/printing-editions-parametrs';
import { DataService } from 'src/app/shared/services/data.service';
import { OrderItemModelItem } from 'src/app/shared/models/order-item/OrderItemModelItem';

@Component({
  selector: 'app-printing-edition-details',
  templateUrl: './printing-edition-details.component.html',
  styleUrls: ['./printing-edition-details.component.scss']
})
export class PrintingEditionDetailsComponent implements OnInit {

  printingEdition = new PrintingEditionModelItem();
  printingEditionIcon = faBook;
  cartIcon = faShoppingCart;

  currencyTypes = this.prinringEditionParametrs.currencyTypes;

  error = false;
  quantity = 1;
  currency = 1;
  quantities = Array<number>();

  constructor(private route: ActivatedRoute, private printingEditionService: PrintingEditionService,
              private prinringEditionParametrs: PrintingEditionsParametrs, private dataService: DataService) {
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
    let localPrintingEdition =  {
      printingEditionId: printingEdition.id,
      title: printingEdition.title,
      count: this.quantity,
      price: printingEdition.price,
      currency: this.currency
    };
    let count = this.dataService.getCount();
    let orderItem: OrderItemModelItem;

    if (count === 0) {
      this.dataService.setLocalStorage('cartItem' + count, JSON.stringify(localPrintingEdition));
    }
    if (count > 0) {
      let id = count - 1;
      orderItem = JSON.parse(this.dataService.getLocalStorage('cartItem' + id));
      orderItem.printingEditionId === printingEdition.id
      ? this.error = true
      : this.dataService.setLocalStorage('cartItem' + count, JSON.stringify(localPrintingEdition));
    }
  }
}
