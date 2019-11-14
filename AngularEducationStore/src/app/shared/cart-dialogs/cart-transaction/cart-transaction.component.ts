import { Component, OnInit, Inject, ViewChild, AfterViewInit } from '@angular/core';
import { PaymentService } from 'src/app/shared/services/payment.service';
import { StripeService, Elements, Element as StripeElement, ElementsOptions, StripeCardComponent, ElementOptions } from 'ngx-stripe';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { PrintingEditionsComponent } from 'src/app/printing-edition/printing-editions/printing-editions.component';


@Component({
  selector: 'app-cart-transaction',
  templateUrl: './cart-transaction.component.html',
  styleUrls: ['./cart-transaction.component.scss']
})
export class CartTransactionComponent implements OnInit {

  // @ViewChild(StripeCardComponent, { static: false }) card: StripeCardComponent;
 
  elements: Elements;
  card: StripeElement;

  cardOptions: ElementOptions = {
    iconStyle: 'solid',
    style: {
      base: {
        iconColor: '#c4f0ff',
        color: '#fff',
        fontWeight: 500,
        fontFamily: 'Roboto, Open Sans, Segoe UI, sans-serif',
        fontSize: '16px',
        fontSmoothing: 'antialiased',

        ':-webkit-autofill': {
          color: '#fce883',
        },
        '::placeholder': {
          color: '#87BBFD',
        },
      },
      invalid: {
        iconColor: '#FFC7EE',
        color: '#FFC7EE',
      },
    },
  };
 
  elementsOptions: ElementsOptions = {
    locale: 'auto',
    fonts: [{
      cssSrc: 'https://fonts.googleapis.com/css?family=Roboto'
    }]
  };

  stripe: FormGroup;

  constructor(private dialogRef: MatDialogRef<PrintingEditionsComponent>, @Inject(MAT_DIALOG_DATA) public data: any,
              private paymentService: PaymentService, private fb: FormBuilder, private stripeService: StripeService) {

  }

  ngOnInit() {
    this.stripeService.setKey('pk_test_tlcMD8vu8ttNtVSH6RF3OAkp004sTIYGEr');
    this.stripe = this.fb.group({
      email: ['', [Validators.required]]
    });
    this.stripeService.elements(this.elementsOptions)
      .subscribe(elements => {
        this.elements = elements;
        // Only mount the element the first time
        if (!this.card) {
          this.card = this.elements.create('card', {
            style: {
              base: {
                iconColor: '#666EE8',
                color: '#31325F',
                lineHeight: '40px',
                fontWeight: 300,
                fontFamily: '"Helvetica Neue", Helvetica, sans-serif',
                fontSize: '18px',
                '::placeholder': {
                  color: '#CFD7E0'
                }
              }
            }
          });
          this.card.mount('#card-element');
        }
      });
  }
  close() {
    this.dialogRef.close();
  }
  buy() {
    const email = this.stripe.get('email').value;
    this.stripeService
      .createToken(this.card, { name })
      .subscribe(result => {
        if (result.token) {
          // Use the token to create a charge or a customer
          // https://stripe.com/docs/charges
          console.log(result.token);
        } else if (result.error) {
          // Error creating the token
          console.log(result.error.message);
        }
      });
  }
}
