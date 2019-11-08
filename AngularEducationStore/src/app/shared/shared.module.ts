import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { routes } from 'src/app/shared/shared-routing.module';
import { RouterModule } from '@angular/router';
import { AccountService } from './services/account.service';
import { FontAwesomeModule  } from '@fortawesome/angular-fontawesome';
import {
  MatToolbarModule,
  MatMenuModule,
  MatButtonModule,
  MatDialogModule
} from '@angular/material';
import { AuthorService } from './services/author.service';
import { PrintingEditionService } from './services/printing-edition.service';
import { LoaderService } from './services/loader.service';
import { UserService } from './services/user.service';
import { DataService } from './services/data.service';
import { AuthGuard } from './guards/auth.guard';
import { RemoveDialogComponent } from './components/remove-dialog/remove-dialog.component';



@NgModule({
  declarations: [
    HeaderComponent,
    FooterComponent,
    RemoveDialogComponent
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
    MatButtonModule,
    MatDialogModule
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
  ],
  entryComponents: [RemoveDialogComponent]
})
export class SharedModule { }
