import { SortType } from 'src/app/shared/enums/sort-type';
import { Injectable } from '@angular/core';
import { BaseParameters } from 'src/app/shared/constants/base-parameters';
import { AuthorPresentationModel } from 'src/app/shared/models/presentation/AuthorPresentationModel';

Injectable();

export class AuthorParameters extends BaseParameters {

    public readonly SortAuthorModels: Array<AuthorPresentationModel> = 
    [
        { name: SortType[SortType.Id], value: SortType.Id},
        { name: SortType[SortType.Name], value: SortType.Name }
    ];
}
