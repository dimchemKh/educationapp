import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RouterModule } from '@angular/router';
import { routes } from 'src/app/author/author-routing.module';
import { from } from 'rxjs';
import { GetAuthorsComponent } from './get-authors/get-authors.component';
import { CreateAuthorComponent } from './create-author/create-author.component';
import { UpdateAuthorComponent } from './update-author/update-author.component';
import { DeleteAuthorComponent } from './delete-author/delete-author.component';

@NgModule({
  declarations: [
    GetAuthorsComponent,
    CreateAuthorComponent,
    UpdateAuthorComponent,
    DeleteAuthorComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes)
  ]
})
export class AuthorModule { }
