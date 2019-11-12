import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from 'src/app/app-routing.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { AccountModule } from 'src/app/account/account.module';
import { AuthorModule } from 'src/app/author/author.module';

import { MatSidenavModule } from '@angular/material';
import { HttpClientModule } from '@angular/common/http';
import { PrintingEditionModule } from 'src/app/printing-edition/printing-edition.module';

import { httpInterceptorProviders } from 'src/app/shared/interceptors';
import { UserModule } from 'src/app/user/user.module';
import { ApiRoutes } from 'src/environments/api-routes';
import { PrintingEditionsParametrs } from 'src/app/shared/constants/printing-editions-parametrs';
import { CookieService } from 'ngx-cookie-service';
import { UserParametrs } from 'src/app/shared/constants/user-parametrs';
import { ValidationPatterns } from 'src/app/shared/constants/validation-patterns';
import { AuthorParametrs } from 'src/app/shared/constants/author-parametrs';
import { OrderModule } from 'src/app/order/order.module';
import { OrderParametrs } from './shared/constants/order-parametrs';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    RouterModule,
    SharedModule,
    MatSidenavModule,
    AccountModule,
    AuthorModule,
    PrintingEditionModule,
    HttpClientModule,
    UserModule,
    OrderModule
  ],
  providers: [
    httpInterceptorProviders,
    CookieService,
    ApiRoutes,
    PrintingEditionsParametrs,
    UserParametrs,
    ValidationPatterns,
    AuthorParametrs,
    OrderParametrs
  ],
  bootstrap: [AppComponent],
})
export class AppModule { }
