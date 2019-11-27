import { Component, OnInit } from '@angular/core';
import { FormControl, Validators, FormBuilder, FormGroup } from '@angular/forms';
import { UserRegistrationModel } from 'src/app/shared/models/user/UserRegistrationModel';
import { BaseModel } from 'src/app/shared/models/base/BaseModel';
import { AccountService, DataService } from 'src/app/shared/services';
import { faUser } from '@fortawesome/free-solid-svg-icons';
import { ValidationPatterns } from 'src/app/shared/constants/validation-patterns';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.scss']
})
export class SignUpComponent {

  title = 'Create Account';
  userIcon = faUser;

  diameter = 25;
  isLoading = false;
  hidePassword = true;
  hideConfirmPassword = true;
  checked = false;
  form: FormGroup;
  fileToUpload: File = null;
  image: SafeUrl;

  constructor(private accountService: AccountService, private patterns: ValidationPatterns, private dataService: DataService,
              private fb: FormBuilder, private sanitizer: DomSanitizer) {
    this.form = this.fb.group({
      firstName: new FormControl('', [Validators.required, Validators.pattern(this.patterns.namePattern)]),
      lastName: new FormControl('', [Validators.required, Validators.pattern(this.patterns.namePattern)]),
      email: new FormControl('',
        [
          Validators.required,
          Validators.pattern(this.patterns.emailPattern)
        ]),
      password: new FormControl('', [Validators.required]),
      confirmPassword: new FormControl('', [Validators.required])
    });
  }

  userModel = new UserRegistrationModel();
  baseModel = new BaseModel();
  isSuccessSignUp: boolean;

  isControlInvalid(controlName: string): boolean {
    let control = this.form.controls[controlName];

    let result = control.invalid && control.touched;

    return result;
  }
  isSamePasswords(controlName: string): boolean {

    let control = this.form.controls[controlName];

    let result = control.touched && (control.value !== this.form.controls.password.value);
    if (result) {
      this.form.controls.confirmPassword.setErrors(Validators.required);
      return true;
    }
    return false;
  }
  onFileChange(files: FileList) {
    this.fileToUpload = files.item(0);

    if (files && this.fileToUpload) {
      let reader = new FileReader();

      reader.onload = this._handleReaderLoaded.bind(this);

      reader.readAsBinaryString(this.fileToUpload);
    }

  }
  _handleReaderLoaded(readerEvt) {
    let binaryString = readerEvt.target.result;
    this.userModel.image = 'data:image/jpeg;base64,' + btoa(binaryString);
    let objectURL = 'data:image/jpeg;base64,' + btoa(binaryString);
    this.image = this.sanitizer.bypassSecurityTrustUrl(objectURL);
  }

  submit(userModel: UserRegistrationModel) {
    if (!this.form.invalid) {
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
      this.dataService.setLocalStorage('confirmUserName',
        this.form.controls.firstName.value + ' ' + this.form.controls.lastName.value);
      this.isSuccessSignUp = true;
    }
    this.isLoading = false;
  }
}
