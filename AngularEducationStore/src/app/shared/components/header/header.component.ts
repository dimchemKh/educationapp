import { Component, OnInit, OnChanges } from '@angular/core';
import { Subscription, Observable, from } from 'rxjs';
import { AccountService } from '../../services/account.service';
import { faBookOpen } from '@fortawesome/free-solid-svg-icons';
import { faUserCircle } from '@fortawesome/free-solid-svg-icons';
import { faUser } from '@fortawesome/free-solid-svg-icons';
import { faShoppingCart } from '@fortawesome/free-solid-svg-icons';
import { RemoveDialogComponent } from '../remove-dialog/remove-dialog.component';
import { MatDialog } from '@angular/material';
import { RemoveModel } from '../../models/RemoveModel';


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

  constructor(private authService: AccountService, private dialog: MatDialog) {
  }

  ngOnInit() {
    this.subscription = this.authService.authNavStatus$.subscribe(status => {
      this.isAuth = status;
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
