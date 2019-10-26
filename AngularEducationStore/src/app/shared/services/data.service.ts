import { Injectable } from '@angular/core';

import { UserLoginModel } from 'src/app/models/user/UserLoginModel';

import { UserRequestModel } from 'src/app/models/user/UserRequestModel';
import { UserRegistrationModel } from 'src/app/models/user/UserRegistrationModel';
import { BaseModel } from 'src/app/models/base/BaseModel';
import { HttpHeaders, HttpClient } from '@angular/common/http';

@Injectable({
    providedIn: 'root'
  })
export class DataService {

    constructor(private http: HttpClient) {}

    requestSignIn(userModel: UserLoginModel) {
        const httpOptions = {
            headers: new HttpHeaders({'Content-Type': 'application/json'}),
            withCredentials: true
        };
        return this.http.post<UserRequestModel>('http://localhost:52196/api/account/signIn', userModel, httpOptions);
    }
    requestSignUp(user: UserRegistrationModel) {
        return this.http.post('http://localhost:52196/api/account/signUp', user);
    }
    requestForgotPassword(userModel: UserLoginModel) {
        return this.http.post<BaseModel>('http://localhost:52196/api/account/forgotPassword', userModel);
    }
    requestGetAuthors() {
        const httpOptions = {
            // header = 'wqe',
            // headers: new HttpHeaders({'Content-Type': 'application/json'}),
            withCredentials: true
        };
        return this.http.get('http://localhost:52196/api/author/test');
    }

}
