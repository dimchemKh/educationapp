import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { routes } from 'src/app/printing-edition/printing-edition-routing.module';
import { GetPrintingEditionComponent } from './get-printing-edition/get-printing-edition.component';



@NgModule({
  declarations: [
    GetPrintingEditionComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes)
  ],
  exports: []
})
export class PrintingEditionModule { }
