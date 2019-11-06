import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { UserModelItem } from 'src/app/shared/models/user/UserModelItem';
import { faUser } from '@fortawesome/free-solid-svg-icons';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { ValidationPatterns } from 'src/app/shared/constants/validation-patterns';
import { UsersAllComponent } from '../users-all.component';
import { UserService } from 'src/app/shared/services/user.service';
import { UserUpdateModel } from 'src/app/shared/models/user/UserUpdateModel';


@Component({
  selector: 'app-user-edit-dialog',
  templateUrl: './user-edit-dialog.component.html',
  styleUrls: ['./user-edit-dialog.component.scss']
})
export class UserEditDialogComponent implements OnInit {

  userIcon = faUser;
  form: FormGroup;

  userId: number;
  constructor(public dialogRef: MatDialogRef<UsersAllComponent>, @Inject(MAT_DIALOG_DATA) public data: UserUpdateModel,
    private fb: FormBuilder, private patterns: ValidationPatterns, private userService: UserService) {
    this.form = this.fb.group({
      firstName: new FormControl(this.data.firstName, [Validators.pattern(this.patterns.namePattern), Validators.required]),
      lastName: new FormControl(this.data.lastName, [Validators.pattern(this.patterns.namePattern), Validators.required]),
    });
  }
  ngOnInit() {

  }
  submit() {
    if (!this.form.invalid) {
      this.userService.updateUser(this.data).subscribe();
      this.dialogRef.close();
    }
  }
  close() {
    this.dialogRef.close();
  }
}
