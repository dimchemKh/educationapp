import { Injectable } from '@angular/core';
import {
    HttpEvent,
    HttpInterceptor,
    HttpHandler,
    HttpRequest,
    HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError, from } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { AccountService } from 'src/app/shared/services';
import { AuthHelper } from 'src/app/shared/helpers/auth-helper';
import { CookieService } from 'ngx-cookie-service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  private isRefreshing: boolean;

  constructor(
    private authHelper: AuthHelper,
    private accountService: AccountService,
    private cookieService: CookieService
    ) {
      this.isRefreshing = false;
  }  

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    let expireRemember = this.cookieService.get('expire');

    // if (accessToken && expireRemember) {
    //   req = this.addToken(req, accessToken);
    // }

    // if (!accessToken && expireRemember) {
    //   return next.handle(req).pipe(catchError(error => {
    //     if (error instanceof HttpErrorResponse && error.status === 401) {
    //       if (refreshToken && expireRemember) {
    //         return this.handle401Error(req, next);
    //       }
    //       if (!expireRemember || !refreshToken) {
    //         this.authHelper.signOut();
    //       }
    //       return throwError('');
    //     } else {
    //     return throwError(error);
    //   }
    // }));
    // }

    return next.handle(req);
}
  private handle401Error(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (!this.isRefreshing) {
      this.isRefreshing = true;

      return this.accountService.refresh().pipe(
        switchMap((token: any) => {
          // token = this.dataSerive.getCookie('Access');

          this.isRefreshing = false;

          return next.handle(this.addToken(req, token));
        }
      ));
    }
  }
  private addToken(request: HttpRequest<any>, token: string): HttpRequest<any> {
    return request.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });

  }
}
