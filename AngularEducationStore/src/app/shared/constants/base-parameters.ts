import { Injectable } from '@angular/core';
import { PageSize } from 'src/app/shared/enums/page-size';
import { SortState } from 'src/app/shared/enums/sort-state';
import { SortStatesPresentationModel } from 'src/app/shared/models/presentation/SortStatesPresentationModel';

Injectable();

export class BaseParameters {

    public readonly pageSizes = [
        PageSize.Six,
        PageSize.Twelve,
        PageSize.Twenty
    ];

    public readonly sortStateModels: Array<SortStatesPresentationModel> = [
        { name: 'Low to high', direction: SortState[SortState.asc], value: SortState.asc },
        { name: 'High to low', direction: SortState[SortState.desc], value: SortState.desc }
    ];
}
