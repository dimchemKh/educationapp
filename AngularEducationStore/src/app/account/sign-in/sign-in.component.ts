import { FormControl, Validators } from '@angular/forms';
import { UserLoginModel } from 'src/app/shared/models/user/UserLoginModel';
import { AccountService } from 'src/app/shared/services';
import { UserRequestModel } from 'src/app/shared/models/user/UserRequestModel';
import { faUser, IconDefinition } from '@fortawesome/free-solid-svg-icons';
import { ValidationPatterns } from 'src/app/shared/constants/validation-patterns';
import { AuthHelper } from 'src/app/shared/helpers/auth-helper';
import { Component } from '@angular/core';
import { AuthModel } from 'src/app/shared/models/auth/auth.model';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.scss']
})


export class SignInComponent {

  readonly title = 'SignIn';
  userIcon: IconDefinition;
  userModel: UserLoginModel;
  userRequestModel: UserRequestModel;
  hide: boolean;
  email: FormControl;
  password: FormControl;
  checkedRemember: boolean;

  constructor(
    private accountService: AccountService,
    private patterns: ValidationPatterns,
    private authHelper: AuthHelper
  ) {
    this.userIcon = faUser;
    this.userModel = new UserLoginModel();
    this.userRequestModel = new UserRequestModel();
    this.hide = true;
    this.checkedRemember = false;
    this.initControls();
  }

  private initControls(): void {
    this.email = new FormControl(null,
      [
        Validators.required,
        Validators.pattern(this.patterns.emailPattern)
      ]);
    this.password = new FormControl(null, Validators.required);
  }

  submit(model: UserLoginModel): void {
    if (!this.email.invalid && !this.password.invalid) {
      this.accountService.signInUser(model).subscribe((data: AuthModel) => {          
          this.authHelper.login(data);
          this.authHelper.isRemember(this.checkedRemember);
        });
    }
  }  
}
