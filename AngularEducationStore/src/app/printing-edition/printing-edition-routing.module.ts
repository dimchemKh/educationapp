import { NgModule } from '@angular/core';
import { Routes } from '@angular/router';

import { PrintingEditionsComponent } from 'src/app/printing-edition/printing-editions/printing-editions.component';
import { PrintingEditionDetailsComponent } from './printing-edition-details/printing-edition-details.component';
import { PrintingEdiotionsManagerComponent } from './printing-editions-manager/printing-editions-manager.component';
import { AdminGuard } from '../shared/guards/admin.guard';
import { NonAdminGuard } from '../shared/guards/non-admin.guard';



export const routes: Routes = [
    { path: '', component: PrintingEditionsComponent, canActivate: [NonAdminGuard] },
    { path: 'details', component: PrintingEditionDetailsComponent, canActivate: [NonAdminGuard] },
    { path: 'manager', component: PrintingEdiotionsManagerComponent, canActivate: [AdminGuard] },
];

@NgModule({
    imports: [],
    exports: []
})
export class PrintingEditionRoutingModule {}
