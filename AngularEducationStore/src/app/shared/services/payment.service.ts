import { Injectable } from '@angular/core';
import { PaymentModel } from 'src/app/shared/models';
import { MatDialog } from '@angular/material';
import { CartSuccessComponent } from 'src/app/shared/components/cart-dialogs/cart-success/cart-success.component';
import { OrderService } from 'src/app/shared/services/order.service';



@Injectable({
  providedIn: 'root'
})

export class PaymentService {

  constructor(private dialog: MatDialog, private orderService: OrderService) {
  }

  openStripeDialog(payment: PaymentModel) {

    let handler = (window as any).StripeCheckout.configure({
      key: 'pk_test_tlcMD8vu8ttNtVSH6RF3OAkp004sTIYGEr',
      locale: 'auto',
      token: (token: any) => {
        payment.transactionId = token.id;
        this.orderService.updateOrder(payment).then();
      }
    });

    handler.open({
      name: 'Localhost',
      description: 'Payment description',
      closed: () => {
        if (payment.transactionId) {
          this.dialog.open(CartSuccessComponent, {
            data: {
              orderId: payment.orderId
            }
          });
        }
      }
    });
  }
}
