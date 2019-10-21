import { NgModule } from '@angular/core';
import { Routes } from '@angular/router';

import { CreateAuthorComponent } from 'src/app/author/create-author/create-author.component';
import { DeleteAuthorComponent } from 'src/app/author/delete-author/delete-author.component';
import { UpdateAuthorComponent } from 'src/app/author/update-author/update-author.component';
import { GetAuthorsComponent } from 'src/app/author/get-authors/get-authors.component';

export const routes: Routes = [
  { path: 'create', component: CreateAuthorComponent },
  { path: 'delete', component: DeleteAuthorComponent },
  { path: 'update', component: UpdateAuthorComponent },
  { path: 'get', component: GetAuthorsComponent }
];

@NgModule({
  imports: [],
  exports: []
})

export class AuthorRoutingModule { }
