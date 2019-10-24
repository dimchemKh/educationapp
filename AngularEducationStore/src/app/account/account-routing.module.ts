import { NgModule } from '@angular/core';
import { Routes } from '@angular/router';
import { SignUpComponent } from 'src/app/account/sign-up/sign-up.component';
import { SignInComponent } from 'src/app/account/sign-in/sign-in.component';
import { RefreshTokenComponent } from 'src/app/account/refresh-token/refresh-token.component';
import { ForgotPasswordComponent } from 'src/app/account/forgot-password/forgot-password.component';
import { ConfirmEmailComponent } from 'src/app/account/confirm-email/confirm-email.component';
import { AuthGuardService } from '../shared/services/auth.guard.service';

export const routes: Routes = [
    { path: 'signUp', component: SignUpComponent },
    { path: 'signIn', component: SignInComponent },
    { path: 'refresh', component: RefreshTokenComponent, canActivate: [AuthGuardService] },
    { path: 'forgotPassword', component: ForgotPasswordComponent },
    { path: 'confirmEmail', component: ConfirmEmailComponent }
];

@NgModule({
    imports: [],
    exports: [],
    declarations: [],
})
export class AccountRoutingModule { }
