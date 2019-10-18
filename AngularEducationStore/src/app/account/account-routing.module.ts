import { NgModule } from '@angular/core';
import { Routes } from '@angular/router';
import { SignUpComponent } from './sign-up/sign-up.component';
import { SignInComponent } from './sign-in/sign-in.component';
import { RefreshTokenComponent } from './refresh-token/refresh-token.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { ConfirmEmailComponent } from './confirm-email/confirm-email.component';

export const routes: Routes = [
    { path: 'signUp', component: SignUpComponent },
    { path: 'signIn', component: SignInComponent },
    { path: 'refresh', component: RefreshTokenComponent },
    { path: 'forgotPassword', component: ForgotPasswordComponent },
    { path: 'confirmEmail', component: ConfirmEmailComponent }
];

@NgModule({
    imports: [],
    exports: [],
    declarations: [],
})
export class AccountRoutingModule { }
