import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from 'src/app/shared/components/header/header.component';
import { FooterComponent } from 'src/app/shared/components/footer/footer.component';
import { routes } from 'src/app/shared/shared-routing.module';
import { RouterModule } from '@angular/router';
import { AccountService } from 'src/app/shared/services/account.service';
import { FontAwesomeModule  } from '@fortawesome/angular-fontawesome';
import {
  MatToolbarModule,
  MatMenuModule,
  MatButtonModule,
  MatDialogModule,
  MatBadgeModule
} from '@angular/material';
import { AuthorService } from 'src/app/shared/services/author.service';
import { PrintingEditionService } from 'src/app/shared/services/printing-edition.service';
import { UserService } from 'src/app/shared/services/user.service';
import { DataService } from 'src/app/shared/services/data.service';
import { AuthGuard } from 'src/app/shared/guards/auth.guard';
import { RemoveDialogComponent } from 'src/app/shared/components/remove-dialog/remove-dialog.component';
import { OrderService } from 'src/app/shared/services/order.service';



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
    MatDialogModule,
    MatBadgeModule
  ],
  bootstrap: [],
  providers: [
    AccountService,
    AuthorService,
    UserService,
    PrintingEditionService,
    DataService,
    OrderService,
    AuthGuard
  ],
  entryComponents: [RemoveDialogComponent]
})
export class SharedModule { }
