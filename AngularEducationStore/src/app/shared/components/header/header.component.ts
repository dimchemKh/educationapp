import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { AccountService, CartService, DataService, UserService } from 'src/app/shared/services';
import { faBookOpen } from '@fortawesome/free-solid-svg-icons';
import { faUserCircle } from '@fortawesome/free-solid-svg-icons';
import { faUser } from '@fortawesome/free-solid-svg-icons';
import { faShoppingCart } from '@fortawesome/free-solid-svg-icons';
import { RemoveDialogComponent } from 'src/app/shared/components/remove-dialog/remove-dialog.component';

import { CartItemsComponent } from 'src/app/shared/components/cart-dialogs/cart-items/cart-items.component';
import { MatDialog } from '@angular/material';
import { SafeUrl, DomSanitizer } from '@angular/platform-browser';


@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit, OnDestroy {

  subscription: Subscription;
  cartSubscribe: Subscription;

  isAuth = false;
  image: SafeUrl = null;
  userImage: string;


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
  getUserImage() {
    this.userImage = this.dataService.getLocalStorage('userImage');

    if (this.userImage) {
      this.image = this.sanitizer.bypassSecurityTrustUrl(this.userImage);
    }

  }

  constructor(private authService: AccountService, private userService: UserService, private dialog: MatDialog,
              private cartService: CartService, private dataService: DataService, private sanitizer: DomSanitizer) {
  }

  ngOnInit() {
    this.userService.userImageSubject.subscribe((image) => {
      this.userImage = image;
      this.image = this.sanitizer.bypassSecurityTrustUrl(this.userImage);
    });
    this.subscription = this.authService.authNavStatus$.subscribe(status => {
      this.isAuth = status;
      this.getUserImage();
    });
    this.cartService.cartSource.subscribe((data) => {
      if (data) {
        this.countOrders = data.length;
      }
    });
    
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
