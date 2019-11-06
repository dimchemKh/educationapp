import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { routes } from 'src/app/printing-edition/printing-edition-routing.module';
import { PrintingEditionsComponent } from 'src/app/printing-edition/printing-editions/printing-editions.component';
import {
  MatFormFieldModule,
  MatInputModule,
  MatToolbarModule,
  MatGridListModule,
  MatCheckboxModule,
  MatSliderModule,
  MatButtonModule,
  MatSelectModule,
  MatDividerModule,
  MatPaginatorModule,
  MatTableModule,
  MatSortModule
} from '@angular/material';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { PrintingEditionDetailsComponent } from 'src/app/printing-edition/printing-edition-details/printing-edition-details.component';
import { PrintingEdiotionsManagerComponent } from 'src/app/printing-edition/printing-editions-manager/printing-editions-manager.component';
// tslint:disable-next-line: max-line-length
import { PrintingEditionEditDialogComponent } from 'src/app/printing-edition/printing-editions-manager/printing-edition-edit-dialog/printing-edition-edit-dialog.component';
import { MatSelectInfiniteScrollModule } from 'ng-mat-select-infinite-scroll';

@NgModule({
  declarations: [
    PrintingEditionsComponent,
    PrintingEditionDetailsComponent,
    PrintingEdiotionsManagerComponent,
    PrintingEditionEditDialogComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    MatFormFieldModule,
    FormsModule,
    ReactiveFormsModule,
    MatInputModule,
    MatToolbarModule,
    MatGridListModule,
    MatCheckboxModule,
    MatSliderModule,
    MatButtonModule,
    MatSelectModule,
    MatDividerModule,
    MatPaginatorModule,
    FontAwesomeModule,
    MatTableModule,
    MatSortModule,
    MatSelectInfiniteScrollModule
  ],
  exports: [],
  providers: [],
  entryComponents: [PrintingEditionEditDialogComponent]
})
export class PrintingEditionModule { }
