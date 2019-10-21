import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { UserLoginModel } from 'src/app/models/user/userLoginModel';
import { from } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  private url = 'api/account';
  constructor(private http: HttpClient) { }

  signInUser(user: UserLoginModel) {
    return this.http.post('http://localhost:52196/api/account/signIn', user);
  }
}
