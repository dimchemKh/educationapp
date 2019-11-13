import { NgModule } from '@angular/core';
import { SignUpComponent } from 'src/app/account/sign-up/sign-up.component';
import { SignInComponent } from 'src/app/account/sign-in/sign-in.component';
import { ConfirmEmailComponent } from 'src/app/account/confirm-email/confirm-email.component';
import { ForgotPasswordComponent } from 'src/app/account/forgot-password/forgot-password.component';
import { RouterModule } from '@angular/router';
import { routes } from 'src/app/account/account-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatCheckboxModule, MatFormFieldModule, MatButtonModule, MatChipsModule, MatIconModule, MatInputModule, MatProgressSpinnerModule } from '@angular/material';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { ValidationPatterns } from '../shared/constants/validation-patterns';


@NgModule({
  declarations: [
    SignUpComponent,
    SignInComponent,
    ConfirmEmailComponent,
    ForgotPasswordComponent
  ],
  imports: [
    RouterModule.forChild(routes),
    MatCheckboxModule,
    MatButtonModule,
    MatFormFieldModule,
    FormsModule,
    ReactiveFormsModule,
    MatChipsModule,
    MatIconModule,
    MatInputModule,
    CommonModule,
    FontAwesomeModule,
    MatProgressSpinnerModule
  ],
  providers: [
    ValidationPatterns
  ],
  schemas: []

})
export class AccountModule { }
