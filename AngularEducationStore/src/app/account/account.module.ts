import { NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { SignUpComponent } from 'src/app/account/sign-up/sign-up.component';
import { SignInComponent } from 'src/app/account/sign-in/sign-in.component';
import { ConfirmEmailComponent } from 'src/app/account/confirm-email/confirm-email.component';
import { RefreshTokenComponent } from 'src/app/account/refresh-token/refresh-token.component';
import { ForgotPasswordComponent } from 'src/app/account/forgot-password/forgot-password.component';
import { RouterModule } from '@angular/router';
import { routes } from 'src/app/account/account-routing.module';
import { MaterialModule } from '../material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { from } from 'rxjs';
import { CommonModule } from '@angular/common';
import { AccountService } from '../services/account.service';

@NgModule({
  declarations: [
    SignUpComponent,
    SignInComponent,
    ConfirmEmailComponent,
    RefreshTokenComponent,
    ForgotPasswordComponent
  ],
  imports: [
    RouterModule.forChild(routes),
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    CommonModule
  ],
  providers: [AccountService],
  schemas: [NO_ERRORS_SCHEMA]

})
export class AccountModule { }
