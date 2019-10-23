import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private authNavStatusSource = new BehaviorSubject<boolean>(false);
  authNavStatus$ = this.authNavStatusSource.asObservable();

  constructor() {
    this.authNavStatusSource.next(this.editHeader());
  }

  editHeader(): boolean {
    if (this.getUserInfo() !== null) {
      return true;
    }
    return false;
  }

  login() {
    this.authNavStatusSource.next(this.editHeader());
  }

  getUserInfo() {
    return localStorage.getItem('userName');
  }
}
