import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { UserRegistrationModel } from 'src/app/shared/models/user/UserRegistrationModel';
import { BaseModel } from 'src/app/shared/models/base/BaseModel';
import { Router } from '@angular/router';
import { AccountService } from 'src/app/shared/services/account.service';
import { faUser } from '@fortawesome/free-solid-svg-icons';
import { ValidationPatterns } from 'src/app/shared/constants/validation-patterns';
import { DataService } from 'src/app/shared/services/data.service';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.scss']
})
export class SignUpComponent implements OnInit {

  title = 'Create Account';
  userIcon = faUser;

  diameter = 25;
  isLoading = false;
  hidePassword = true;
  hideConfirmPassword = true;
  checked = false;

  constructor(private accountService: AccountService, private patterns: ValidationPatterns, private dataService: DataService) {

  }

  userModel = new UserRegistrationModel();
  baseModel = new BaseModel();
  isSuccessSignUp: boolean;

  firstName = new FormControl('', [Validators.required, Validators.pattern(this.patterns.namePattern)]);
  lastName = new FormControl('', [Validators.required, Validators.pattern(this.patterns.namePattern)]);
  email = new FormControl('',
    [
      Validators.required,
      Validators.pattern(this.patterns.emailPattern)
    ]);
  password = new FormControl('', [Validators.required]);
  confirmPassword = new FormControl('', [Validators.required]);

  ngOnInit() {
  }

  submit(userModel: UserRegistrationModel) {
    if (!this.firstName.invalid && !this.lastName.invalid && !this.email.invalid && this.confirmPassword.value === this.password.value) {
      this.isLoading = true;
      this.accountService.signUpUser(userModel).subscribe(
        (data: BaseModel) => {
          this.baseModel = data;
          this.checkErrors();
        });
    }
  }
  checkErrors() {
    if (this.baseModel.errors.length === 0) {
      this.dataService.setLocalStorage('confirmUserName', this.firstName.value + ' ' + this.lastName.value);
      this.isSuccessSignUp = true;
    }
    this.isLoading = false;
  }
}
