import { NgModule } from '@angular/core';
import { Routes } from '@angular/router';

import { AuthorsComponent } from 'src/app/author/authors/authors.component';


export const routes: Routes = [
  { path: 'get', component: AuthorsComponent }

];

@NgModule({
  imports: [],
  exports: []
})

export class AuthorRoutingModule { }
