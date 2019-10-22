import { Component } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { AccountService } from 'src/app/services/account.service';
import { UserLoginModel } from 'src/app/models/user/UserLoginModel';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';
import { DataService } from 'src/app/services/data.service';



@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.scss']
})
export class SignInComponent {

  title = 'SignIn';

  constructor(private accountService: AccountService, private dataService: DataService, private authService: AuthService) {

  }

  // tslint:disable-next-line: max-line-length
  email = new FormControl('', [Validators.required, Validators.pattern(/^[a-zA-Z]{1}[a-zA-Z0-9.\-_]*@[a-zA-Z]{1}[a-zA-Z.-]*[a-zA-Z]{1}[.][a-zA-Z]{2,4}$/)]);
  hide = true;
  checked = false;

  userModel: UserLoginModel = new UserLoginModel();

  submit() {
    if (this.getErrorMessage() === '') {
      this.accountService.signInUser(this.userModel).subscribe(
        (data: UserLoginModel) =>
         this.setModel(data)
        );
    }
  }
  getErrorMessage() {
    return this.email.hasError('email') ? 'Not a valid email' :
      this.email.hasError('required') ? 'Empty email' :
      this.email.hasError('pattern') ? 'Not a valid email' : '';
  }
  setModel(userModel: UserLoginModel) {
    // this.userModel.errors = userModel.errors;
    if (userModel.errors === null) {
      this.userModel.userName = userModel.userName;
      this.dataService.setUserInfo(this.userModel.userName);
    }
    this.userModel.errors = userModel.errors;
  }
}
