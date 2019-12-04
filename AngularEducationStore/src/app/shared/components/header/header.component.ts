import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { AccountService, CartService, UserService } from 'src/app/shared/services';
import { faBookOpen, IconDefinition } from '@fortawesome/free-solid-svg-icons';
import { faUserCircle } from '@fortawesome/free-solid-svg-icons';
import { faUser } from '@fortawesome/free-solid-svg-icons';
import { faShoppingCart } from '@fortawesome/free-solid-svg-icons';
import { RemoveDialogComponent } from 'src/app/shared/components/remove-dialog/remove-dialog.component';
import { CartItemsComponent } from 'src/app/shared/components/cart-dialogs/cart-items/cart-items.component';
import { MatDialog } from '@angular/material';
import { SafeUrl, DomSanitizer } from '@angular/platform-browser';
import { AuthHelper } from '../../helpers/auth-helper';


@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit, OnDestroy {

  subscription: Subscription;
  cartSubscribe: Subscription;
  isAuth: boolean;
  image: SafeUrl;
  userImage: string;
  countOrders: number;
  faUser: IconDefinition;
  faBookOpen: IconDefinition;
  faUserCircle: IconDefinition;
  faShoppingCart: IconDefinition;

  get userName(): string {
    return this.authHelper.getUserNameFromToken();
  }
  get userRole(): string {
    return this.authHelper.getUserRoleFromToken();
  }


  constructor(
    private userService: UserService,
    private dialog: MatDialog,
    private cartService: CartService,
    private sanitizer: DomSanitizer,
    private authHelper: AuthHelper
  ) {
    this.isAuth = false;
    this.image = null;
    this.faUser = faUser;
    this.faBookOpen = faBookOpen;
    this.faUserCircle = faUserCircle;
    this.faShoppingCart = faShoppingCart;
  }

  ngOnInit() {
    this.authHelper.userImageSubject$.subscribe((image) => {
      this.userImage = image;
      this.image = this.sanitizer.bypassSecurityTrustUrl(this.userImage);
    });

    this.subscription = this.authHelper.getAuthNavStatus().subscribe(status => {
      this.isAuth = status;
      this.getUserImage();
    });

    this.cartService.getCartSource().subscribe((data) => {
      if (data) {
        this.countOrders = data.length;
      }
    });

  }

  getUserImage(): void {
    this.userImage = this.authHelper.getUserImageFromToken();

    if (this.userImage) {
      this.image = this.sanitizer.bypassSecurityTrustUrl(this.userImage);
    }
  }

  openCart(): void {
    let dialog = this.dialog.open(CartItemsComponent, {
      data: {

      }
    });

    dialog.afterClosed().subscribe(() => {

    });
  }

  signOut(): void {
    let dialog = this.dialog.open(RemoveDialogComponent, {
      data: {
        message: 'Do you wan`t leave?',
        closeTitle: 'Back',
        removeTitle: 'Ok'
      }
    });

    dialog.afterClosed().subscribe((result) => {
      if (result) {
        this.authHelper.logout();
      }
    });
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
