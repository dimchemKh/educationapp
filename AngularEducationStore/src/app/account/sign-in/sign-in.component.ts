import { Component } from '@angular/core';
import {
  FormControl,
  Validators } from '@angular/forms';

import { UserLoginModel } from 'src/app/models/user/UserLoginModel';

import { AuthService } from 'src/app/shared/services/auth.service';
import { UserRequestModel } from 'src/app/models/user/UserRequestModel';
import { DataService } from 'src/app/shared/services/data.service';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.scss']
})
export class SignInComponent {
  title = 'SignIn';

  userModel: UserLoginModel = new UserLoginModel();
  userRequest = new UserRequestModel();
  cook: string;
  constructor(private dataService: DataService, private authService: AuthService) {
  }

  email = new FormControl('',
  [
    Validators.required,
    Validators.pattern(/^[a-zA-Z]{1}[a-zA-Z0-9.\-_]*@[a-zA-Z]{1}[a-zA-Z.-]*[a-zA-Z]{1}[.][a-zA-Z]{2,4}$/)
  ]);
  password = new FormControl('', Validators.required);
  hide = true;
  checked = false;

  submit(model: UserLoginModel) {
    if (!this.email.invalid && !this.password.invalid) {
      this.dataService.requestSignIn(model)
        .subscribe((data: UserRequestModel) => {
          this.userRequest = data;
          this.checkErrors();
        });
    }
  }
  getEmailErrorMessage() {
    return (this.email.hasError('required') && this.email.touched) ? 'Empty field' :
            this.email.hasError('pattern') ? 'Not a valid email' : '';
  }
  getPasswordErrorMessage() {
    return (this.password.hasError('required') && this.password.touched) ? 'Empty password' : '';
  }
  checkErrors() {
    if (this.userRequest.errors.length > 0) {
      return;
    }
    localStorage.setItem('userName', this.userRequest.userName);
    localStorage.setItem('userRole', this.userRequest.userRole);
    this.authService.signIn();
  }
}
