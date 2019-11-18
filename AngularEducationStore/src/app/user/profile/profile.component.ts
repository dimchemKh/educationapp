import { Component, OnInit } from '@angular/core';
import { faUser } from '@fortawesome/free-solid-svg-icons';
import { faEdit } from '@fortawesome/free-solid-svg-icons';
import { UserService } from 'src/app/shared/services/user.service';
import { UserUpdateModel } from 'src/app/shared/models/user/UserUpdateModel';
import { FormControl, Validators, FormBuilder, FormGroup } from '@angular/forms';
import { ValidationPatterns } from 'src/app/shared/constants/validation-patterns';
import { DataService } from 'src/app/shared/services/data.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  userIcon = faUser;
  editIcon = faEdit;

  hidePassword = true;
  hideConfirmPassword = true;
  isDisabled = true;
  existedErrors = false;
  userUpdateModel = new UserUpdateModel();
  form: FormGroup;


  constructor(private userService: UserService, private fb: FormBuilder,
              private patterns: ValidationPatterns, private dataService: DataService) {
    this.form = this.fb.group({
      firstName: new FormControl(this.userUpdateModel.firstName, Validators.pattern(this.patterns.namePattern)),
      lastName: new FormControl(this.userUpdateModel.lastName, Validators.pattern(this.patterns.namePattern)),
      email: new FormControl(this.userUpdateModel.email, Validators.pattern(this.patterns.emailPattern)),
      currentPassword: new FormControl(this.userUpdateModel.currentPassword, Validators.required),
      newPassword: new FormControl(this.userUpdateModel.newPassword)
    });
  }

  ngOnInit() {
    this.userService.getUserOne().subscribe((data: UserUpdateModel) => {
      if (data.errors.length > 0) {
        this.userUpdateModel.errors = data.errors;
        this.existedErrors = true;
        return;
      }
      this.userUpdateModel = data;
      this.dataService.setLocalStorage('user', JSON.stringify(this.userUpdateModel));
    });
  }
  submit(userModel: UserUpdateModel) {
    if (!this.form.invalid) {
      this.userService.updateUser(userModel).subscribe((data: UserUpdateModel) => {
        if (data.errors.length > 0) {
          this.userUpdateModel.errors = data.errors;
          this.existedErrors = true;
          return;
        }
        this.existedErrors = false;
        const userName = this.userUpdateModel.firstName.concat(' ', this.userUpdateModel.lastName);
        this.dataService.setLocalStorage('userName', userName);
        this.changeTemplate();
      });
    }
  }
  close() {
    let tempUser: UserUpdateModel = JSON.parse(this.dataService.getLocalStorage('user'));
    this.userUpdateModel.firstName = tempUser.firstName;
    this.userUpdateModel.lastName = tempUser.lastName;
    this.userUpdateModel.email = tempUser.email;
    this.changeTemplate();
  }
  changeTemplate() {
    this.isDisabled = !this.isDisabled;
  }
  // tslint:disable-next-line: use-lifecycle-interface
  ngOnDestroy(): void {
    this.dataService.deleteItemLocalStorage('user');
  }
}
