import { Injectable } from '@angular/core';
import { SortState } from '../enums/sort-state';
import { SortType } from '../enums/sort-type';
import { IsBlocked } from '../enums/is-blocked';

Injectable();

export class UserParametrs {
    public readonly sortStates = [
        { name: SortState[SortState.asc], value:  SortState.asc },
        { name: SortState[SortState.desc], value: SortState.desc }
    ];
    public readonly sortTypes = [
        { name: SortType[SortType.userName], value: SortType.userName },
        { name: SortType[SortType.userEmail], value: SortType.userEmail },
    ];
    public readonly blockedTypes =
    [
      { name: IsBlocked[IsBlocked.Block], value: IsBlocked.Block },
      { name: IsBlocked[IsBlocked.Unblock], value: IsBlocked.Unblock }
    ];
}

