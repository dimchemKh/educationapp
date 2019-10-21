import { Component } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { AccountService } from 'src/app/services/account.service';
import { UserLoginModel } from 'src/app/models/user/UserLoginModel';



@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.scss']
})
export class SignInComponent {


  constructor(private accountService: AccountService) {
  }
  email = new FormControl('', [Validators.required, Validators.email]);
  hide = true;
  checked = false;

  user: UserLoginModel = new UserLoginModel();
  title = 'SignIn';

  submit() {
    this.accountService.signInUser(this.user).subscribe((data: string) => console.log(data));
  }

  getErrorMessage() {
    return this.email.hasError('email') ? 'Not a valid email' : '';
  }

}
