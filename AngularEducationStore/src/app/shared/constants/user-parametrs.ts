import { Injectable } from '@angular/core';
import { SortType } from 'src/app/shared/enums/sort-type';
import { IsBlocked } from 'src/app/shared/enums/is-blocked';
import { BaseParameters } from 'src/app/shared/constants/base-parameters';

Injectable();

export class UserParameters extends BaseParameters {

    public readonly sortTypes = [
        { name: SortType[SortType.Name], value: SortType.Name },
        { name: SortType[SortType.userEmail], value: SortType.userEmail },
    ];
    public readonly blockedTypes =
    [
      { name: IsBlocked[IsBlocked.Block], value: IsBlocked.Block },
      { name: IsBlocked[IsBlocked.Unblock], value: IsBlocked.Unblock }
    ];
}

