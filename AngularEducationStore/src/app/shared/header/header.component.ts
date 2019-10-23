import { Component, OnInit, OnChanges } from '@angular/core';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  isAuthentificated: boolean;

  constructor(private authService: AuthService) {
  }
  ngOnInit() {
    this.authService.authNavStatus$.subscribe(
      status =>
      this.isAuthentificated = status
    );
  }
  signOut() {
    localStorage.clear();
  }
}
