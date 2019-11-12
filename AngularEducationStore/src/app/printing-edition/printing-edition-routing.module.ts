import { NgModule } from '@angular/core';
import { Routes } from '@angular/router';

import { PrintingEditionsComponent } from 'src/app/printing-edition/printing-editions/printing-editions.component';
import { PrintingEditionDetailsComponent } from 'src/app/printing-edition/printing-edition-details/printing-edition-details.component';
import { PrintingEdiotionsManagerComponent } from 'src/app/printing-edition/printing-editions-manager/printing-editions-manager.component';
import { AdminGuard } from 'src/app/shared/guards/admin.guard';
import { NonAdminGuard } from 'src/app/shared/guards/non-admin.guard';
import { AuthGuard } from '../shared/guards/auth.guard';



export const routes: Routes = [
    { path: '', component: PrintingEditionsComponent, canActivate: [NonAdminGuard] },
    { path: 'details/:id', component: PrintingEditionDetailsComponent, canActivate: [AuthGuard] },
    { path: 'manager', component: PrintingEdiotionsManagerComponent, canActivate: [AdminGuard] },
];

@NgModule({
    imports: [],
    exports: []
})
export class PrintingEditionRoutingModule {}
