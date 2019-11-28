import { Component, Inject } from '@angular/core';
import { faCheckSquare, IconDefinition } from '@fortawesome/free-solid-svg-icons';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { OrdersUserComponent } from 'src/app/order/orders-user/orders-user.component';


@Component({
  selector: 'app-cart-success',
  templateUrl: './cart-success.component.html',
  styleUrls: ['./cart-success.component.scss']
})
export class CartSuccessComponent {

  checkIcon: IconDefinition;

  constructor(public dialogRef: MatDialogRef<OrdersUserComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
    ) { 
    this.checkIcon = faCheckSquare;
  }

  submit(): void {
    this.dialogRef.close();
  }
}
