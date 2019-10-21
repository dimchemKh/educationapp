import { NgModule } from '@angular/core';
import { Routes } from '@angular/router';

import { GetPrintingEditionComponent } from 'src/app/printing-edition/get-printing-edition/get-printing-edition.component';

export const routes: Routes = [
    { path: 'get', component: GetPrintingEditionComponent }
];

@NgModule({
    imports: [],
    exports: []
})
export class PrintingEditionRoutingModule {}
