import { Routes } from '@angular/router';
import { SignUpComponent } from 'src/app/account/sign-up/sign-up.component';
import { SignInComponent } from 'src/app/account/sign-in/sign-in.component';
import { ForgotPasswordComponent } from 'src/app/account/forgot-password/forgot-password.component';
import { ConfirmEmailComponent } from 'src/app/account/confirm-email/confirm-email.component';

export const routes: Routes = [
    { path: 'signUp', component: SignUpComponent },
    { path: 'signIn', component: SignInComponent },
    { path: 'forgotPassword', component: ForgotPasswordComponent },
    { path: 'confirmEmail', component: ConfirmEmailComponent },
    { path: 'confirmEmail/:error', component: ConfirmEmailComponent },
];

export class AccountRoutingModule { }
