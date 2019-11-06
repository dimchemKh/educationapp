import { Component, ViewChild, AfterViewInit } from '@angular/core';
import { MatPaginator, MatSort, PageEvent, MatDialog } from '@angular/material';

import { UserService } from 'src/app/shared/services/user.service';
import { FilterUserModel } from 'src/app/shared/models/filter/filter-user-model';
import { UserModel } from 'src/app/shared/models/user/UserModel';

import { IsBlocked } from 'src/app/shared/enums/is-blocked';
import { faHighlighter } from '@fortawesome/free-solid-svg-icons';
import { faTimes } from '@fortawesome/free-solid-svg-icons';
import { UserParametrs } from 'src/app/shared/constants/user-parametrs';
import { UserEditDialogComponent } from './user-edit-dialog/user-edit-dialog.component';
import { UserRemoveDialogComponent } from './user-remove-dialog/user-remove-dialog.component';

@Component({
  selector: 'app-users-all',
  templateUrl: './users-all.component.html',
  styleUrls: ['./users-all.component.scss']
})

export class UsersAllComponent implements AfterViewInit {


  constructor(private dialog: MatDialog, private userService: UserService, private userParametrs: UserParametrs) { }

  displayedColumns: string[] = ['userName', 'userEmail', 'status', ' '];


  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  editIcon = faHighlighter;
  closeIcon = faTimes;
  isLoading = true;

  searchString: string;
  pageSize = 3;

  sortStates = this.userParametrs.sortStates;
  sortTypes = this.userParametrs.sortTypes;
  blockedTypes = this.userParametrs.blockedTypes;

  filterModel = new FilterUserModel();
  userModel = new UserModel();

  selectedBlockTypes = [IsBlocked.Block, IsBlocked.Unblock];

  ngAfterViewInit() {
    // this.userModel = new UserModel();
    this.userService.getAllUsers(this.filterModel).subscribe((data: UserModel) => {
      this.userModel = data;
    });

    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
  }
  sortData(event: MatSort) {

    let sortState = this.sortStates.find(x => x.name === event.direction.toLowerCase());
    this.filterModel.sortState = sortState.value;

    let sortTypes = this.sortTypes.find(x => x.name.toLowerCase() === event.active.toLowerCase());
    this.filterModel.sortType = sortTypes.value;

    this.submit();
  }
  submit(page: number = 1) {
    this.filterModel.pageSize = this.pageSize;
    this.filterModel.searchString = this.searchString;
    if (this.selectedBlockTypes.length >= 2) {
      this.filterModel.isBlocked = IsBlocked.All;
    }
    if (this.selectedBlockTypes.length < 2) {
      this.filterModel.isBlocked = this.selectedBlockTypes[0];
    }
    this.filterModel.page = page;
    this.userService.getAllUsers(this.filterModel).subscribe((data: UserModel) => {
      this.userModel = data;
    });
  }
  sortBlockedUsers(event: boolean) {
    if (!event) {
      this.submit();
    }
  }
  pageEvent(event: PageEvent) {
    let page = event.pageIndex + 1;

    if (event.pageSize !== this.pageSize) {
      page = 1;
      this.pageSize = event.pageSize;
    }
    this.submit(page);
  }
  openEditDialog(element) {
    let dialog = this.dialog.open(UserEditDialogComponent, {
      data: {
        id: element.id,
        firstName: element.firstName,
        lastName: element.lastName,
        email: element.email
      }
    });
    dialog.afterClosed().subscribe(() => {
      this.submit(this.filterModel.page);
    });
  }
  
  openRemoveDialog(element) {
    let dialog = this.dialog.open(UserRemoveDialogComponent, {
      data: {
        id: element.id
      }
    });
    dialog.afterClosed().subscribe(() => {
      this.submit(this.filterModel.page);
    });
  }
}
