import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { PrintingEditionService } from 'src/app/shared/services/printing-edition.service';
import { FilterPrintingEditionModel } from 'src/app/shared/models/filter/filter-printing-edition-model';
import { PrintingEditionModel } from 'src/app/shared/models/printing-editions/PrintingEditionModel';
import { faHighlighter, IconDefinition } from '@fortawesome/free-solid-svg-icons';
import { faTimes } from '@fortawesome/free-solid-svg-icons';
import { faPlusCircle } from '@fortawesome/free-solid-svg-icons';
import { PrintingEditionsParameters } from 'src/app/shared/constants/printing-editions-parameters';
import { PrintingEditionType } from 'src/app/shared/enums/printing-edition-type';
import { MatSort, PageEvent, MatDialog, MatSnackBar, MatSelectChange } from '@angular/material';
import { PrintingEditionEditDialogComponent } from './printing-edition-edit-dialog/printing-edition-edit-dialog.component';
import { PrintingEditionModelItem } from 'src/app/shared/models/printing-editions/PrintingEditionModelItem';
import { RemoveDialogComponent } from 'src/app/shared/components/remove-dialog/remove-dialog.component';
import { RemoveModel } from 'src/app/shared/models/dialogs/RemoveModel';
import { DataService } from 'src/app/shared/services/data.service';
import { ColumnsTitles } from 'src/app/shared/constants/columns-titles';
import { ProductPresentationModel } from 'src/app/shared/models/presentation/ProductPresenatationModel';
import { PageSize } from 'src/app/shared/enums';
import { SortTypesPresentationModel } from 'src/app/shared/models/presentation/SortTypesPresentationModel';
import { SortStatesPresentationModel } from 'src/app/shared/models/presentation/SortStatesPresentationModel';

@Component({
  selector: 'app-printing-editions-manager',
  templateUrl: './printing-editions-manager.component.html',
  styleUrls: ['./printing-editions-manager.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class PrintingEdiotionsManagerComponent implements OnInit {

  editIcon: IconDefinition;
  closeIcon: IconDefinition;
  createIcon: IconDefinition;

  filterModel: FilterPrintingEditionModel;
  printingEditionModel: PrintingEditionModel;

  columnsPrintingEditions: string[];

  sortStateModels: SortStatesPresentationModel[];
  sortProductModels: SortTypesPresentationModel[];
  pageSizes: PageSize[];
  productPresentationModels: Array<ProductPresentationModel>;

  isRequire: number;


  constructor(private dialog: MatDialog, private printingEditionService: PrintingEditionService,
    private printingEditionParams: PrintingEditionsParameters, private descriptionBar: MatSnackBar,
    private dataService: DataService, private columnsTitles: ColumnsTitles
    ) {
    this.editIcon = faHighlighter;
    this.closeIcon = faTimes;
    this.createIcon = faPlusCircle;
    this.filterModel = new FilterPrintingEditionModel();
    this.printingEditionModel = new PrintingEditionModel();
    this.columnsPrintingEditions = this.columnsTitles.columnsPrintingEditions;
    this.filterModel.PrintingEditionTypes = [PrintingEditionType.Book, PrintingEditionType.Magazine, PrintingEditionType.Newspaper];
    this.productPresentationModels = this.printingEditionParams.productPresentationModels;
    this.sortStateModels = this.printingEditionParams.sortStateModels;
    this.sortProductModels = this.printingEditionParams.sortProductModels;
    this.pageSizes = this.printingEditionParams.pageSizes;
  }

  ngOnInit(): void {
    this.printingEditionService.getPrintingEditions(this.dataService.getLocalStorage('userRole'), this.filterModel)
    .subscribe((data: PrintingEditionModel) => {
      this.printingEditionModel = data;
    });
  }

  sortData(event: MatSort): void {

    let sortState = this.sortStateModels.find(x => x.direction.toLowerCase() === event.direction.toLowerCase());

    this.filterModel.sortState = sortState.value;

    let sortTypes = this.sortProductModels.find(x => x.name.toLowerCase() === event.active.toLowerCase());

    this.filterModel.sortType = sortTypes.value;

    this.submit();
  }

  getType(id: number): string {
    return PrintingEditionType[id];
  }

  submit(page: number = 1): void {
    debugger
    this.filterModel.page = page;

    this.printingEditionService.getPrintingEditions(this.dataService.getLocalStorage('userRole'), this.filterModel)
    .subscribe((data) => {
      this.printingEditionModel = data;
    });
  }

  pageEvent(event: PageEvent): void {
    let page = event.pageIndex + 1;

    if (event.pageSize !== this.filterModel.pageSize) {
      page = 1;
      this.filterModel.pageSize = event.pageSize;
    }

    this.submit(page);
  }

  closedTypeSelect(event: boolean): void {
    if (!event) {
      this.submit();
    }
  }
  
  changeTypeSelect(event: MatSelectChange): void {
    if (event.value.length === 1) {
      this.isRequire = event.value[0];
    }

    if (event.value.length === 0) {
      event.source.value = [this.isRequire];
      this.filterModel.PrintingEditionTypes = [this.isRequire];
    }
  }

  openDescription(dataDescription: string): void {
    this.descriptionBar.open(dataDescription, 'close', {
      data: dataDescription,
      horizontalPosition: 'center',
      verticalPosition: 'top'
    });
  }

  openCreateDialog(): void {
    let dialog = this.dialog.open(PrintingEditionEditDialogComponent, {
      data: {
        dialogTitle: 'Create new product'
      }
    });

    dialog.afterClosed().subscribe((result) => {
      if (result) {
        this.printingEditionService.createPrintingEdition(result).subscribe(() => {
          this.submit(this.filterModel.page);
        });
      }
    });
  }
  
  openEditDialog(element: PrintingEditionModelItem): void {
    let dialog = this.dialog.open(PrintingEditionEditDialogComponent, {
      data: {
        dialogTitle: 'Change product',
        id: element.id,
        title: element.title,
        description: element.description,
        printingEditionType: element.printingEditionType,
        authors: element.authors,
        currency: element.currency,
        price: element.price
      }
    });

    dialog.afterClosed().subscribe((result) => {
      if (result) {
        this.printingEditionService.updatePrintingEdition(result).subscribe(() => {
          this.submit(this.filterModel.page);
        });
      }
    });
  }

  openRemoveDialog(element): void {
    let dialog = this.dialog.open(RemoveDialogComponent, {
      data: {
        message: 'Do you wan`t to delete: ' + element.title + '?',
        closeTitle: 'Close',
        removeTitle: 'Remove',
        id: element.id
      }
    });

    dialog.afterClosed().subscribe((result: RemoveModel) => {
      if (result) {
        this.printingEditionService.removePrintingEdition(result.id).subscribe(() => {
          this.submit(this.filterModel.page);
        });
      }
    });
  }

  close(): void {
    this.descriptionBar.dismiss();
  }
}
