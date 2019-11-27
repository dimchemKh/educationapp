import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class DataService {
  
  constructor(private cookieService: CookieService) { }

  getCookie(name: string) {
    return this.cookieService.get(name);
  }
  setCookie(name: string, value: string, expires?: Date | number) {
    return this.cookieService.set(name, value, expires, '/');
  }
  deleteAllCookie(path: string) {
    this.cookieService.deleteAll(path);
  }
  getLocalStorage(name: string) {
    return localStorage.getItem(name);
  }
  setLocalStorage(name: string, value: string) {
    return localStorage.setItem(name, value);
  }
  deleteItemLocalStorage(name: string) {
    localStorage.removeItem(name);
  }
  clearLocalStorage() {
    localStorage.clear();
  }
}
