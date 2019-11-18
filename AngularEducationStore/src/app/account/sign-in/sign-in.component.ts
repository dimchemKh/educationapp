import { Component } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';

import { UserLoginModel } from 'src/app/shared/models/user/UserLoginModel';

import { AccountService, DataService } from 'src/app/shared/services';
import { UserRequestModel } from 'src/app/shared/models/user/UserRequestModel';
import { faUser } from '@fortawesome/free-solid-svg-icons';
import { ValidationPatterns } from 'src/app/shared/constants/validation-patterns';


@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.scss']
})
export class SignInComponent {
  title = 'SignIn';
  userIcon = faUser;

  userModel: UserLoginModel = new UserLoginModel();
  userRequest = new UserRequestModel();
  
  constructor(private accountService: AccountService, private patterns: ValidationPatterns, private dataService: DataService) {
  }

  email = new FormControl('',
    [
      Validators.required,
      Validators.pattern(this.patterns.emailPattern)
    ]);
  password = new FormControl('', Validators.required);
  hide = true;
  checked = false;

  submit(model: UserLoginModel) {
    if (!this.email.invalid && !this.password.invalid) {
      this.accountService.signInUser(model)
        .subscribe((data: UserRequestModel) => {
          this.userRequest = data;
          this.checkErrors();
        });
    }
  }
  checkErrors() {
    if (this.userRequest.errors.length > 0) {
      return;
    }
    if (!this.userRequest.userName || !this.userRequest.userRole) {
      this.userRequest.errors.push('Sorry');
    }
    this.dataService.setLocalStorage('userName', this.userRequest.userName);
    this.dataService.setLocalStorage('userRole', this.userRequest.userRole);

    this.accountService.signIn();
  }
}
