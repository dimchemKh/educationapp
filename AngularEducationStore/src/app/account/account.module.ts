import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SignUpComponent } from './sign-up/sign-up.component';
import { SignInComponent } from './sign-in/sign-in.component';
import { ConfirmEmailComponent } from './confirm-email/confirm-email.component';
import { RefreshTokenComponent } from './refresh-token/refresh-token.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { RouterModule } from '@angular/router';
import { routes } from 'src/app/account/account-routing.module';


@NgModule({
  imports: [
    RouterModule.forChild(routes),
    CommonModule
  ],
  declarations: [SignUpComponent, SignInComponent, ConfirmEmailComponent, RefreshTokenComponent, ForgotPasswordComponent]

})
export class AccountModule { }
