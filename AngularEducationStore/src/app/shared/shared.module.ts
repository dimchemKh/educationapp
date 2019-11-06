import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';
import { routes } from 'src/app/shared/shared-routing.module';
import { RouterModule } from '@angular/router';
import { AccountService } from './services/account.service';
import { FontAwesomeModule  } from '@fortawesome/angular-fontawesome';
import {
  MatToolbarModule,
  MatMenuModule,
  MatButtonModule
} from '@angular/material';
import { AuthorService } from './services/author.service';
import { PrintingEditionService } from './services/printing-edition.service';
import { LoaderService } from './services/loader.service';
import { UserService } from './services/user.service';
import { DataService } from './services/data.service';
import { AuthGuard } from './guards/auth.guard';



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
    FontAwesomeModule,
    MatButtonModule
  ],
  bootstrap: [],
  providers: [
    AccountService,
    AuthorService,
    UserService,
    PrintingEditionService,
    LoaderService,
    DataService,
    AuthGuard
  ]
})
export class SharedModule { }
