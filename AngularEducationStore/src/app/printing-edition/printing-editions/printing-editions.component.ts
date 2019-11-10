import { Component, OnInit } from '@angular/core';
import { FilterPrintingEditionModel } from 'src/app/shared/models/filter/filter-printing-edition-model';
import { PrintingEditionService } from 'src/app/shared/services/printing-edition.service';
import { PrintingEditionsParametrs } from 'src/app/shared/constants/printing-editions-parametrs';
import { faBook } from '@fortawesome/free-solid-svg-icons';
import { PrintingEditionModel } from 'src/app/shared/models/printing-editions/PrintingEditionModel';
import { PageEvent } from '@angular/material';
import { PageSize } from 'src/app/shared/enums/page-size';
import { PrintingEditionType } from 'src/app/shared/enums/printing-edition-type';
import { Router } from '@angular/router';
import { DataService } from 'src/app/shared/services/data.service';

@Component({
  selector: 'app-printing-editions',
  templateUrl: './printing-editions.component.html',
  styleUrls: ['./printing-editions.component.scss']
})

export class PrintingEditionsComponent implements OnInit {

  printingEditionIcon = faBook;

  filterPrintingEditionModel = new FilterPrintingEditionModel();
  printingEditionModel = new PrintingEditionModel();

  printingEditionTypes = this.printingEditionParams.printingEditionTypes;
  currencyTypes = this.printingEditionParams.currencyTypes;
  sortStates = this.printingEditionParams.sortStates;
  pageSizes = this.printingEditionParams.pageSizes;
  gridLayout = this.printingEditionParams.gridFormationPrintingEditions;
  pageSize = PageSize.Six;
  page = 1;
  pageCols: number;
  pageRows: number;

  constructor(private printingEditionService: PrintingEditionService,
              private printingEditionParams: PrintingEditionsParametrs,
              private dataService: DataService) {
  }
  getIconStyle(pageSize: number) {
    return this.printingEditionService.getIconStyle(pageSize);
  }
  pageEvent(event: PageEvent) {
    this.printingEditionModel = new PrintingEditionModel();
    let page = event.pageIndex + 1;

    if (event.pageSize !== this.pageSize) {
      page = 1;
      this.pageSize = event.pageSize;
    }
    this.submit(page);
    this.getGridParams(event.pageSize);
  }
  submit(page: number = 1) {
    this.filterPrintingEditionModel.printingEditionTypes = [];
    for (let i = 0; i < this.printingEditionTypes.length; i++) {
      if (this.printingEditionTypes[i].checked === true) {

        this.filterPrintingEditionModel.printingEditionTypes.push(i + 1);
      }
    }
    this.filterPrintingEditionModel.pageSize = this.pageSize;
    this.filterPrintingEditionModel.page = page;
    this.page = page;
    this.printingEditionService.getPrintingEditions(this.filterPrintingEditionModel, this.dataService.getLocalStorage('userRole'))
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
    this.filterPrintingEditionModel.printingEditionTypes = [PrintingEditionType.Book];
    this.printingEditionService.getPrintingEditions(this.filterPrintingEditionModel, this.dataService.getLocalStorage('userRole'))
    .subscribe((data: PrintingEditionModel) => {
      this.printingEditionModel = data;
    });
    this.getGridParams(this.pageSize);
  }
}
