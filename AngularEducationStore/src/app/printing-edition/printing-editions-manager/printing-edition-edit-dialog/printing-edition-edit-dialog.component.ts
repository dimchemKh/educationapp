import { Component, OnInit, Inject, AfterViewInit, ChangeDetectorRef, AfterContentChecked  } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { PrintingEdiotionsManagerComponent } from 'src/app/printing-edition/printing-editions-manager/printing-editions-manager.component';
import { PrintingEditionsParametrs } from 'src/app/shared/constants/printing-editions-parametrs';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { AuthorService } from 'src/app/shared/services/author.service';
import { PageSize } from 'src/app/shared/enums/page-size';
import { AuthorModel } from 'src/app/shared/models/authors/AuthorModel';
import { BehaviorSubject, Observable, pipe } from 'rxjs';
import { AuthorModelItem } from 'src/app/shared/models/authors/AuthorModelItem';
import { scan, map, finalize, tap } from 'rxjs/operators';
import { FilterPrintingEditionModel } from 'src/app/shared/models/filter/filter-printing-edition-model';

@Component({
  selector: 'app-printing-edition-edit-dialog',
  templateUrl: './printing-edition-edit-dialog.component.html',
  styleUrls: ['./printing-edition-edit-dialog.component.scss']
})

export class PrintingEditionEditDialogComponent implements OnInit, AfterContentChecked {

  isExistedData = false;

  constructor(public dialogRef: MatDialogRef<PrintingEdiotionsManagerComponent>,
              @Inject(MAT_DIALOG_DATA) public data: any, private printingEditionParams: PrintingEditionsParametrs,
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
    if (this.data.authors) {
      this.data.authors.forEach(element => {
        this.authorsId.push(element.id);
      });
      this.authorsSubj.next(this.data.authors);
    }

    

  }
  get isValidForm(): boolean {
    return this.form.valid;
  }
  offset = 0;

  dataArray = new AuthorModel();
  
  authorsSubj = new BehaviorSubject<AuthorModelItem[]>([]);
  authorsSubj$: Observable<AuthorModelItem[]>;

  filterModel = new FilterPrintingEditionModel();
  authorsModel = new AuthorModel();
  authorsId = new Array<number>();

  printingEditionTypes = this.printingEditionParams.printingEditionTypes;
  currencyTypes = this.printingEditionParams.currencyTypes;

  form: FormGroup;

  ngOnInit() {
    this.filterModel.pageSize = PageSize.Twelve;
    this.filterModel.page = 1;
    this.filterModel.currency = this.data.currency;

    this.getNextBatch();
  }

  close() {
    this.dialogRef.close(this.isExistedData);
  }

  getNextBatch() {
    
    this.authorService.getAllAuthors(this.filterModel).subscribe((data: AuthorModel) => {
      this.authorsModel.itemsCount = data.itemsCount;

      data = this.compareArray(data);
      
      this.authorsSubj.next(data.items);

    });

    this.filterModel.page += 1;
    this.offset += this.filterModel.pageSize;
  }
  compareArray(data: AuthorModel) {
    for (let i = 0; i < data.items.length; i++) {
      // tslint:disable-next-line: prefer-for-of
      for (let j = 0; j < this.authorsId.length; j++) {
        if (this.authorsId[j] === data.items[i].id) {
          data.items.splice(i, 1);
        }
      }
    }
    return data;
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
