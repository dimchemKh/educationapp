import { MaterialModule } from 'src/app/material.module';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from 'src/app/app-routing.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { AccountModule } from 'src/app/account/account.module';
import { AuthorModule } from 'src/app/author/author.module';
import { UserModule } from 'src/app/user/user.module';
import { PrintingEditionModule } from 'src/app/printing-edition/printing-edition.module';
import { OrderModule } from 'src/app/order/order.module';
import { ApiRoutes } from 'src/environments/api-routes';
import { ValidationPatterns } from 'src/app/shared/constants/validation-patterns';
import { UserParameters } from 'src/app/shared/constants/user-parametrs';
import { PrintingEditionsParameters } from 'src/app/shared/constants/printing-editions-parameters';
import { AuthorParameters } from 'src/app/shared/constants/author-parameters';
import { OrderParameters } from 'src/app/shared/constants/order-parameters';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from 'src/app/shared/interceptors/auth.interceptor';
import { ColumnsTitles } from 'src/app/shared/constants/columns-titles';

export const HttpInterceptorProviders = [
  { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
];

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    MaterialModule,
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    RouterModule,
    HttpClientModule,
    SharedModule
  ],
  providers: [ 
    HttpInterceptorProviders,
    ApiRoutes,
    PrintingEditionsParameters,
    UserParameters,
    ValidationPatterns,
    AuthorParameters,
    OrderParameters,
    ColumnsTitles
  ],
  bootstrap: [AppComponent],
})
export class AppModule { }
