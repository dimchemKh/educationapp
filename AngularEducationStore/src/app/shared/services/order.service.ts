import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiRoutes } from 'src/environments/api-routes';
import { FilterOrderModel } from '../models/filter/filter-order-model';
import { Observable } from 'rxjs';
import { OrderModel } from '../models/order/OrderModel';
import { OrderItemModel } from '../models/order-item/OrderItemModel';
import { DataService } from './data.service';
import { OrderItemModelItem } from '../models/order-item/OrderItemModelItem';
import { ConverterModel } from '../models/ConverterModel';
import { OrderModelItem } from '../models/order/OrderModelItem';
import { Model } from 'src/app/shared/models/model';


@Injectable({
  providedIn: 'root'
})
export class OrderService {

  constructor(private http: HttpClient, private apiRoutes: ApiRoutes, private dataService: DataService) {
  }

  getOrders(role: string, filterModel: FilterOrderModel): Observable<OrderModel> {
    return this.http.post<OrderModel>(this.apiRoutes.orderRoute + 'get?role=' + role,  filterModel);
  }
  createOrder(role: string, orderModel: OrderModelItem): Observable<OrderModel> {
    debugger
    return this.http.post<OrderModel>(this.apiRoutes.orderRoute + 'create?role=' + role, orderModel);
  }
  updateOrder(model: Model) {
    return this.http.post(this.apiRoutes.orderRoute + 'update', model);
  }
}
