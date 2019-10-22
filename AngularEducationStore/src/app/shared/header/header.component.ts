import { Component, OnInit, OnChanges } from '@angular/core';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/services/auth.service';
import { UserLoginModel } from 'src/app/models/user/UserLoginModel';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  constructor(private authService: AuthService) { }

  userName: string;

  isAuthentificated: boolean;
  subscription: Subscription;

  ngOnInit(): void {
    this.subscription = this.authService.authNavStatus$.subscribe(
      status =>
      this.isAuthentificated = status
      );
  }

  signOut() {
    this.authService.signOut();
  }


  // tslint:disable-next-line: use-lifecycle-interface
  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
}
