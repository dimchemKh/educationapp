import { Component, OnInit } from '@angular/core';
import { PrintingEditionModelItem } from 'src/app/shared/models/printing-editions/PrintingEditionModelItem';
import { faBook } from '@fortawesome/free-solid-svg-icons';
import { from } from 'rxjs';

@Component({
  selector: 'app-printing-edition-details',
  templateUrl: './printing-edition-details.component.html',
  styleUrls: ['./printing-edition-details.component.scss']
})
export class PrintingEditionDetailsComponent implements OnInit {

  printingEdition: PrintingEditionModelItem;
  printingEditionIcon = faBook;

  constructor() { }

  ngOnInit() {
    this.printingEdition = history.state.data;
  }
  getCard() {
    console.log('card');
  }
}
