import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Router } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { BaseModel, UserLoginModel, UserRegistrationModel, UserRequestModel } from 'src/app/shared/models';

import { ApiRoutes } from 'src/environments/api-routes';
import { DataService, CartService } from 'src/app/shared/services/index';


@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private dataService: DataService, private router: Router, private http: HttpClient, private apiRoutes: ApiRoutes,
              private cartService: CartService) {
    this.authNavStatusSource.next(this.isAuth());
  }


  private authNavStatusSource = new BehaviorSubject<boolean>(false);
  authNavStatus$ = this.authNavStatusSource.asObservable();

  isAuth(): boolean {
    if (this.dataService.getLocalStorage('userName') !== null) {
      return true;
    }
    return false;
  }
  refresh() {
    return this.http.get(this.apiRoutes.accountRoute + 'refresh',
    {
      withCredentials: true
    });
  }
  signInUser(userModel: UserLoginModel) {
    const httpOptions = {
        headers: new HttpHeaders({'Content-Type': 'application/json'}),
        withCredentials: true
    };
    return this.http.post<UserRequestModel>(this.apiRoutes.accountRoute + 'signIn', userModel, httpOptions);
  }
  signUpUser(user: UserRegistrationModel) {
      return this.http.post(this.apiRoutes.accountRoute + 'signUp', user);
  }
  forgotPassword(userModel: UserLoginModel) {
      return this.http.post<BaseModel>(this.apiRoutes.accountRoute + 'forgotPassword', userModel);
  }
  signIn() {
    this.authNavStatusSource.next(this.isAuth());
    if (this.dataService.getLocalStorage('userRole') === 'user') {
      this.router.navigate(['/']);
      return;
    }
    this.router.navigate(['/printing-edition/manager']);

  }
  signOut() {
    this.dataService.clearLocalStorage();
    this.dataService.deleteAllCookie('/');
    this.authNavStatusSource.next(false);
    this.cartService.cartSource.next([]);
    this.router.navigate(['/account/signIn']);
  }
}
