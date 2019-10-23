import { Component } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { AccountService } from 'src/app/services/account.service';
import { UserLoginModel } from 'src/app/models/user/UserLoginModel';
import { AuthService } from 'src/app/services/auth.service';
import { DataService } from 'src/app/services/data.service';



@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.scss']
})
export class SignInComponent {

  title = 'SignIn';
  isAuthentificated: boolean;

  constructor(private accountService: AccountService, private dataService: DataService,
              private authService: AuthService) {
    this.authService.authNavStatus$.subscribe(status => this.isAuthentificated = status);
  }
  // tslint:disable-next-line: max-line-length
  email = new FormControl('',
  [
    Validators.required,
    Validators.pattern(/^[a-zA-Z]{1}[a-zA-Z0-9.\-_]*@[a-zA-Z]{1}[a-zA-Z.-]*[a-zA-Z]{1}[.][a-zA-Z]{2,4}$/)
  ]);
  password = new FormControl('', Validators.required);
  hide = true;
  checked = false;
  userModel: UserLoginModel = new UserLoginModel();

  submit() {
    if (this.getEmailErrorMessage() === '' && this.getPasswordErrorMessage() === '') {
    this.accountService.signInUser(this.userModel).subscribe(
      (data: UserLoginModel) =>
        this.mapModel(data)
      );
    }
    // this.authService.login();
  }
  getEmailErrorMessage() {
    return (this.email.hasError('required') && this.email.touched) ? 'Empty field' :
            this.email.hasError('pattern') ? 'Not a valid email' : '';
  }
  getPasswordErrorMessage() {
    return (this.password.hasError('required') && this.password.touched) ? 'Empty password' : '';
  }
  mapModel(userModel: UserLoginModel) {
    if (userModel.errors.length === 0) {
      this.userModel.userName = userModel.userName;
      this.dataService.setUserInfo(this.userModel.userName);
    }
    this.userModel.errors = userModel.errors;
  }
}
