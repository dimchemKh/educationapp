import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription, BehaviorSubject } from 'rxjs';
import { AccountService } from 'src/app/shared/services/account.service';
import { faBookOpen } from '@fortawesome/free-solid-svg-icons';
import { faUserCircle } from '@fortawesome/free-solid-svg-icons';
import { faUser } from '@fortawesome/free-solid-svg-icons';
import { faShoppingCart } from '@fortawesome/free-solid-svg-icons';
import { RemoveDialogComponent } from 'src/app/shared/components/remove-dialog/remove-dialog.component';
import { MatDialog } from '@angular/material';
import { DataService } from 'src/app/shared/services/data.service';
import { CartItemsComponent } from 'src/app/shared/cart-dialogs/cart-items/cart-items.component';
import { OrderService } from 'src/app/shared/services/order.service';
import { CartService } from '../../services/cart.service';


@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit, OnDestroy {

  subscription: Subscription;
  isAuth = false;

  cartSubscribe: Subscription;

  countOrders: number;

  faUser = faUser;
  faBookOpen = faBookOpen;
  faUserCircle = faUserCircle;
  faShoppingCart = faShoppingCart;

  get userName(): string {
    return this.dataService.getLocalStorage('userName');
  }
  get userRole(): string {
    return this.dataService.getLocalStorage('userRole');
  }

  constructor(private authService: AccountService, private dialog: MatDialog,
              private cartService: CartService, private dataService: DataService) {
  }

  ngOnInit() {
    this.subscription = this.authService.authNavStatus$.subscribe(status => {
      this.isAuth = status;
    });
    this.cartService.cartSource.subscribe((data) => {
      if (data) {
        this.countOrders = data.length;
      }
    });
  }
  getCountPurchase() {   
    
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
        this.cartService.cartSource.next([]);
      }
    });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
}
