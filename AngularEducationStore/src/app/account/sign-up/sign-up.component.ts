import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.scss']
})
export class SignUpComponent implements OnInit {

  constructor() { }
  email = new FormControl('', [Validators.required, Validators.email]);
  hide = true;
  checked = false;

  ngOnInit() {
  }

  getErrorMessage() {
    return this.email.hasError('email') ? 'Not a valid email' : '';
  }
}
