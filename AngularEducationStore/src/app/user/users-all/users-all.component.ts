import { Component, OnInit } from '@angular/core';
import { MatSort, PageEvent, MatDialog, MatSelectChange } from '@angular/material';

import { UserService } from 'src/app/shared/services';
import { FilterUserModel } from 'src/app/shared/models/filter/filter-user-model';
import { UserModel } from 'src/app/shared/models/user/UserModel';

import { IsBlocked } from 'src/app/shared/enums/is-blocked';
import { faHighlighter } from '@fortawesome/free-solid-svg-icons';
import { faTimes } from '@fortawesome/free-solid-svg-icons';
import { UserParametrs } from 'src/app/shared/constants/user-parametrs';
import { RemoveDialogComponent } from 'src/app/shared/components/remove-dialog/remove-dialog.component';
import { UserEditDialogComponent } from 'src/app/user/users-all/user-edit-dialog/user-edit-dialog.component';
import { UserModelItem } from 'src/app/shared/models/user/UserModelItem';

@Component({
  selector: 'app-users-all',
  templateUrl: './users-all.component.html',
  styleUrls: ['./users-all.component.scss']
})

export class UsersAllComponent implements OnInit {


  constructor(private dialog: MatDialog, private userService: UserService, private userParametrs: UserParametrs) {
    this.filterModel.pageSize = 3;
  }

  displayedColumns: string[] = ['name', 'userEmail', 'status', ' '];

  editIcon = faHighlighter;
  closeIcon = faTimes;
  isLoading = true;
  isRequire: number;

  sortStates = this.userParametrs.sortStates;
  sortTypes = this.userParametrs.sortTypes;
  blockedTypes = this.userParametrs.blockedTypes;

  filterModel = new FilterUserModel();
  userModel = new UserModel();

  selectedBlockTypes = [IsBlocked.Block, IsBlocked.Unblock];

  ngOnInit() {
    this.userService.getAllUsers(this.filterModel).subscribe((data: UserModel) => {
      this.userModel = data;
    });
  }

  sortData(event: MatSort) {

    let sortState = this.sortStates.find(x => x.direction === event.direction.toLowerCase());
    this.filterModel.sortState = sortState.value;

    let sortTypes = this.sortTypes.find(x => x.name.toLowerCase() === event.active.toLowerCase());
    this.filterModel.sortType = sortTypes.value;

    this.submit();
  }
  submit(page: number = 1) {

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
  blockUser(user: UserModelItem) {
    this.userService.blockUser(user).subscribe(() => {

    });
  }
  changeBlockedUsers(event: MatSelectChange) {
    if (this.selectedBlockTypes.length === 1) {
      this.isRequire = event.value[0];
    }
    if (this.selectedBlockTypes.length === 0) {
      event.source.value = [this.isRequire];
      this.selectedBlockTypes = [this.isRequire];
    }
  }
  pageEvent(event: PageEvent) {
    let page = event.pageIndex + 1;

    if (event.pageSize !== this.filterModel.pageSize) {
      page = 1;
      this.filterModel.pageSize = event.pageSize;
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
    dialog.afterClosed().subscribe((result) => {
      if (result) {
        this.userService.updateUser(result).subscribe(() => {
          this.submit(this.filterModel.page);
        });
      }
    });
  }
  openRemoveDialog(element) {
    let dialog = this.dialog.open(RemoveDialogComponent, {
      data: {
        message: 'Do you wan`t to delete: ' + element.firstName + ' ' + element.lastName,
        closeTitle: 'No',
        removeTitle: 'Yes',
        id: element.id
      }
    });
    dialog.afterClosed().subscribe((result) => {
      if (result) {
        this.userService.removeUser(result.id).subscribe(() => {
          this.submit();
        });
      }
    });
  }
}
