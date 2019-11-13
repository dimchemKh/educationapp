import { Component, Inject } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { AuthorsComponent } from '../authors.component';


@Component({
  selector: 'app-authors-dialog',
  templateUrl: './authors-dialog.component.html',
  styleUrls: ['./authors-dialog.component.scss']
})
export class AuthorsDialogComponent {

  isExistedId = false;

  authorName = new FormControl('', [Validators.required]);

  get isValidName(): boolean {
    return this.authorName.valid;
  }

  constructor(public dialogRef: MatDialogRef<AuthorsComponent>,
              @Inject(MAT_DIALOG_DATA) public data: any) {

  }

  close() {
    this.dialogRef.close(this.isExistedId);
  }
}
