import { Injectable } from '@angular/core';
import {
    HttpEvent,
    HttpInterceptor,
    HttpHandler,
    HttpRequest,
    HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { tap, catchError, switchMap, filter, take } from 'rxjs/operators';

import { AccountService } from 'src/app/shared/services/account.service';
import { Router } from '@angular/router';
import { DataService } from 'src/app/shared/services/data.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private dataSerive: DataService, private accountService: AccountService, private router: Router) {}
  private isRefreshing = false;

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const idToken = this.dataSerive.getCookie('Access');
    const refreshToken = this.dataSerive.getCookie('Refresh');

    if (idToken) {
      req = this.addToken(req, idToken);
    }
    if (!idToken) {
      return next.handle(req).pipe(catchError(error => {
        if (error instanceof HttpErrorResponse && error.status === 401) {
          if (refreshToken) {
            return this.handle401Error(req, next);
          }
          this.accountService.signOut();
          return throwError('');
        } else {
        return throwError(error);
      }
    }));
    }
    return next.handle(req);

}
  private handle401Error(req: HttpRequest<any>, next: HttpHandler) {
    if (!this.isRefreshing) {
      this.isRefreshing = true;
      return this.accountService.refresh().pipe(
        switchMap((token: any) => {
          token = this.dataSerive.getCookie('Access');
          this.isRefreshing = false;
          return next.handle(this.addToken(req, token));
        }
      ));
    }
  }
  private addToken(request: HttpRequest<any>, token: string) {
    return request.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });

  }
}
