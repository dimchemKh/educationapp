import { NgModule } from '@angular/core';
import { OrdersUserComponent } from './orders-user/orders-user.component';
import { Routes } from '@angular/router';
import { OrdersAdminComponent } from './orders-admin/orders-admin.component';
import { AdminGuard } from 'src/app/shared/guards/admin.guard';
import { UserGuard } from 'src/app/shared/guards/user.guard';


export const routes: Routes = [
    { path: 'my-orders', component: OrdersUserComponent, canActivate: [UserGuard] },
    { path: 'get-all', component: OrdersAdminComponent, canActivate: [AdminGuard] }
];


@NgModule({
    imports: [],
    exports: []
})
export class OrderRoutingModule {

}
