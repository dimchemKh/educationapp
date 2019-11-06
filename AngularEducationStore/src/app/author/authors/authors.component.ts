import { Component, OnInit } from '@angular/core';
import { AuthorService } from 'src/app/shared/services/author.service';
import { FilterAuthorModel } from 'src/app/shared/models/filter/filter-author-model';
import { faHighlighter } from '@fortawesome/free-solid-svg-icons';
import { faTimes } from '@fortawesome/free-solid-svg-icons';
import { faPlusCircle } from '@fortawesome/free-solid-svg-icons';
import { AuthorModel } from 'src/app/shared/models/authors/AuthorModel';
import { AuthorParametrs } from 'src/app/shared/constants/author-parametrs';

@Component({
  selector: 'app-authors',
  templateUrl: './authors.component.html',
  styleUrls: ['./authors.component.scss']
})
export class AuthorsComponent implements OnInit {

  editIcon = faHighlighter;
  closeIcon = faTimes;
  createIcon = faPlusCircle;

  filterModel = new FilterAuthorModel();
  authorModel = new AuthorModel();
  pageSizes = this.authorParametrs.pageSizes;

  constructor(private authorService: AuthorService, private authorParametrs: AuthorParametrs) { }
  displayedColumns = ['id', 'name', 'products', ' '];

  ngOnInit() {
    this.authorService.getAuthors(this.filterModel).subscribe((data: AuthorModel) => {
      this.authorModel = data;
    });
  }
  pageEvent(event) {

  }
  sortData(event) {

  }
  openEditDialog(element) {

  }
  openCreateDialog() {

  }
}
