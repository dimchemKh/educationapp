import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from 'src/app/shared/components/header/header.component';
import { FooterComponent } from 'src/app/shared/components/footer/footer.component';
import { routes } from 'src/app/shared/shared-routing.module';
import { RouterModule } from '@angular/router';

import { AccountService } from 'src/app/shared/services/account.service';
import { FontAwesomeModule  } from '@fortawesome/angular-fontawesome';
import {
  MatToolbarModule,
  MatMenuModule,
  MatButtonModule,
  MatDialogModule,
  MatBadgeModule,
  MatTableModule,
  MatSelectModule,
  MatFormFieldModule,
  MatInputModule
} from '@angular/material';
import { AuthorService } from 'src/app/shared/services/author.service';
import { PrintingEditionService } from 'src/app/shared/services/printing-edition.service';
import { UserService } from 'src/app/shared/services/user.service';
import { DataService } from 'src/app/shared/services/data.service';
import { AuthGuard } from 'src/app/shared/guards/auth.guard';
import { RemoveDialogComponent } from 'src/app/shared/components/remove-dialog/remove-dialog.component';
import { OrderService } from 'src/app/shared/services/order.service';
import { CartItemsComponent } from 'src/app/shared/cart-dialogs/cart-items/cart-items.component';
import { PaymentService } from 'src/app/shared/services/payment.service';
import { CartTransactionComponent } from 'src/app/shared/cart-dialogs/cart-transaction/cart-transaction.component';
import { NgxStripeModule } from 'ngx-stripe';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    HeaderComponent,
    FooterComponent,
    RemoveDialogComponent,
    CartItemsComponent,
    CartTransactionComponent
  ],
  exports: [
    HeaderComponent,
    FooterComponent
  ],
  imports: [
    NgxStripeModule.forRoot(),
    CommonModule,
    MatToolbarModule,
    MatMenuModule,
    RouterModule.forChild(routes),
    FontAwesomeModule,
    MatButtonModule,
    MatDialogModule,
    MatBadgeModule,
    MatTableModule,
    MatSelectModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule
  ],
  bootstrap: [],
  providers: [
    AccountService,
    AuthorService,
    UserService,
    PrintingEditionService,
    DataService,
    OrderService,
    AuthGuard,
    PaymentService
  ],
  entryComponents: [RemoveDialogComponent, CartItemsComponent, CartTransactionComponent]
})
export class SharedModule { }
