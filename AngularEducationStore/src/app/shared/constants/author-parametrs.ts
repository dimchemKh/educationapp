
import { SortType } from 'src/app/shared/enums/sort-type';
import { Injectable } from '@angular/core';
import { BaseParametrs } from './base-parametrs';

Injectable();

export class AuthorParametrs extends BaseParametrs {

    public readonly SortTypes = [
        { name: SortType[SortType.Id], value: SortType.Id },
        { name: SortType[SortType.Name], value: SortType.Name },
    ];
}
