import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { faUser, IconDefinition } from '@fortawesome/free-solid-svg-icons';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { ValidationPatterns } from 'src/app/shared/constants/validation-patterns';
import { UsersAllComponent } from 'src/app/user/users-all/users-all.component';
import { UserUpdateModel } from 'src/app/shared/models/user/UserUpdateModel';


@Component({
  selector: 'app-user-edit-dialog',
  templateUrl: './user-edit-dialog.component.html',
  styleUrls: ['./user-edit-dialog.component.scss']
})
export class UserEditDialogComponent {

  isExistedId: boolean;
  userIcon: IconDefinition;
  form: FormGroup;
  userId: number;

  constructor(public dialogRef: MatDialogRef<UsersAllComponent>,
    @Inject(MAT_DIALOG_DATA) public data: UserUpdateModel,
    private fb: FormBuilder, private patterns: ValidationPatterns
    ) {
    this.userIcon = faUser;
    this.isExistedId = false;
    this.initFormGroup();
  }

  get isValidForm(): boolean {
    return this.form.valid;
  }
  
  initFormGroup(): void {
    this.form = this.fb.group({
      firstName: new FormControl(this.data.firstName, [Validators.pattern(this.patterns.namePattern), Validators.required]),
      lastName: new FormControl(this.data.lastName, [Validators.pattern(this.patterns.namePattern), Validators.required]),
    });
  }

  close(): void {
    this.dialogRef.close(this.isExistedId);
  }
}
