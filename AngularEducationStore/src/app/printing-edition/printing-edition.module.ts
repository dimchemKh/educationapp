import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { routes } from 'src/app/printing-edition/printing-edition-routing.module';
import { GetPrintingEditionComponent } from './get-printing-edition/get-printing-edition.component';
import {
  MatFormFieldModule,
  MatInputModule,
  MatToolbarModule,
  MatGridListModule,
  MatCheckboxModule,
  MatSliderModule,
  MatButtonModule,
  MatAutocompleteModule,
  MatSelectModule
} from '@angular/material';
import { FormsModule } from '@angular/forms';




@NgModule({
  declarations: [
    GetPrintingEditionComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    MatFormFieldModule,
    FormsModule,
    MatInputModule,
    MatToolbarModule,
    MatGridListModule,
    MatCheckboxModule,
    MatSliderModule,
    MatButtonModule,
    MatSelectModule
  ],
  exports: []
})
export class PrintingEditionModule { }
