import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { BaseModel, UserLoginModel } from 'src/app/shared/models';
import { AccountService } from 'src/app/shared/services';
import { ValidationPatterns } from 'src/app/shared/constants/validation-patterns';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent implements OnInit {

  constructor(private accountService: AccountService, private patterns: ValidationPatterns) { }

  userModel = new UserLoginModel();
  responseModel = new BaseModel();
  isSuccessForgot = false;

  emailForm = new FormControl('',
    [
      Validators.required,
      Validators.pattern(this.patterns.emailPattern)
    ]);
  submit(userModel: UserLoginModel) {
    if (!this.emailForm.invalid) {
      this.accountService.forgotPassword(userModel)
        .subscribe(
          (data: BaseModel) => {
            this.responseModel = data;
            this.getErrorsFromApi();
          });
    }
  }

  ngOnInit() {
  }
  getErrorsFromApi() {
    if (this.responseModel.errors.length > 0) {
      return;
    }
    this.isSuccessForgot = true;
  }
}
