import { Component, OnInit } from '@angular/core';
import { Currency } from '../../enums/currency';

@Component({
  selector: 'app-get-printing-edition',
  templateUrl: './get-printing-edition.component.html',
  styleUrls: ['./get-printing-edition.component.scss']
})
export class GetPrintingEditionComponent implements OnInit {

  constructor() { }
  minValue = 0;
  maxValue = 10000;
  searchString: string;
  currency: Currency
  selected = Currency.USD;
  ngOnInit() {
    
  }
  
}
