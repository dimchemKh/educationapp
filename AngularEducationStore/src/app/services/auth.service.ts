import { Injectable } from '@angular/core';
import { UserLoginModel } from '../models/user/UserLoginModel';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private authNavStatusSource = new BehaviorSubject<boolean>(false);
  authNavStatus$ = this.authNavStatusSource.asObservable();

  constructor() { }

  signOut() {
    // redirect
  }

}
