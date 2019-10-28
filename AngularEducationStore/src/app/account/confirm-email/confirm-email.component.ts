import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-confirm-email',
  templateUrl: './confirm-email.component.html',
  styleUrls: ['./confirm-email.component.scss']
})
export class ConfirmEmailComponent implements OnInit {

  error: string;
  constructor(private router: Router, private route: ActivatedRoute) { }

  ngOnInit() {
    this.error = this.route.snapshot.queryParamMap.get('error');
  }
  get userName(): string {
    return localStorage.getItem('confirmUserName');
  }
  submit() {
    localStorage.clear();
    this.router.navigate(['/account/signIn']);
  }
}
