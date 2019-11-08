import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { routes } from 'src/app/order/order-routing.module';
import { OrdersUserComponent } from 'src/app/order/orders-user/orders-user.component';
import { OrdersAdminComponent } from 'src/app/order/orders-admin/orders-admin.component';
import {
    MatFormFieldModule,
    MatButtonModule,
    MatInputModule,
    MatToolbarModule,
    MatGridListModule,
    MatCheckboxModule,
    MatSliderModule,
    MatSelectModule,
    MatDividerModule,
    MatPaginatorModule,
    MatCardModule,
    MatTableModule,
    MatSortModule,
    MatDialogModule
} from '@angular/material';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

@NgModule({
    declarations: [
        OrdersUserComponent,
        OrdersAdminComponent
    ],
    imports: [
        CommonModule,
        RouterModule.forChild(routes),
        MatFormFieldModule,
        MatButtonModule,
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
        MatSortModule,
        MatDialogModule
    ],
    exports: [],
    providers: [],
})
export class OrderModule {

}
