import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { routes } from 'src/app/printing-edition/printing-edition-routing.module';
import { PrintingEditionsComponent } from './printing-editions/printing-editions.component';
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
  MAT_CHECKBOX_CLICK_ACTION,
  MatPaginatorModule,
  MatCardModule,
  MatTableModule,
  MatSortModule
} from '@angular/material';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { PrintingEditionDetailsComponent } from './printing-edition-details/printing-edition-details.component';
import { PrintingEdiotionsManagerComponent } from 'src/app/printing-edition/printing-editions-manager/printing-editions-manager.component';





@NgModule({
  declarations: [
    PrintingEditionsComponent,
    PrintingEditionDetailsComponent,
    PrintingEdiotionsManagerComponent
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
    MatCardModule,
    MatTableModule,
    MatSortModule
  ],
  exports: [],
  providers: []
})
export class PrintingEditionModule { }
