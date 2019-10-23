import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserLoginModel } from 'src/app/models/user/UserLoginModel';
import { UserRegistrationModel } from '../models/user/UserRegistrationModel';

@Injectable({
  providedIn: 'root'
})
export class AccountService {


  constructor(private http: HttpClient) { }

  signInUser(user: UserLoginModel) {
    return this.http.post('http://localhost:52196/api/account/signIn', user);
  }

  signUpUser(user: UserRegistrationModel) {
    return this.http.post('http://localhost:52196/api/account/signUp', user);
  }
}
