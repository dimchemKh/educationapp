import { Component, OnInit } from '@angular/core';
import { FilterPrintingEditionModel } from 'src/app/shared/models/filter/filter-printing-edition-model';
import { PrintingEditionService } from 'src/app/shared/services/printing-edition.service';
import { PrintingEditionsParametrs } from 'src/app/shared/constants/printing-editions-parametrs';
import { faBook } from '@fortawesome/free-solid-svg-icons';
import { PrintingEditionModel } from 'src/app/shared/models/printing-editions/PrintingEditionModel';
import { PageEvent } from '@angular/material';
import { PrintingEditionType } from 'src/app/shared/enums/printing-edition-type';
import { DataService } from 'src/app/shared/services/data.service';
import { PrintingEditionModelItem } from 'src/app/shared/models/printing-editions/PrintingEditionModelItem';
import { Router } from '@angular/router';

@Component({
  selector: 'app-printing-editions',
  templateUrl: './printing-editions.component.html',
  styleUrls: ['./printing-editions.component.scss']
})

export class PrintingEditionsComponent implements OnInit {

  printingEditionIcon = faBook;

  filterModel = new FilterPrintingEditionModel();
  printingEditionModel = new PrintingEditionModel();

  printingEditionTypes = this.printingEditionParams.printingEditionTypes;
  currencyTypes = this.printingEditionParams.currencyTypes;
  sortStates = this.printingEditionParams.sortStates;
  pageSizes = this.printingEditionParams.pageSizes;
  gridLayout = this.printingEditionParams.gridFormationPrintingEditions;
  
  pageCols: number;
  pageRows: number;

  constructor(private printingEditionService: PrintingEditionService,
              private printingEditionParams: PrintingEditionsParametrs,
              private dataService: DataService, private router: Router) {
  }
  getIconStyle(pageSize: number) {
    return this.printingEditionService.getIconStyle(pageSize);
  }
  pageEvent(event: PageEvent) {
    let page = event.pageIndex + 1;
    this.printingEditionModel.items = new Array<PrintingEditionModelItem>();

    if (event.pageSize !== this.filterModel.pageSize) {
      page = 1;
      this.filterModel.pageSize = event.pageSize;
      this.getGridParams(event.pageSize);
    }

    this.submit(page);
  }
  submit(page: number = 1) {
    this.filterModel.printingEditionTypes = [];
    for (let i = 0; i < this.printingEditionTypes.length; i++) {
      if (this.printingEditionTypes[i].checked === true) {

        this.filterModel.printingEditionTypes.push(i + 1);
      }
    }
    this.filterModel.page = page;

    this.printingEditionService.getPrintingEditions(this.dataService.getLocalStorage('userRole'), this.filterModel)
    .subscribe((data: PrintingEditionModel) => {
      this.printingEditionModel = data;
    });
  }
  getGridParams(pageSize: number) {
    this.gridLayout.forEach(x => {
      if (x.value === pageSize) {
        this.pageCols = x.cols;
        this.pageRows = x.rowsHeight;
      }
    });
  }
  onChange(event, i: number) {
    // tslint:disable-next-line: no-unused-expression
    let num = -1;
    this.printingEditionTypes.forEach(x => {
      if (x.checked) {
        num = num + 1;
      }
    });
    if (num < 0) {
      event.source.toggle();
      this.printingEditionParams.printingEditionTypes[i].checked = true;
    }
  }
  ngOnInit() {
    this.filterModel.printingEditionTypes = [PrintingEditionType.Book];
    this.printingEditionService.getPrintingEditions(this.dataService.getLocalStorage('userRole'), this.filterModel)
    .subscribe((data: PrintingEditionModel) => {
      this.printingEditionModel = data;
    });
    this.getGridParams(this.filterModel.pageSize);
  }
  getDetails(id: number, currency: number) {
    this.router.navigate(['/details/', id], {state: {data: { _id: id, _currency: currency}}});
  }
}
