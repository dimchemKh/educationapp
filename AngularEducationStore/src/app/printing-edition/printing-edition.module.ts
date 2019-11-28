import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { routes } from 'src/app/printing-edition/printing-edition-routing.module';
import { PrintingEditionsComponent } from 'src/app/printing-edition/printing-editions/printing-editions.component';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { PrintingEditionDetailsComponent } from 'src/app/printing-edition/printing-edition-details/printing-edition-details.component';
import { PrintingEdiotionsManagerComponent } from 'src/app/printing-edition/printing-editions-manager/printing-editions-manager.component';
// tslint:disable-next-line: max-line-length
import { PrintingEditionEditDialogComponent } from 'src/app/printing-edition/printing-editions-manager/printing-edition-edit-dialog/printing-edition-edit-dialog.component';
import { MatSelectInfiniteScrollModule } from 'ng-mat-select-infinite-scroll';
import { MaterialModule } from 'src/app/material.module';


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
    FormsModule,
    ReactiveFormsModule,
    FontAwesomeModule,
    MaterialModule,
    MatSelectInfiniteScrollModule
  ],
  exports: [],
  providers: [],
  entryComponents: [PrintingEditionEditDialogComponent]
})
export class PrintingEditionModule { }
