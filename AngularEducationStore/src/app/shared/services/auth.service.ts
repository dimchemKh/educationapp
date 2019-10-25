import { Injectable, OnInit } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { UserLoginModel } from 'src/app/models/user/UserLoginModel';
import { UserRegistrationModel } from 'src/app/models/user/UserRegistrationModel';
import { UserRequestModel } from 'src/app/models/user/UserRequestModel';
import { BaseModel } from 'src/app/models/base/BaseModel';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private authNavStatusSource = new BehaviorSubject<boolean>(false);
  authNavStatus$ = this.authNavStatusSource.asObservable();

  constructor(private router: Router, private http: HttpClient) {
    this.authNavStatusSource.next(this.isAuth());
  }

  isAuth(): boolean {

    if (this.getUserInfo() !== null) {
      return true;
    }
    return false;
  }
  responseSignIn(userModel: UserLoginModel) {
    return this.http.post<UserRequestModel>('http://localhost:52196/api/account/signIn', userModel);
  }
  responseSignUp(user: UserRegistrationModel) {
    return this.http.post('http://localhost:52196/api/account/signUp', user);
  }
  responseForgotPassword(userModel: UserLoginModel) {
    return this.http.post<BaseModel>('http://localhost:52196/api/account/forgotPassword', userModel);
  }
  signIn() {
    this.authNavStatusSource.next(this.isAuth());
    this.router.navigate(['/']);
  }
  signUp() {

  }
  signOut() {
    this.authNavStatusSource.next(false);
    this.router.navigate(['/account/signIn']);
  }
  getUserInfo() {
    return localStorage.getItem('userName');
  }
}
