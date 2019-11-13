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
        { name: 'Low to high', direction: SortState[SortState.asc], value: SortState.asc },
        { name: 'High to low', direction: SortState[SortState.desc], value: SortState.desc }
    ];
}
