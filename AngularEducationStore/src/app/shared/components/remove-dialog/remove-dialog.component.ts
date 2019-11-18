import { Component, Inject } from '@angular/core';
import { faExclamationCircle } from '@fortawesome/free-solid-svg-icons';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { RemoveModel } from 'src/app/shared/models';


@Component({
  selector: 'app-remove-dialog',
  templateUrl: './remove-dialog.component.html',
  styleUrls: ['./remove-dialog.component.scss']
})

export class RemoveDialogComponent {

  isExistedId = false;

  alertIcon = faExclamationCircle;

  constructor(@Inject(MAT_DIALOG_DATA) public data: RemoveModel, public dialogRef: MatDialogRef<RemoveDialogComponent>) {

  }

  close() {
    this.dialogRef.close(this.isExistedId);
  }
}
