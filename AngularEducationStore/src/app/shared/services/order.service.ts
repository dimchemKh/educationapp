import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiRoutes } from 'src/environments/api-routes';
import { FilterOrderModel } from '../models/filter/filter-order-model';
import { Observable } from 'rxjs';
import { OrderModel } from '../models/order/OrderModel';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  constructor(private http: HttpClient, private apiRoutes: ApiRoutes) {
  }

  getOrders(role: string, filterModel: FilterOrderModel): Observable<OrderModel> {
    return this.http.post<OrderModel>(this.apiRoutes.orderRoute + 'get?role=' + role,  filterModel);
  }
}
