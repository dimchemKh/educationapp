import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private cookieService: CookieService, private router: Router) {
    this.authNavStatusSource.next(this.isAuth());
  }

  private authNavStatusSource = new BehaviorSubject<boolean>(false);
  authNavStatus$ = this.authNavStatusSource.asObservable();


  isAuth(): boolean {

    if (this.getUserInfo() !== null) {
      return true;
    }
    return false;
  }

  signIn() {
    this.authNavStatusSource.next(this.isAuth());
    this.router.navigate(['/']);
  }
  signOut() {
    this.cookieService.deleteAll();
    this.authNavStatusSource.next(false);
    this.router.navigate(['/account/signIn']);
  }
  getUserInfo() {
    return localStorage.getItem('userName');
  }
}
