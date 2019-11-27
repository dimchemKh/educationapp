import { Component, OnInit, Inject, ChangeDetectorRef, AfterContentChecked } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { PrintingEdiotionsManagerComponent } from 'src/app/printing-edition/printing-editions-manager/printing-editions-manager.component';
import { PrintingEditionsParameters } from 'src/app/shared/constants/printing-editions-parameters';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { AuthorService } from 'src/app/shared/services/author.service';
import { AuthorModel } from 'src/app/shared/models/authors/AuthorModel';
import { BehaviorSubject, Observable } from 'rxjs';
import { AuthorModelItem } from 'src/app/shared/models/authors/AuthorModelItem';
import { scan } from 'rxjs/operators';
import { FilterPrintingEditionModel } from 'src/app/shared/models/filter/filter-printing-edition-model';
import { FilterAuthorModel } from 'src/app/shared/models/filter/filter-author-model';
import { PageSize } from 'src/app/shared/enums/page-size';

@Component({
  selector: 'app-printing-edition-edit-dialog',
  templateUrl: './printing-edition-edit-dialog.component.html',
  styleUrls: ['./printing-edition-edit-dialog.component.scss']
})

export class PrintingEditionEditDialogComponent implements OnInit, AfterContentChecked {

  isExistedData = false;

  constructor(public dialogRef: MatDialogRef<PrintingEdiotionsManagerComponent>,
              @Inject(MAT_DIALOG_DATA) public data: any, private printingEditionParams: PrintingEditionsParameters,
              private fb: FormBuilder, private authorService: AuthorService, private changeDetector: ChangeDetectorRef) {
    this.form = this.fb.group({
      title: new FormControl('', [Validators.required]),
      textarea: new FormControl('', [Validators.required]),
      types: new FormControl('', [Validators.required]),
      authors: new FormControl('', [Validators.required]),
      price: new FormControl('', [Validators.required]),
    });
    this.authorsSubj$ = this.authorsSubj.asObservable().pipe(
      scan((acc, curr) => {
        return [...acc, ...curr];
      }, [])
    );

  }
  get isValidForm(): boolean {
    return this.form.valid;
  }
  offset = 0;

  dataArray = new AuthorModel();

  authorsSubj = new BehaviorSubject<AuthorModelItem[]>([]);
  authorsSubj$ = new Observable<AuthorModelItem[]>();

  filterModel = new FilterPrintingEditionModel();

  authorFilter = new FilterAuthorModel();
  authorsModel = new AuthorModel();
  authorsId = new Array<number>();

  printingEditionTypes = this.printingEditionParams.printingEditionTypes;
  currencyTypes = this.printingEditionParams.currencyTypes;

  form: FormGroup;

  ngOnInit() {
    this.filterModel.currency = this.data.currency;

    if (this.data.authors) {
      this.data.authors.forEach(element => {
        this.authorsId.push(element.id);
      });
      this.authorsSubj.next(this.data.authors);
    }

    this.getNextAuthors();
  }

  close() {
    this.dialogRef.close(this.isExistedData);
  }

  getNextAuthors() {
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
  ngOnDestroy() {
    this.data.authors = new Array<AuthorModelItem>();
    this.authorsId.forEach(element => {
      this.data.authors.push({ id: element });
    });
  }
  ngAfterContentChecked() {
    this.changeDetector.detectChanges();
  }
}
