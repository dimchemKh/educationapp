import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { PrintingEdiotionsManagerComponent } from 'src/app/printing-edition/printing-editions-manager/printing-editions-manager.component';
import { PrintingEditionsParametrs } from 'src/app/shared/constants/printing-editions-parametrs';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { PrintingEditionService } from 'src/app/shared/services/printing-edition.service';
import { Currency } from 'src/app/shared/enums/currency';
import { FilterAuthorModel } from 'src/app/shared/models/filter/filter-author-model';
import { AuthorService } from 'src/app/shared/services/author.service';
import { PageSize } from 'src/app/shared/enums/page-size';
import { AuthorModel } from 'src/app/shared/models/authors/AuthorModel';
import { BehaviorSubject, Observable } from 'rxjs';
import { AuthorModule } from 'src/app/author/author.module';
import { MatSelect } from '@angular/material';
import { AuthorModelItem } from 'src/app/shared/models/authors/AuthorModelItem';

@Component({
  selector: 'app-printing-edition-edit-dialog',
  templateUrl: './printing-edition-edit-dialog.component.html',
  styleUrls: ['./printing-edition-edit-dialog.component.scss']
})



export class PrintingEditionEditDialogComponent implements OnInit {

  total: number;
  authors: string[];

  constructor(public dialogRef: MatDialogRef<PrintingEdiotionsManagerComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, private printingEditionParams: PrintingEditionsParametrs,
    private fb: FormBuilder, private printingEditionService: PrintingEditionService, private authorService: AuthorService) {

      this.total = this.authorsModel.itemsCount;
    }

  limit = 12;
  offset = 0;
  options = new BehaviorSubject<string[]>([]);
  options$: Observable<string[]>;

  isChangeProduct = false;
  selectedCurrency = Currency.USD;
  filterModel = new FilterAuthorModel();
  authorsModel = new AuthorModel();

  printingEditionTypes = this.printingEditionParams.printingEditionTypes;
  currencyTypes = this.printingEditionParams.currencyTypes;
  form: FormGroup;

  ngOnInit() {
    this.filterModel.pageSize = PageSize.Twelve;
    this.isChangeProduct = this.data.isChangeProduct;
    this.form = this.fb.group({
      price: new FormControl('', [Validators.required])
    });
    debugger
    this.authorService.getAllAuthors(this.filterModel).subscribe((data) => {
      this.authorsModel = data;
    });
    this.getNextBatch(this.filterModel);
  }


  close() {
    this.dialogRef.close();
  }
  submit() {
    if (!this.isChangeProduct) {
      this.printingEditionService.createPrintingEdition(this.data);
    }
  }
  getNextBatch(filterModel: FilterAuthorModel) {
    debugger
    filterModel.page += 1;
    // tslint:disable-next-line: new-parens
    let temp = Array<AuthorModelItem>();

    this.authorService.getAllAuthors(filterModel).subscribe((data) => {
      temp = data.items;
    });
    temp.forEach(element => {
      this.authorsModel.items.push(element);
    });
    this.offset += this.limit;
  }
}
