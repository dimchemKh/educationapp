import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { NgxStripeModule } from 'ngx-stripe';
import { FontAwesomeModule  } from '@fortawesome/angular-fontawesome';
import { MaterialModule } from 'src/app/material.module';
import { routes } from 'src/app/shared/shared-routing.module';
import { RouterModule } from '@angular/router';
import {
  AccountService,
  OrderService,
  AuthorService,
  PrintingEditionService,
  UserService,
  PaymentService,
  CookieService
} from 'src/app/shared/services';
import { AuthGuard } from 'src/app/shared/guards/auth.guard';
import { RemoveDialogComponent } from 'src/app/shared/components/remove-dialog/remove-dialog.component';
import { HeaderComponent } from 'src/app/shared/components/header/header.component';
import { FooterComponent } from 'src/app/shared/components/footer/footer.component';
import { CartItemsComponent } from 'src/app/shared/components/cart-dialogs/cart-items/cart-items.component';
import { CartSuccessComponent } from 'src/app/shared/components/cart-dialogs/cart-success/cart-success.component';
import { NotFoundComponent } from 'src/app/shared/components/not-found/not-found.component';
import { AuthHelper } from 'src/app/shared/helpers/auth-helper';
import { JwtHelperService } from '@auth0/angular-jwt';

@NgModule({
  declarations: [
    HeaderComponent,
    FooterComponent,
    RemoveDialogComponent,
    CartItemsComponent,
    CartSuccessComponent,
    NotFoundComponent
  ],
  exports: [
    HeaderComponent,
    FooterComponent
  ],
  imports: [
    NgxStripeModule.forRoot(),
    CommonModule,
    ReactiveFormsModule,
    RouterModule.forChild(routes),
    FontAwesomeModule,
    MaterialModule
  ],
  bootstrap: [],
  providers: [
    AccountService,
    AuthorService,
    CookieService,
    UserService,
    PrintingEditionService,
    OrderService,
    AuthGuard,
    PaymentService,
    AuthHelper,
    JwtHelperService
  ],
  entryComponents: [RemoveDialogComponent, CartItemsComponent, CartSuccessComponent]
})
export class SharedModule { }
