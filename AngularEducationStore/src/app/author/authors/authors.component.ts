import { Component, OnInit } from '@angular/core';
import { AuthorService } from 'src/app/shared/services';
import { FilterAuthorModel, AuthorModel, AuthorModelItem } from 'src/app/shared/models';
import { faHighlighter, IconDefinition } from '@fortawesome/free-solid-svg-icons';
import { faTimes } from '@fortawesome/free-solid-svg-icons';
import { faPlusCircle } from '@fortawesome/free-solid-svg-icons';
import { AuthorParameters } from 'src/app/shared/constants/author-parameters';
import { RemoveDialogComponent } from 'src/app/shared/components/remove-dialog/remove-dialog.component';
import { MatDialog, MatSort, PageEvent } from '@angular/material';
import { AuthorsDialogComponent } from './authors-dialog/authors-dialog.component';
import { PageSize } from 'src/app/shared/enums';
import { ColumnsTitles } from 'src/app/shared/constants/columns-titles';
import { AuthorPresentationModel } from 'src/app/shared/models/presentation/AuthorPresentationModel';
import { SortStatesPresentationModel } from 'src/app/shared/models/presentation/SortStatesPresentationModel';

@Component({
  selector: 'app-authors',
  templateUrl: './authors.component.html',
  styleUrls: ['./authors.component.scss']
})
export class AuthorsComponent implements OnInit {

  editIcon: IconDefinition;
  closeIcon: IconDefinition;
  createIcon: IconDefinition;

  filterModel: FilterAuthorModel;
  authorModel: AuthorModel;

  pageSizes: PageSize[];
  sortStateModels: Array<SortStatesPresentationModel>;
  SortAuthorModels: Array<AuthorPresentationModel>; 

  columnsAuthors: string[];

  constructor(private dialog: MatDialog, private authorService: AuthorService, private authorParametrs: AuthorParameters,
              private columnTitles: ColumnsTitles) {
    this.editIcon = faHighlighter;
    this.closeIcon = faTimes;
    this.createIcon = faPlusCircle;
    this.filterModel = new FilterAuthorModel();
    this.authorModel = new AuthorModel();
    this.pageSizes = this.authorParametrs.pageSizes;
    this.sortStateModels = this.authorParametrs.sortStateModels;
    this.SortAuthorModels = this.authorParametrs.SortAuthorModels;
    this.columnsAuthors = this.columnTitles.columnsAuthors;
  }

  ngOnInit(): void {
    this.authorService.getAuthorsInPrintingEditions(this.filterModel).subscribe((data) => {
      this.authorModel = data;
    });
  }

  pageEvent(event: PageEvent): void {
    let page = event.pageIndex + 1;

    if (event.pageSize !== this.filterModel.pageSize) {
      page = 1;
      this.filterModel.pageSize = event.pageSize;
    }

    this.submit(page);
  }

  sortData(event: MatSort): void {
    let sortState = this.sortStateModels.find(x => x.direction.toLowerCase() === event.direction.toLowerCase());

    this.filterModel.sortState = sortState.value;

    let sortTypes = this.SortAuthorModels.find(x => x.name.toLowerCase() === event.active.toLowerCase());

    this.filterModel.sortType = sortTypes.value;

    this.submit();
  }

  submit(page: number = 1): void {
    this.filterModel.page = page;

    this.authorService.getAuthorsInPrintingEditions(this.filterModel).subscribe((data) => {
      this.authorModel = data;
    });
  }

  openCreateDialog(): void {
    let dialog = this.dialog.open(AuthorsDialogComponent, {
      data: {
        dialogTitle: 'Create new Author'
      }
    });

    dialog.afterClosed().subscribe((result) => {
      if (result) {
        this.authorService.createAuthor(result).subscribe(() => {

        });
      }
    });
  }

  openEditDialog(element: AuthorModelItem = null): void {
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

  openRemoveDialog(element): void {
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
