import { Component, OnInit } from '@angular/core';
import { FilterPrintingEditionModel } from 'src/app/shared/models/filter/filter-printing-edition-model';
import { PrintingEditionService } from 'src/app/shared/services/printing-edition.service';
import { PrintingEditionsParameters } from 'src/app/shared/constants/printing-editions-parameters';
import { faBook, IconDefinition } from '@fortawesome/free-solid-svg-icons';
import { PrintingEditionModel } from 'src/app/shared/models/printing-editions/PrintingEditionModel';
import { PageEvent } from '@angular/material';
import { PrintingEditionType } from 'src/app/shared/enums/printing-edition-type';
import { DataService } from 'src/app/shared/services/data.service';
import { PrintingEditionModelItem } from 'src/app/shared/models/printing-editions/PrintingEditionModelItem';
import { Router } from '@angular/router';
import { ProductPresentationModel } from 'src/app/shared/models/presentation/ProductPresenatationModel';
import { CurrencyPresentationModel } from 'src/app/shared/models/presentation/CurrencyPresentationModel';
import { SortStatesPresentationModel } from 'src/app/shared/models/presentation/SortStatesPresentationModel';
import { PageSize } from 'src/app/shared/enums';
import { GridFormatPresentationModel } from 'src/app/shared/models/presentation/GridFormatPresentationModel';

@Component({
  selector: 'app-printing-editions',
  templateUrl: './printing-editions.component.html',
  styleUrls: ['./printing-editions.component.scss']
})

export class PrintingEditionsComponent implements OnInit {

  printingEditionIcon: IconDefinition;
  filterModel: FilterPrintingEditionModel;
  printingEditionModel: PrintingEditionModel;
  productPresentationModels: Array<ProductPresentationModel>;
  currencyPresentationModels: Array<CurrencyPresentationModel>;
  sortStateModels: Array<SortStatesPresentationModel>;
  pageSizes: PageSize[];
  gridLayout: GridFormatPresentationModel[];
  
  pageCols: number;
  pageRows: number;

  constructor(private printingEditionService: PrintingEditionService,
    private printingEditionParams: PrintingEditionsParameters,
    private dataService: DataService,
    private router: Router
    ) {
    this.pageSizes = this.printingEditionParams.pageSizes;
    this.currencyPresentationModels = this.printingEditionParams.currencyPresentationModels;
    this.productPresentationModels = this.printingEditionParams.productPresentationModels;
    this.sortStateModels = this.printingEditionParams.sortStateModels;
    this.printingEditionIcon = faBook;
    this.filterModel = new FilterPrintingEditionModel();
    this.printingEditionModel = new PrintingEditionModel();
    this.gridLayout = this.printingEditionParams.gridFormationPrintingEditions;
    this.filterModel.PrintingEditionTypes = [
      PrintingEditionType.Book,
      PrintingEditionType.Magazine,
      PrintingEditionType.Newspaper
    ];
  }

  getIconStyle(pageSize: number): any {
    return this.printingEditionService.getIconStyle(pageSize);
  }

  pageEvent(event: PageEvent): void {
    let page = event.pageIndex + 1;

    this.printingEditionModel.items = new Array<PrintingEditionModelItem>();

    if (event.pageSize !== this.filterModel.pageSize) {
      page = 1;

      this.filterModel.pageSize = event.pageSize;

      this.getGridParams(event.pageSize);
    }

    this.submit(page);
  }
  submit(page: number = 1): void {
    this.filterModel.PrintingEditionTypes = [];

    for (let i = 0; i < this.productPresentationModels.length; i++) {
      if (this.productPresentationModels[i].checked === true) {
        this.filterModel.PrintingEditionTypes.push(i + 1);
      }
    }

    this.filterModel.page = page;

    this.printingEditionService.getPrintingEditions(this.dataService.getLocalStorage('userRole'), this.filterModel)
    .subscribe((data: PrintingEditionModel) => {
      this.printingEditionModel = data;
    });
  }

  private getGridParams(pageSize: number): void {
    this.gridLayout.forEach(x => {
      if (x.value === pageSize) {
        this.pageCols = x.cols;
        this.pageRows = x.rowsHeight;
      }
    });
  }

  onChange(event, i: number): void {
    let num = -1;

    this.productPresentationModels.forEach(x => {
      if (x.checked) {
        num++;
      }
    });

    if (num < 0) {
      event.source.toggle();
      this.printingEditionParams.productPresentationModels[i].checked = true;
    }
  }

  ngOnInit(): void {
    this.printingEditionService.getPrintingEditions(this.dataService.getLocalStorage('userRole'), this.filterModel)
    .subscribe((data: PrintingEditionModel) => {
      this.printingEditionModel = data;
    });

    this.getGridParams(this.filterModel.pageSize);
  }

  getDetails(id: number, currency: number): void {
    this.router.navigate(['/details/', id], {state: {data: { _currency: currency}}});
  }
}
