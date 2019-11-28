import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiRoutes } from 'src/environments/api-routes';
import { Observable } from 'rxjs';
import { FilterOrderModel, OrderModel, OrderModelItem, PaymentModel } from 'src/app/shared/models';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  constructor(private http: HttpClient, private apiRoutes: ApiRoutes
    ) {

  }

  getOrders(role: string, filterModel: FilterOrderModel): Observable<OrderModel> {
    return this.http.post<OrderModel>(this.apiRoutes.orderRoute + 'get?role=' + role,  filterModel);
  }

  createOrder(role: string, orderModel: OrderModelItem): Promise<OrderModel> {
    return this.http.post<OrderModel>(this.apiRoutes.orderRoute + 'create?role=' + role, orderModel).toPromise();
  }

  updateOrder(payment: PaymentModel): Promise<Object> {
    return this.http.post(this.apiRoutes.orderRoute + 'update', payment).toPromise();
  }
}
