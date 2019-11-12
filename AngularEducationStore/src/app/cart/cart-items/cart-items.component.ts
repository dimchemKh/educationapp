import { Component, OnInit } from '@angular/core';
import { OrderItemModel } from 'src/app/shared/models/order-item/OrderItemModel';
import { DataService } from 'src/app/shared/services/data.service';
import { OrderItemModelItem } from 'src/app/shared/models/order-item/OrderItemModelItem';

@Component({
  selector: 'app-cart-items',
  templateUrl: './cart-items.component.html',
  styleUrls: ['./cart-items.component.scss']
})
export class CartItemsComponent implements OnInit {

  isEmptyCart = false;
  quantities = Array<number>();
  quantity: number;
  
  ordersModel = new OrderItemModel();
  displayedColumns = ['product', 'price', 'qty', 'amount', ' '];
  
  constructor(private dataService: DataService) {
    for (let i = 1; i < 10; i++) {
      this.quantities.push(i);
    }
   }

  ngOnInit() {
    let count = this.dataService.getCount();
    if (count <= 0) {
      this.isEmptyCart = true;
      return
    }
    for (let i = 0; i < count; i++) {
      let orderItem: OrderItemModelItem = JSON.parse(this.dataService.getLocalStorage('cartItem' + i));
      this.ordersModel.items.push(orderItem);
    }
    console.log(this.ordersModel);
  }
  removeOrderItem(element) {
    console.log(element);
  }
}
