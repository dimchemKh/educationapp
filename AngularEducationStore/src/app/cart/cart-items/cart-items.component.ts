import { Component, OnInit } from '@angular/core';
import { OrderItemModel } from 'src/app/shared/models/order-item/OrderItemModel';
import { DataService } from 'src/app/shared/services/data.service';
import { OrderItemModelItem } from 'src/app/shared/models/order-item/OrderItemModelItem';
import { OrderService } from 'src/app/shared/services/order.service';

@Component({
  selector: 'app-cart-items',
  templateUrl: './cart-items.component.html',
  styleUrls: ['./cart-items.component.scss']
})
export class CartItemsComponent implements OnInit {

  isEmptyCart = false;
  quantities = Array<number>();
  quantity: number;
  
  orders = new OrderItemModel();
  displayedColumns = ['product', 'price', 'qty', 'amount', ' '];
  i:number;
  
  constructor(private orderService: OrderService) {
    for (let i = 1; i < 10; i++) {
      this.quantities.push(i);
    }
   }

  ngOnInit() {
    this.getOrders();
  }

  getOrders() {
    let orders = this.orderService.getAllPurchases();
    if (!orders || orders.items.length === 0) {
      // debugger
      this.isEmptyCart = true;
      return;
    }
    this.orders = orders;
  }
  removeOrderItem(printingEditionId: number) {
    this.orderService.removeOrderItem(printingEditionId);
    this.getOrders();
  }
}
