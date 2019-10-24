import { Injectable, OnInit } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { UserLoginModel } from 'src/app/models/user/UserLoginModel';
import { UserRegistrationModel } from 'src/app/models/user/UserRegistrationModel';
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
    debugger
    if (localStorage.length > 0) {
      return true;
    }
    return false;
  }
  responseSI(userModel: UserLoginModel) {
    return this.http.post<UserLoginModel>('http://localhost:52196/api/account/signIn', userModel);
  }
  signInUser() {
    debugger
    // let qwe = this.responseSI(userModel).subscribe((data: )=> );
    debugger
    // qwe.subscribe((data: UserLoginModel ) => this.mapModel(data));
    debugger
  }
  signUpUser(user: UserRegistrationModel) {
    return this.http.post('http://localhost:52196/api/account/signUp', user);
  }
  signIn() {
    debugger
    this.authNavStatusSource.next(this.isAuth());
    debugger
    this.router.navigate(['/']);
  }
  signOut() {
    this.authNavStatusSource.next(false);
  }
  getUserInfo() {
    return localStorage.getItem('userName');
  }
}
