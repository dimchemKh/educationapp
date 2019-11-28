import { NgModule } from '@angular/core';
import { SignUpComponent } from 'src/app/account/sign-up/sign-up.component';
import { SignInComponent } from 'src/app/account/sign-in/sign-in.component';
import { ConfirmEmailComponent } from 'src/app/account/confirm-email/confirm-email.component';
import { ForgotPasswordComponent } from 'src/app/account/forgot-password/forgot-password.component';
import { RouterModule } from '@angular/router';
import { routes } from 'src/app/account/account-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { ValidationPatterns } from 'src/app/shared/constants/validation-patterns';
import { MaterialModule } from 'src/app/material.module';
import { MaterialFileInputModule } from 'ngx-material-file-input';

@NgModule({
  declarations: [
    SignUpComponent,
    SignInComponent,
    ConfirmEmailComponent,
    ForgotPasswordComponent
  ],
  imports: [
    RouterModule.forChild(routes),
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialFileInputModule,
    CommonModule,
    FontAwesomeModule,
  ],
  providers: [
    ValidationPatterns
  ],
  schemas: []

})
export class AccountModule { }
