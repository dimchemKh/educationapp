import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { faCheckSquare, IconDefinition } from '@fortawesome/free-solid-svg-icons';
import { DataService } from 'src/app/shared/services';

@Component({
  selector: 'app-confirm-email',
  templateUrl: './confirm-email.component.html',
  styleUrls: ['./confirm-email.component.scss']
})
export class ConfirmEmailComponent implements OnInit {

  checkIcon: IconDefinition;
  error: string;

  constructor(private router: Router, private route: ActivatedRoute, private dataService: DataService) {
    this.checkIcon = faCheckSquare;
   }

  ngOnInit() {
    this.error = this.route.snapshot.queryParamMap.get('error');
  }
  get userName(): string {
    return this.dataService.getLocalStorage('confirmUserName');
  }
  submit() {
    this.dataService.clearLocalStorage();
    this.router.navigate(['/account/signIn']);
  }
}
