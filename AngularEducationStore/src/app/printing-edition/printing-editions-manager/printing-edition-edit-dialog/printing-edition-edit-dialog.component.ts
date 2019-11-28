import { Component, OnInit, Inject, ChangeDetectorRef, AfterContentChecked } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { PrintingEdiotionsManagerComponent } from 'src/app/printing-edition/printing-editions-manager/printing-editions-manager.component';
import { PrintingEditionsParameters } from 'src/app/shared/constants/printing-editions-parameters';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { AuthorService } from 'src/app/shared/services';
import { AuthorModel } from 'src/app/shared/models/authors/AuthorModel';
import { BehaviorSubject, Observable } from 'rxjs';
import { AuthorModelItem } from 'src/app/shared/models/authors/AuthorModelItem';
import { scan } from 'rxjs/operators';
import { FilterPrintingEditionModel } from 'src/app/shared/models/filter/filter-printing-edition-model';
import { FilterAuthorModel } from 'src/app/shared/models/filter/filter-author-model';
import { PageSize } from 'src/app/shared/enums/page-size';
import { ProductPresentationModel } from 'src/app/shared/models/presentation/ProductPresenatationModel';
import { CurrencyPresentationModel } from 'src/app/shared/models/presentation/CurrencyPresentationModel';

@Component({
  selector: 'app-printing-edition-edit-dialog',
  templateUrl: './printing-edition-edit-dialog.component.html',
  styleUrls: ['./printing-edition-edit-dialog.component.scss']
})

export class PrintingEditionEditDialogComponent implements OnInit, AfterContentChecked {

  isExistedData: boolean;
  offset: number;

  dataArray = new AuthorModel();

  authorsSubj: BehaviorSubject<AuthorModelItem[]>;
  authorsSubj$: Observable<AuthorModelItem[]>;

  filterModel: FilterPrintingEditionModel;

  authorFilter: FilterAuthorModel;
  authorsModel: AuthorModel;
  authorsId: Array<number>;

  productPresentationModels: ProductPresentationModel[];
  currencyPresentationModels: CurrencyPresentationModel[];

  form: FormGroup;

  constructor(public dialogRef: MatDialogRef<PrintingEdiotionsManagerComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, private printingEditionParams: PrintingEditionsParameters,
    private fb: FormBuilder, private authorService: AuthorService, private changeDetector: ChangeDetectorRef
    ) {
    this.isExistedData = false;
    this.authorsModel = new AuthorModel();
    this.authorFilter = new FilterAuthorModel();
    this.filterModel = new FilterPrintingEditionModel();
    this.authorsId = new Array<number>();
    this.authorsSubj = new BehaviorSubject<AuthorModelItem[]>([]);
    this.authorsSubj$ = this.authorsSubj.asObservable();
    this.productPresentationModels = this.printingEditionParams.productPresentationModels;
    this.currencyPresentationModels = this.printingEditionParams.currencyPresentationModels;
    this.offset = 0;

    this.authorsSubj$ = this.authorsSubj.asObservable().pipe(
      scan((acc, curr) => {
        return [...acc, ...curr];
      }, [])
    );
  }

  initFormGroup(): void {
    this.form = this.fb.group({
      title: new FormControl('', [Validators.required]),
      textarea: new FormControl('', [Validators.required]),
      types: new FormControl('', [Validators.required]),
      authors: new FormControl('', [Validators.required]),
      price: new FormControl('', [Validators.required]),
    });
  }

  get isValidForm(): boolean {
    return this.form.valid;
  }

  ngOnInit(): void {
    this.filterModel.currency = this.data.currency;

    if (this.data.authors) {
      this.data.authors.forEach(element => {
        this.authorsId.push(element.id);
      });

      this.authorsSubj.next(this.data.authors);
    }

    this.getNextAuthors();
  }

  close(): void {
    this.dialogRef.close(this.isExistedData);
  }

  getNextAuthors(): void {
    let arr = Array<AuthorModelItem>();

    this.authorFilter.pageSize = PageSize.Twelve;

    this.authorService.getAllAuthors(this.authorFilter).subscribe((data: AuthorModel) => {
      this.authorsModel.itemsCount = data.itemsCount;
      // tslint:disable-next-line: prefer-for-of
      for (let i = 0; i < data.items.length; i++) {
        if (!this.authorsId.includes(data.items[i].id)) {
          arr.push(data.items[i]);
        }
      }

      this.authorsSubj.next(arr);
    });

    this.authorFilter.page += 1;

    this.offset += PageSize.Twelve;
  }
  // tslint:disable-next-line: use-lifecycle-interface
  ngOnDestroy(): void {
    this.data.authors = new Array<AuthorModelItem>();

    this.authorsId.forEach(element => {
      this.data.authors.push({ id: element });
    });
  }

  ngAfterContentChecked(): void {
    this.changeDetector.detectChanges();
  }
}
