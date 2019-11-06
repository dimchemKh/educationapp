import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RouterModule } from '@angular/router';
import { routes } from 'src/app/author/author-routing.module';


import { CreateAuthorComponent } from './create-author/create-author.component';
import { UpdateAuthorComponent } from './update-author/update-author.component';
import { DeleteAuthorComponent } from './delete-author/delete-author.component';
import { AuthorsComponent } from './authors/authors.component';
import { AuthorService } from '../shared/services/author.service';

@NgModule({
  declarations: [
    CreateAuthorComponent,
    UpdateAuthorComponent,
    DeleteAuthorComponent,
    AuthorsComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes)
  ],
  exports: [],
  providers: []
})
export class AuthorModule { }
