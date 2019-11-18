import { Component, OnInit, Inject } from '@angular/core';
import { faCheckSquare } from '@fortawesome/free-solid-svg-icons';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { OrdersUserComponent } from 'src/app/order/orders-user/orders-user.component';


@Component({
  selector: 'app-cart-success',
  templateUrl: './cart-success.component.html',
  styleUrls: ['./cart-success.component.scss']
})
export class CartSuccessComponent implements OnInit {

  checkIcon = faCheckSquare;

  constructor(public dialogRef: MatDialogRef<OrdersUserComponent>, @Inject(MAT_DIALOG_DATA) public data: number) { }

  ngOnInit() {
  }

  submit() {
    this.dialogRef.close();
  }
}
