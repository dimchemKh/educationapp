import { Component, Inject } from '@angular/core';
import { FormControl, Validators, FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { AuthorsComponent } from '../authors.component';
import { AuthorModelItem } from 'src/app/shared/models/authors/AuthorModelItem';


@Component({
  selector: 'app-authors-dialog',
  templateUrl: './authors-dialog.component.html',
  styleUrls: ['./authors-dialog.component.scss']
})
export class AuthorsDialogComponent {

  isExistedId = false;
  form: FormGroup;

  get isValidName(): boolean {
    return this.form.valid;
  }

  constructor(private fb: FormBuilder, public dialogRef: MatDialogRef<AuthorsComponent>,
              @Inject(MAT_DIALOG_DATA) public data: AuthorModelItem) {
    this.form = this.fb.group({
      authorName: new FormControl('', [Validators.required])
    });
  }

  close() {
    this.dialogRef.close(this.isExistedId);
  }
}
