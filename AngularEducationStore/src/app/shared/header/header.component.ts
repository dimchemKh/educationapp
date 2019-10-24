import { Component, OnInit, OnChanges } from '@angular/core';
import { Subscription, Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  isLoggedIn$: Observable<boolean>;
  subscription: Subscription;
  isAuth = false;

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
