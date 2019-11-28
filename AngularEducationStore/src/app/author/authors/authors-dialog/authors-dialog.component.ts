import { Component, Inject } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { AuthorsComponent } from 'src/app/author/authors/authors.component';

@Component({
  selector: 'app-authors-dialog',
  templateUrl: './authors-dialog.component.html',
  styleUrls: ['./authors-dialog.component.scss']
})
export class AuthorsDialogComponent {

  isExistedId: boolean;
  authorName: FormControl;

  constructor(public dialogRef: MatDialogRef<AuthorsComponent>,
              @Inject(MAT_DIALOG_DATA) public data: any) {
    this.isExistedId = false;
    this.authorName = new FormControl(null, [Validators.required]);
  }
  
  get isValidName(): boolean {
    return this.authorName.valid;
  }
  
  close(): void {
    this.dialogRef.close(this.isExistedId);
  }
}
