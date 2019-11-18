import { Injectable } from '@angular/core';
import { PaymentModel } from '../models/payment/PaymentModel';
import { MatDialog } from '@angular/material';
import { CartSuccessComponent } from '../cart-dialogs/cart-success/cart-success.component';



@Injectable({
  providedIn: 'root'
})

export class PaymentService {

  

  constructor(private dialog: MatDialog) {
  }

  openStripeDialog(payment: PaymentModel, updateMethod: Promise<object>) {
    let handler = (window as any).StripeCheckout.configure({
      key: 'pk_test_tlcMD8vu8ttNtVSH6RF3OAkp004sTIYGEr',
      locale: 'auto',
      token: (token: any) => {
        payment.transactionId = token.id;
        updateMethod.then();
      }
    });

    handler.open({
      name: 'Localhost',
      description: 'Payment description',
      closed: () => {
          this.dialog.open(CartSuccessComponent, {
          data: {
            orderId: payment.orderId
          }
        });
      }
    });
  }
}
