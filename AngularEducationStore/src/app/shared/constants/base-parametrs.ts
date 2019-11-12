import { Injectable } from '@angular/core';
import { PageSize } from 'src/app/shared/enums/page-size';
import { SortState } from 'src/app/shared/enums/sort-state';

Injectable();

export class BaseParametrs {
    public readonly pageSizes = [
        PageSize.Six,
        PageSize.Twelve,
        PageSize.Twenty
    ];
    public readonly sortStates = [
        { direction: 'asc', name: SortState[SortState['Low to hight']], value: SortState['Low to hight'] },
        { direction: 'desc', name: SortState[SortState['Hight to low']], value: SortState['Hight to low'] }
    ];
}
