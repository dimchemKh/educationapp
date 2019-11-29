import { FormControl, Validators } from '@angular/forms';
import { UserLoginModel } from 'src/app/shared/models/user/UserLoginModel';
import { AccountService, DataService } from 'src/app/shared/services';
import { UserRequestModel } from 'src/app/shared/models/user/UserRequestModel';
import { faUser, IconDefinition } from '@fortawesome/free-solid-svg-icons';
import { ValidationPatterns } from 'src/app/shared/constants/validation-patterns';
import { AuthHelper } from 'src/app/shared/helpers/auth-helper';
import { Component } from '@angular/core';
import { AuthModel } from 'src/app/shared/models/auth/auth.model';

const key = 'USER_KEY';

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
    private dataService: DataService,
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
      this.accountService.signInUser(model)
        .subscribe((data: AuthModel) => {          
          this.authHelper.signIn(data);
          // this.signInUser();
          // this.isRemember();
        });
    }
  }

  // private signInUser(): void {
  //   this.dataService.setLocalStorage('userName', this.userRequestModel.userName);

  //   this.dataService.setLocalStorage('userRole', this.userRequestModel.userRole);

  //   if (this.userRequestModel.image) {
  //     this.dataService.setLocalStorage('userImage', this.userRequestModel.image);
  //   }
  // }

  // private isRemember(): void {
  //   let date = new Date();

  //   if (!this.checkedRemember) {
  //     this.dataService.setCookie('expire', 'time', date.setHours(date.getHours() + 12));
  //   }

  //   if (this.checkedRemember) {
  //     this.dataService.setCookie('expire', 'time', date.setMonth(date.getMonth() + 2));
  //   }

  //   this.authHelper.signIn();
  // }
}
