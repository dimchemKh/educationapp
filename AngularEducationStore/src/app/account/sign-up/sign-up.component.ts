import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { UserRegistrationModel } from 'src/app/models/user/UserRegistrationModel';
import { BaseModel } from 'src/app/models/base/BaseModel';
import { Router } from '@angular/router';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.scss']
})
export class SignUpComponent implements OnInit {

  title = 'Create Account';
  hidePassword = true;
  hideConfirmPassword = true;
  checked = false;

  constructor(private router: Router) {

  }

  userModel = new UserRegistrationModel();
  baseModel = new BaseModel();
  isSuccessSignUp: boolean;

  firstName = new FormControl('', [Validators.required, Validators.pattern(/^[a-zA-Z]{3,}$/)]);
  lastName = new FormControl('', [Validators.required, Validators.pattern(/^[a-zA-Z]{3,}$/)]);
  email = new FormControl('',
  [
    Validators.required,
    Validators.pattern(/^[a-zA-Z]{1}[a-zA-Z0-9.\-_]*@[a-zA-Z]{1}[a-zA-Z.-]*[a-zA-Z]{1}[.][a-zA-Z]{2,4}$/)
  ]);
  password = new FormControl('', [Validators.required]);
  confirmPassword = new FormControl('', [Validators.required]);

  ngOnInit() {
  }

  getFirstNameErrorMessage() {
    return this.firstName.touched && this.firstName.hasError('required') ? 'Please enter your FirstName' :
           this.firstName.hasError('pattern') ? 'Invalid FirstName!' : '';
  }
  getLastNameErrorMessage() {
    return this.lastName.touched && this.lastName.hasError('required') ? 'Please enter your LastName' :
           this.lastName.hasError('pattern') ? 'Invalid LastName!' : '';
  }
  getEmailErrorMessage() {
    return this.email.hasError('pattern') ? 'Not a valid email' :
          (this.email.hasError('required') && this.email.touched) ? 'Empty field' : '';
  }
  getPasswordErrorMessage() {
    return (this.password.hasError('required') && this.password.touched) ? 'Empty password' : '';
  }
  getConfirmPasswordMessage() {
    return (this.confirmPassword.hasError('required') && this.confirmPassword.touched) ? 'Empty password' :
           (this.confirmPassword.value !== this.password.value && this.confirmPassword.touched) ? 'Not same passwords' : '';
  }
  submit() {
    // if (!this.firstName.invalid && !this.lastName.invalid && !this.email.invalid && this.confirmPassword.value === this.password.value) {
    //   this.accountService.signUpUser(this.userModel).subscribe(
    //     (data: BaseModel) =>
    //       this.mapModel(data)
    //     );
    // }
  }
  mapModel(baseModel: BaseModel) {
    if (baseModel.errors.length === 0) {
      // this.router.navigate(['/account/confirmEmail']);
      this.isSuccessSignUp = true;
      localStorage.setItem('userName', this.firstName.value + ' ' + this.lastName.value);
    }
    if (baseModel.errors.length > 0) {
      this.baseModel.errors = baseModel.errors;
    }
  }
}
