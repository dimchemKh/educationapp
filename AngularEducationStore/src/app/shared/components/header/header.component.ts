import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { AccountService } from '../../services/account.service';
import { faBookOpen } from '@fortawesome/free-solid-svg-icons';
import { faUserCircle } from '@fortawesome/free-solid-svg-icons';
import { faUser } from '@fortawesome/free-solid-svg-icons';
import { faShoppingCart } from '@fortawesome/free-solid-svg-icons';
import { RemoveDialogComponent } from '../remove-dialog/remove-dialog.component';
import { MatDialog } from '@angular/material';
import { DataService } from '../../services/data.service';
import { CartItemsComponent } from 'src/app/cart/cart-items/cart-items.component';
import { OrderService } from '../../services/order.service';


@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit, OnDestroy {

  subscription: Subscription;
  isAuth = false;

  faUser = faUser;
  faBookOpen = faBookOpen;
  faUserCircle = faUserCircle;
  faShoppingCart = faShoppingCart;

  get userName(): string {
    return this.dataService.getLocalStorage('userName')
  }
  get userRole(): string {
    return this.dataService.getLocalStorage('userRole')
  }

  constructor(private authService: AccountService, private dialog: MatDialog,
              private orderService: OrderService, private dataService: DataService) {
  }

  ngOnInit() {
    this.subscription = this.authService.authNavStatus$.subscribe(status => {
      this.isAuth = status;
    });

  }
  getCountPurchase() {
    let orders = this.orderService.getAllPurchases();
    if (!orders || orders.items.length === 0) {
      return null;
    }
    return orders.items.length;
  }
  openCart() {
    let dialog = this.dialog.open(CartItemsComponent, {
      data: {

      }
    });
    dialog.afterClosed().subscribe(() => {
      
    });
  }
  signOut() {
    let dialog = this.dialog.open(RemoveDialogComponent, {
      data: {
        message: 'Do you wan`t leave?',
        closeTitle: 'Back',
        removeTitle: 'Ok'
      }
    });
    dialog.afterClosed().subscribe((result) => {
      if (result) {
        this.authService.signOut();
      }
    });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
}
