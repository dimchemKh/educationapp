import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { AuthService } from 'src/app/shared/services/auth.service';
import { BaseModel } from 'src/app/models/base/BaseModel';
import { Router } from '@angular/router';
import { UserLoginModel } from 'src/app/models/user/UserLoginModel';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent implements OnInit {

  constructor(private authService: AuthService, private router: Router) { }
  userModel = new UserLoginModel();
  responseModel = new BaseModel();
  isSuccessForgot = false;

  emailForm = new FormControl('',
  [
    Validators.required,
    Validators.pattern(/^[a-zA-Z]{1}[a-zA-Z0-9.\-_]*@[a-zA-Z]{1}[a-zA-Z.-]*[a-zA-Z]{1}[.][a-zA-Z]{2,4}$/)
  ]);
  submit(userModel: UserLoginModel) {
    debugger
    if (!this.emailForm.invalid) {
      this.authService.responseForgotPassword(userModel)
      .subscribe(
        (data: BaseModel) => {
          this.responseModel = data;
          this.getErrorsFromApi();
        });
    }
  }
  getEmailErrorMessage() {
    return (this.emailForm.hasError('required') && this.emailForm.touched) ? 'Empty field' :
            this.emailForm.hasError('pattern') ? 'Not a valid email' : '';
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
