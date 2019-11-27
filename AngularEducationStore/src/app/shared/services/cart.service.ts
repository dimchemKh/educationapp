import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { ApiRoutes } from 'src/environments/api-routes';
import { DataService } from 'src/app/shared/services/data.service';
import { HttpClient } from '@angular/common/http';
import { ConverterModel, OrderModelItem } from 'src/app/shared/models';

@Injectable({
  providedIn: 'root'
})
export class CartService {  

  cartSource = new BehaviorSubject<number[]>([]);

  constructor(private http: HttpClient, private apiRoutes: ApiRoutes, private dataService: DataService) { 
    let orders = this.getAllPurchases();
    if (orders) {
      let values = this.cartSource.value;
      orders.orderItems.forEach((x) => {
        values.push(x.printingEditionId);
      });
      this.cartSource.next(values);
      return;
    }
    this.cartSource.next([]);
  }

  getAllPurchases() {
    let orderModel: OrderModelItem = JSON.parse(this.dataService.getLocalStorage('cartItems'));
    if (orderModel) {
      return orderModel;
    }
    return null;
  }
  async addOrder(orders: OrderModelItem) {
    await this.dataService.setLocalStorage('cartItems', JSON.stringify(orders));
  }
  convertToCart(converterModel: ConverterModel) { 
    return this.http.post<number>(this.apiRoutes.orderRoute + 'converting', converterModel).toPromise();
  }
  async removeOrderItem(printingEditionId: number) {
    let orders = this.getAllPurchases();
    orders.orderItems = orders.orderItems.filter(x => x.printingEditionId !== printingEditionId);

    let cartItems = this.cartSource.value.filter(x => x !== printingEditionId);

    this.cartSource.next(cartItems);
    if (orders.orderItems.length === 0) {
      this.dataService.deleteItemLocalStorage('cartItems');
      return;
    }
    await this.addOrder(orders);
    
  }
  checkTheSame(printignEditionsId: number[], printignEditionId: number) {
    if (printignEditionsId) {
      let items = printignEditionsId.filter(id => id === printignEditionId);
      if (items.length === 1) {
        return true;
      }
      return false;
    }    
  }
}
