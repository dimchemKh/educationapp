import { Component, OnInit } from '@angular/core';
import { AuthorService } from 'src/app/shared/services/author.service';
import { FilterAuthorModel } from 'src/app/shared/models/filter/filter-author-model';
import { faHighlighter } from '@fortawesome/free-solid-svg-icons';
import { faTimes } from '@fortawesome/free-solid-svg-icons';
import { faPlusCircle } from '@fortawesome/free-solid-svg-icons';
import { AuthorModel } from 'src/app/shared/models/authors/AuthorModel';
import { AuthorParametrs } from 'src/app/shared/constants/author-parametrs';
import { RemoveDialogComponent } from 'src/app/shared/components/remove-dialog/remove-dialog.component';
import { MatDialog, MatSort, PageEvent } from '@angular/material';
import { AuthorsDialogComponent } from './authors-dialog/authors-dialog.component';
import { AuthorModelItem } from 'src/app/shared/models/authors/AuthorModelItem';

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
  sortStates = this.authorParametrs.sortStates;
  sortTypes = this.authorParametrs.SortTypes;

  displayedColumns = ['id', 'name', 'products', ' '];

  constructor(private dialog: MatDialog, private authorService: AuthorService, private authorParametrs: AuthorParametrs) {
  }

  ngOnInit() {
    this.authorService.getAuthorsInPrintingEditions(this.filterModel).subscribe((data) => {
      this.authorModel = data;
    });
  }
  pageEvent(event: PageEvent) {
    console.log(event);
    let page = event.pageIndex + 1;

    if (event.pageSize !== this.filterModel.pageSize) {
      page = 1;
      this.filterModel.pageSize = event.pageSize;
    }
    this.submit(page);
  }
  sortData(event: MatSort) {
    let sortState = this.sortStates.find(x => x.direction.toLowerCase() === event.direction.toLowerCase());
    this.filterModel.sortState = sortState.value;

    let sortTypes = this.sortTypes.find(x => x.name.toLowerCase() === event.active.toLowerCase());
    this.filterModel.sortType = sortTypes.value;

    this.submit();
  }
  submit(page: number = 1) {
    this.filterModel.page = page;
    this.authorService.getAuthorsInPrintingEditions(this.filterModel).subscribe((data) => {
      this.authorModel = data;
    });
  }
  openCreateDialog() {
    let dialog = this.dialog.open(AuthorsDialogComponent, {
      data: {
        dialogTitle: 'Create new Author'
      }
    });
    dialog.afterClosed().subscribe((result) => {
      if (result) {
        this.authorService.createAuthor(result).subscribe(() => {
          this.submit();
        });
      }
    });
  }
  openEditDialog(element: AuthorModelItem = null) {
    let dialog = this.dialog.open(AuthorsDialogComponent, {
      data: {
        dialogTitle: 'Change',
        id: element.id,
        name: element.name
      }
    });
    dialog.afterClosed().subscribe((result) => {
      if (result) {
        this.authorService.updateAuthor(result).subscribe(() => {
          this.submit();
        });
      }
    });
  }
  openRemoveDialog(element) {
    let dialog = this.dialog.open(RemoveDialogComponent, {
      data: {
        closeTitle: 'Close',
        removeTitle: 'Remove',
        message: 'Do you want`t remove: ' + element.name,
        id: element.id
      }
    });
    dialog.afterClosed().subscribe((result) => {
      if (result) {
        this.authorService.removeAuthor(result.id).subscribe(() => {
          this.submit();
        });
      }
    });
  }
}
