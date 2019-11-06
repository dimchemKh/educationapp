import { Component, OnInit, OnChanges } from '@angular/core';
import { Subscription, Observable, from } from 'rxjs';
import { AccountService } from '../services/account.service';
import { faBookOpen } from '@fortawesome/free-solid-svg-icons';
import { faUserCircle } from '@fortawesome/free-solid-svg-icons';
import { faUser } from '@fortawesome/free-solid-svg-icons';
import { faShoppingCart } from '@fortawesome/free-solid-svg-icons';


@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  subscription: Subscription;
  isAuth = false;

  faUser = faUser;
  faBookOpen = faBookOpen;
  faUserCircle = faUserCircle;
  faShoppingCart = faShoppingCart;

  get userName(): string {
    return localStorage.getItem('userName');
  }
  get userRole(): string {
    return localStorage.getItem('userRole');
  }

  constructor(private authService: AccountService) {
  }

  ngOnInit() {
    this.subscription = this.authService.authNavStatus$.subscribe(status => {
      this.isAuth = status;
    } );

  }
  signOut() {
    this.authService.signOut();
  }
  // tslint:disable-next-line: use-lifecycle-interface
  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
}
