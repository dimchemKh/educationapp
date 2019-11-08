import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { faUser } from '@fortawesome/free-solid-svg-icons';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { ValidationPatterns } from 'src/app/shared/constants/validation-patterns';
import { UsersAllComponent } from '../users-all.component';
import { UserUpdateModel } from 'src/app/shared/models/user/UserUpdateModel';


@Component({
  selector: 'app-user-edit-dialog',
  templateUrl: './user-edit-dialog.component.html',
  styleUrls: ['./user-edit-dialog.component.scss']
})
export class UserEditDialogComponent {


  isExistedId = false;
  userIcon = faUser;
  form: FormGroup;

  get isValidForm(): boolean {
    return this.form.valid;
  }

  userId: number;

  constructor(public dialogRef: MatDialogRef<UsersAllComponent>, @Inject(MAT_DIALOG_DATA) public data: UserUpdateModel,
              private fb: FormBuilder, private patterns: ValidationPatterns) {
    this.form = this.fb.group({
      firstName: new FormControl(this.data.firstName, [Validators.pattern(this.patterns.namePattern), Validators.required]),
      lastName: new FormControl(this.data.lastName, [Validators.pattern(this.patterns.namePattern), Validators.required]),
    });
  }
  close() {
    this.dialogRef.close(this.isExistedId);
  }
}
