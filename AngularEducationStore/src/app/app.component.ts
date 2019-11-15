import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  title = 'AngularEducationStore';

  ngOnInit() {
    this.loadStripe();
  }
  loadStripe() {
     
    if (!window.document.getElementById('stripe-script')) {
      let stripe = window.document.createElement('script');
      stripe.id = 'stripe-script';
      stripe.type = 'text/javascript';
      stripe.src = 'https://checkout.stripe.com/checkout.js';
      window.document.body.appendChild(stripe);
    }
}
}

