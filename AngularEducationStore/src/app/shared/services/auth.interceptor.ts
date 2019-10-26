import { Injectable } from '@angular/core';
import {
    HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpResponse
} from '@angular/common/http';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private cookieService: CookieService) {}
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    const idToken = this.cookieService.get('access');

    if (idToken) {
      const cloned = req.clone({
          headers: req.headers.set('Authorization',
              'Bearer ' + idToken)
      });
      return next.handle(cloned);
    } else {
      return next.handle(req);
    }
  }
}
