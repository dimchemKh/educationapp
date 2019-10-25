import { Component, OnInit, OnChanges } from '@angular/core';
import { Subscription, Observable, from } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { faBookOpen } from '@fortawesome/free-solid-svg-icons';
import { faUserCircle } from '@fortawesome/free-solid-svg-icons';
import { faUser } from '@fortawesome/free-solid-svg-icons';


@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  isLoggedIn$: Observable<boolean>;
  subscription: Subscription;
  isAuth = false;

  faUser = faUser;
  faBookOpen = faBookOpen;
  faUserCircle = faUserCircle;

  get userName(): string {
    return localStorage.getItem('userName');
  }

  constructor(private authService: AuthService) {
  }

  ngOnInit() {
    this.subscription = this.authService.authNavStatus$.subscribe(status => {
      this.isAuth = status;
    } );

  }
  signOut() {
    localStorage.clear();
    this.authService.signOut();
  }
  // tslint:disable-next-line: use-lifecycle-interface
  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
}
