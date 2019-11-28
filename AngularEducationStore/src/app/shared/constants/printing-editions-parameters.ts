import { PrintingEditionType } from 'src/app/shared/enums/printing-edition-type';
import { Injectable } from '@angular/core';
import { Currency } from 'src/app/shared/enums/currency';
import { PageSize } from 'src/app/shared/enums/page-size';
import { SortType } from 'src/app/shared/enums/sort-type';
import { BaseParameters } from 'src/app/shared/constants/base-parameters';
import { ProductPresentationModel } from 'src/app/shared/models/presentation/ProductPresenatationModel';
import { CurrencyPresentationModel } from 'src/app/shared/models/presentation/CurrencyPresentationModel';
import { GridFormatPresentationModel } from 'src/app/shared/models/presentation/GridFormatPresentationModel';
import { SortTypesPresentationModel } from 'src/app/shared/models/presentation/SortTypesPresentationModel';

Injectable();

export class PrintingEditionsParameters extends BaseParameters {

    public readonly productPresentationModels: Array<ProductPresentationModel> = [
        { name: PrintingEditionType[PrintingEditionType.Book], value: PrintingEditionType.Book, checked: true },
        { name: PrintingEditionType[PrintingEditionType.Magazine], value: PrintingEditionType.Magazine, checked: true },
        { name: PrintingEditionType[PrintingEditionType.Newspaper], value: PrintingEditionType.Newspaper, checked: true },
    ];

    public readonly currencyPresentationModels: Array<CurrencyPresentationModel> = [
        { name: Currency[Currency.USD], value: Currency.USD, symbol: '$' },
        { name: Currency[Currency.EUR], value: Currency.EUR, symbol: '€' },
        { name: Currency[Currency.CHF], value: Currency.CHF, symbol: '₣' },
        { name: Currency[Currency.GBP], value: Currency.GBP, symbol: '£' },
        { name: Currency[Currency.JPY], value: Currency.JPY, symbol: '¥' },
        { name: Currency[Currency.UAH], value: Currency.UAH, symbol: '₴' },
    ];

    public readonly gridFormationPrintingEditions: Array<GridFormatPresentationModel> = [
        { value: PageSize.Six, cols: 3, rowsHeight: 300, fontSize: { fontSize: '150' } },
        { value: PageSize.Twelve, cols: 4, rowsHeight: 250, fontSize: { fontSize: '80' } },
        { value: PageSize.Twenty, cols: 5, rowsHeight: 200, fontSize: { fontSize: '50' } },
    ];

    public readonly sortProductModels: Array<SortTypesPresentationModel> = [
        { name: SortType[SortType.Id], value: SortType.Id },
        { name: SortType[SortType.Title], value: SortType.Title },
        { name: SortType[SortType.Price], value: SortType.Price }
    ];
}
