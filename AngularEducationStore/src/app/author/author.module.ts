import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RouterModule } from '@angular/router';
import { routes } from 'src/app/author/author-routing.module';
import { AuthorsComponent } from 'src/app/author/authors/authors.component';
import {
  MatSortModule,
  MatFormFieldModule,
  MatInputModule,
  MatToolbarModule,
  MatGridListModule,
  MatCheckboxModule,
  MatSliderModule,
  MatButtonModule,
  MatSelectModule,
  MatDividerModule,
  MatPaginatorModule,
  MatCardModule,
  MatTableModule
} from '@angular/material';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { AuthorsDialogComponent } from 'src/app/author/authors/authors-dialog/authors-dialog.component';

@NgModule({
  declarations: [
    AuthorsComponent,
    AuthorsDialogComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    MatFormFieldModule,
    FormsModule,
    ReactiveFormsModule,
    MatInputModule,
    MatToolbarModule,
    MatGridListModule,
    MatCheckboxModule,
    MatSliderModule,
    MatButtonModule,
    MatSelectModule,
    MatDividerModule,
    MatPaginatorModule,
    FontAwesomeModule,
    MatCardModule,
    MatTableModule,
    MatSortModule
  ],
  exports: [],
  providers: [],
  entryComponents: [AuthorsDialogComponent]
})
export class AuthorModule { }
