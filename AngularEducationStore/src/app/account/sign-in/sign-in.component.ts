import { Component } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';

import { UserLoginModel } from 'src/app/models/user/UserLoginModel';

import { AuthService } from 'src/app/shared/services/auth.service';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.scss']
})
export class SignInComponent {

  title = 'SignIn';
  userModel: UserLoginModel = new UserLoginModel();
  constructor(private authService: AuthService) {
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
    debugger
    if (!this.email.invalid && !this.password.invalid) {
      debugger
      this.authService.responseSI(model)
        .subscribe((data: UserLoginModel) => {
          debugger
          localStorage.setItem('userName', data.userName);
          debugger
          this.authService.signIn();
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
  // async mapModel(model: UserLoginModel) {
  //   debugger
  //   this.userModel.errors = model.errors;
  //   if (model.errors.length === 0) {
  //     this.userModel.userName = model.userName;
  //     localStorage.setItem('userName', this.userModel.userName);
  //   }
  // }
}
