import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { PrintingEditionService } from 'src/app/shared/services/printing-edition.service';
import { FilterPrintingEditionModel } from 'src/app/shared/models/filter/filter-printing-edition-model';
import { PrintingEditionModel } from 'src/app/shared/models/printing-editions/PrintingEditionModel';
import { faHighlighter } from '@fortawesome/free-solid-svg-icons';
import { faTimes } from '@fortawesome/free-solid-svg-icons';
import { faPlusCircle } from '@fortawesome/free-solid-svg-icons';
import { PrintingEditionsParametrs } from 'src/app/shared/constants/printing-editions-parametrs';
import { PrintingEditionType } from 'src/app/shared/enums/printing-edition-type';
import { MatSort, PageEvent, MatDialog, MatSnackBar, MatSelectChange } from '@angular/material';
import { PrintingEditionEditDialogComponent } from './printing-edition-edit-dialog/printing-edition-edit-dialog.component';
import { PrintingEditionModelItem } from 'src/app/shared/models/printing-editions/PrintingEditionModelItem';
import { RemoveDialogComponent } from 'src/app/shared/components/remove-dialog/remove-dialog.component';
import { RemoveModel } from 'src/app/shared/models/RemoveModel';
import { DataService } from 'src/app/shared/services/data.service';

@Component({
  selector: 'app-printing-editions-manager',
  templateUrl: './printing-editions-manager.component.html',
  styleUrls: ['./printing-editions-manager.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class PrintingEdiotionsManagerComponent implements OnInit {

  editIcon = faHighlighter;
  closeIcon = faTimes;
  createIcon = faPlusCircle;

  filterModel = new FilterPrintingEditionModel();

  displayedColumns: string[] = ['id', 'title', 'description', 'category', 'author', 'price', ' '];

  sortStates = this.printingEditionParams.sortStates;
  sortTypes = this.printingEditionParams.sortTypes;
  pageSizes = this.printingEditionParams.pageSizes;
  printingEditionTypes = this.printingEditionParams.printingEditionTypes;

  isRequire: number;

  printingEditionModel = new PrintingEditionModel();

  constructor(private dialog: MatDialog, private printingEditionService: PrintingEditionService,
              private printingEditionParams: PrintingEditionsParametrs, private descriptionBar: MatSnackBar,
              private dataService: DataService) {
  }

  ngOnInit() {
    this.filterModel.printingEditionTypes = [PrintingEditionType.Book, PrintingEditionType.Magazine, PrintingEditionType.Newspaper];
    this.printingEditionService.getPrintingEditions(this.dataService.getLocalStorage('userRole'), this.filterModel)
    .subscribe((data: PrintingEditionModel) => {
      this.printingEditionModel = data;
    });
  }
  sortData(event: MatSort) {

    let sortState = this.sortStates.find(x => x.direction.toLowerCase() === event.direction.toLowerCase());
    this.filterModel.sortState = sortState.value;

    let sortTypes = this.sortTypes.find(x => x.name.toLowerCase() === event.active.toLowerCase());
    this.filterModel.sortType = sortTypes.value;

    this.submit();
  }
  getType(id: number) {
    return PrintingEditionType[id];
  }
  submit(page: number = 1) {
    this.filterModel.page = page;
    this.printingEditionService.getPrintingEditions(this.dataService.getLocalStorage('userRole'), this.filterModel)
    .subscribe((data) => {
      this.printingEditionModel = data;
    });
  }
  pageEvent(event: PageEvent) {
    let page = event.pageIndex + 1;

    if (event.pageSize !== this.filterModel.pageSize) {
      page = 1;
      this.filterModel.pageSize = event.pageSize;
    }
    this.submit(page);
  }
  closedTypeSelect(event: boolean) {
    if (!event) {
      this.submit();
    }
  }
  changeTypeSelect(event: MatSelectChange) {
    if (event.value.length === 1) {
      this.isRequire = event.value[0];
    }
    if (event.value.length === 0) {
      event.source.value = [this.isRequire];
      this.filterModel.printingEditionTypes = [this.isRequire];
    }
  }
  openDescription(dataDescription: string) {
    this.descriptionBar.open(dataDescription, 'close', {
      data: dataDescription,
      horizontalPosition: 'center',
      verticalPosition: 'top'
    });
  }

  openCreateDialog() {
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
  
  openEditDialog(element: PrintingEditionModelItem) {
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
  openRemoveDialog(element) {
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
  close() {
    this.descriptionBar.dismiss();
  }
}
