import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';
import { MaterialModule } from 'src/app/material.module';
import { routes } from 'src/app/shared/shared-routing.module';
import { from } from 'rxjs';
import { RouterModule } from '@angular/router';
import { AuthService } from './services/auth.service';
import { FontAwesomeModule  } from '@fortawesome/angular-fontawesome';
import { MatToolbarModule, MatMenuModule } from '@angular/material';

@NgModule({
  declarations: [
    HeaderComponent,
    FooterComponent
  ],
  exports: [
    HeaderComponent,
    FooterComponent
  ],
  imports: [
    CommonModule,
    MatToolbarModule,
    MatMenuModule,
    RouterModule.forChild(routes),
    FontAwesomeModule
  ],
  bootstrap: [],
  providers: [
    AuthService
  ]
})
export class SharedModule { }
