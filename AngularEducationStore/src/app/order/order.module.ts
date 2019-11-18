import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { routes } from 'src/app/order/order-routing.module';
import { OrdersUserComponent } from 'src/app/order/orders-user/orders-user.component';
import { OrdersAdminComponent } from 'src/app/order/orders-admin/orders-admin.component';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { MaterialModule } from 'src/app/material.module';

@NgModule({
    declarations: [
        OrdersUserComponent,
        OrdersAdminComponent
    ],
    imports: [
        CommonModule,
        RouterModule.forChild(routes),
        FontAwesomeModule,
        MaterialModule,
        FormsModule,
        ReactiveFormsModule
    ],
    exports: [],
    providers: [],
})
export class OrderModule {

}
