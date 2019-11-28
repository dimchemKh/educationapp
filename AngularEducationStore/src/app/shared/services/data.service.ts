import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class DataService {
  
  constructor(private cookieService: CookieService
    ) { 

  }

  getCookie(name: string): string {
    return this.cookieService.get(name);
  }

  setCookie(name: string, value: string, expires?: Date | number): void {
    return this.cookieService.set(name, value, expires, '/');
  }

  deleteAllCookie(path: string): void {
    this.cookieService.deleteAll(path);
  }

  getLocalStorage(name: string): string {
    return localStorage.getItem(name);
  }

  setLocalStorage(name: string, value: string): void {
    return localStorage.setItem(name, value);
  }

  deleteItemLocalStorage(name: string): void {
    localStorage.removeItem(name);
  }

  clearLocalStorage(): void {
    localStorage.clear();
  }
}
