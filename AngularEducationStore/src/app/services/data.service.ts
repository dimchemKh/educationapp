import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DataService {

  constructor() { }

  setUserInfo(userName: string) {
    localStorage.setItem('userName', userName);
  }
  getUserInfo() {
    return localStorage.getItem('userName');
  }
}
