import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiRoutes } from 'src/environments/api-routes';
import { FilterOrderModel } from '../models/filter/filter-order-model';
import { Observable } from 'rxjs';
import { OrderModel } from '../models/order/OrderModel';
import { OrderItemModel } from '../models/order-item/OrderItemModel';
import { DataService } from './data.service';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  constructor(private http: HttpClient, private apiRoutes: ApiRoutes, private dataService: DataService) {
  }
  getAllPurchases() {
    let orderModel: OrderItemModel = JSON.parse(this.dataService.getLocalStorage('cartItems'));
    if (orderModel) {
      return orderModel;
    }
    return null;
  }
  addOrder(orders: OrderItemModel) {
    this.dataService.setLocalStorage('cartItems', JSON.stringify(orders));
  }
  removeOrderItem(printingEditionId: number) {
    let orders = this.getAllPurchases();
    orders.items = orders.items.filter(x => x.printingEditionId !== printingEditionId);
    if (orders.items.length === 0) {
      this.dataService.deleteItemLocalStorage('cartItems');
      return;
    }
    this.addOrder(orders);
  }
  checkTheSame(orders: OrderItemModel, printignEditionId: number) {
    let ordersItem = orders.items.filter(x => x.printingEditionId === printignEditionId);
    if (ordersItem.length === 1) {
      return true;
    }
    return false;
  }
  getOrders(role: string, filterModel: FilterOrderModel): Observable<OrderModel> {
    return this.http.post<OrderModel>(this.apiRoutes.orderRoute + 'get?role=' + role,  filterModel);
  }
}
