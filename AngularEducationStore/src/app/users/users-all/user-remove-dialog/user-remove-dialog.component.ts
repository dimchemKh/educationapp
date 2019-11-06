import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { UsersAllComponent } from '../users-all.component';
import { UserUpdateModel } from 'src/app/shared/models/user/UserUpdateModel';
import { UserService } from 'src/app/shared/services/user.service';
import { faExclamationCircle } from '@fortawesome/free-solid-svg-icons';
import { from } from 'rxjs';

@Component({
  selector: 'app-user-remove-dialog',
  templateUrl: './user-remove-dialog.component.html',
  styleUrls: ['./user-remove-dialog.component.scss']
})
export class UserRemoveDialogComponent implements OnInit {

  alertIcon = faExclamationCircle;

  // tslint:disable-next-line: max-line-length
  constructor(public dialogRef: MatDialogRef<UsersAllComponent>, @Inject(MAT_DIALOG_DATA) public data: UserUpdateModel, private userService: UserService) { }
  
  ngOnInit() {

  }
  submit() {
    this.userService.removeUser(this.data.id).subscribe();
    this.dialogRef.close();
  }
  close() {
    this.dialogRef.close();
  }
}
