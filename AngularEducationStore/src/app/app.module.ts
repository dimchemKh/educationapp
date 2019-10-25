import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from 'src/app/app-routing.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { AccountModule } from 'src/app/account/account.module';
import { AuthorModule } from 'src/app/author/author.module';

import { MatSidenavModule } from '@angular/material';
import { HttpClientModule } from '@angular/common/http';
import { PrintingEditionModule } from 'src/app/printing-edition/printing-edition.module';

import { from } from 'rxjs';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    RouterModule,
    SharedModule,
    MatSidenavModule,
    AccountModule,
    AuthorModule,
    PrintingEditionModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
