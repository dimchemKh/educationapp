import { Component, OnInit } from '@angular/core';
import { PrintingEditionModelItem } from 'src/app/shared/models/printing-editions/PrintingEditionModelItem';
import { faBook } from '@fortawesome/free-solid-svg-icons';
import { ActivatedRoute } from '@angular/router';
import { PrintingEditionService } from 'src/app/shared/services/printing-edition.service';
import { switchMap } from 'rxjs/operators';
import { faShoppingCart } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-printing-edition-details',
  templateUrl: './printing-edition-details.component.html',
  styleUrls: ['./printing-edition-details.component.scss']
})
export class PrintingEditionDetailsComponent implements OnInit {

  printingEdition = new PrintingEditionModelItem();
  printingEditionIcon = faBook;
  cartIcon = faShoppingCart;
  
  quantity = '1';

  constructor(private route: ActivatedRoute, private printingEditionService: PrintingEditionService) {

   }

  ngOnInit() {
    let data = history.state.data;
    this.getDetails();
  }
  getDetails() {
    let printingEditionId = +this.route.snapshot.paramMap.get('id');
    this.printingEditionService.getPrintingEditionDetails(printingEditionId).subscribe((data) => {
      this.printingEdition = data.items[0];
    });
  }
}
