
import { SortType } from 'src/app/shared/enums/sort-type';
import { Injectable } from '@angular/core';
import { BaseParameters } from './base-parameters';

Injectable();

export class AuthorParameters extends BaseParameters {

    public readonly SortTypes = [
        { name: SortType[SortType.Id], value: SortType.Id },
        { name: SortType[SortType.Name], value: SortType.Name },
    ];
}
