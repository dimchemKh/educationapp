import { Component } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { BaseModel, UserLoginModel } from 'src/app/shared/models';
import { AccountService } from 'src/app/shared/services';
import { ValidationPatterns } from 'src/app/shared/constants/validation-patterns';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent {

  userModel: UserLoginModel;
  responseModel: BaseModel;
  isSuccessForgot: boolean;
  emailForm: FormControl;

  constructor(
    private accountService: AccountService,
    private patterns: ValidationPatterns
  ) {
    this.userModel = new UserLoginModel();
    this.responseModel = new BaseModel();
    this.isSuccessForgot = false;
    this.initCountols();
  }

  private initCountols(): void {
    this.emailForm = new FormControl(null,
      [
        Validators.required,
        Validators.pattern(this.patterns.emailPattern)
      ]);
  }

  submit(userModel: UserLoginModel): void {
    if (!this.emailForm.invalid) {
      this.accountService.forgotPassword(userModel).subscribe(
        (data: BaseModel) => {
          this.responseModel = data;
          this.getErrorsFromApi();
        });
    }
  }

  private getErrorsFromApi(): void {
    if (this.responseModel.errors.length > 0) {
      return;
    }
    this.isSuccessForgot = true;
  }
}
