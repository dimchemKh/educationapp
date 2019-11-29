import { Component } from '@angular/core';
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

  diameter: number;
  isLoading: boolean;
  hidePassword: boolean;
  hideConfirmPassword: boolean;
  checked: boolean;
  form: FormGroup;
  fileToUpload: File;
  image: SafeUrl;
  userModel: UserRegistrationModel;
  baseModel: BaseModel;
  isSuccessSignUp: boolean;

  constructor(private accountService: AccountService,
    private patterns: ValidationPatterns,
    private dataService: DataService,
    private fb: FormBuilder,
    private sanitizer: DomSanitizer
    ) {
    this.diameter = 25;
    this.isLoading = false;
    this.hidePassword = true;
    this.hideConfirmPassword = true;
    this.checked = false;
    this.fileToUpload = null;
    this.userModel = new UserRegistrationModel();
    this.baseModel = new BaseModel();
    this.initFormGroup();
  }

  private initFormGroup(): void {
    this.form = this.fb.group({
      firstName: new FormControl(null, [Validators.required, Validators.pattern(this.patterns.namePattern)]),
      lastName: new FormControl(null, [Validators.required, Validators.pattern(this.patterns.namePattern)]),
      email: new FormControl(null,
        [
          Validators.required,
          Validators.pattern(this.patterns.emailPattern)
        ]),
      password: new FormControl(null, [Validators.required]),
      confirmPassword: new FormControl(null, [Validators.required])
    });
  }

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

  onFileChange(files: FileList): void {
    this.fileToUpload = files.item(0);

    if (files && this.fileToUpload) {
      let reader = new FileReader();

      reader.onload = this.handleReaderLoaded.bind(this);

      reader.readAsBinaryString(this.fileToUpload);
    }
  }

  private handleReaderLoaded(readerEvt): void {
    let binaryString = readerEvt.target.result;

    this.userModel.image = 'data:image/jpeg;base64,' + btoa(binaryString);

    let objectURL = 'data:image/jpeg;base64,' + btoa(binaryString);

    this.image = this.sanitizer.bypassSecurityTrustUrl(objectURL);
  }

  submit(userModel: UserRegistrationModel): void {
    if (!this.form.invalid) {
      this.isLoading = true;

      this.accountService.signUpUser(userModel).subscribe(
        (data: BaseModel) => {
          this.baseModel = data;
          this.checkErrors();
        });
    }
  }
  
  private checkErrors(): void {
    if (this.baseModel.errors.length === 0) {
      this.dataService.setLocalStorage('confirmUserName', this.form.controls.firstName.value + ' ' + this.form.controls.lastName.value);
      this.isSuccessSignUp = true;
    }
    this.isLoading = false;
  }
}
