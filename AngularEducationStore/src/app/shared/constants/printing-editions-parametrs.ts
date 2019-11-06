import { PrintingEditionType } from '../enums/printing-edition-type';
import { Injectable } from '@angular/core';
import { Currency } from '../enums/currency';
import { SortState } from '../enums/sort-state';
import { PageSize } from '../enums/page-size';

Injectable();

export class PrintingEditionsParametrs {
    public readonly printingEditionTypes = [
        { name: PrintingEditionType[PrintingEditionType.Book], value: PrintingEditionType.Book, checked: true },
        { name: PrintingEditionType[PrintingEditionType.Magazine], value: PrintingEditionType.Magazine, checked: false },
        { name: PrintingEditionType[PrintingEditionType.Newspaper], value: PrintingEditionType.Newspaper, checked: false },
      ];
    public readonly currencyTypes = [
        { name: Currency[Currency.USD], value: Currency.USD, symbol: '$' },
        { name: Currency[Currency.EUR], value: Currency.EUR, symbol: '€' },
        { name: Currency[Currency.CHF], value: Currency.CHF, symbol: '₣' },
        { name: Currency[Currency.GBP], value: Currency.GBP, symbol: '£' },
        { name: Currency[Currency.JPY], value: Currency.JPY, symbol: '¥' },
        { name: Currency[Currency.UAH], value: Currency.UAH, symbol: '₴' },
    ];
    public readonly sortTypes = [
        { name: 'Low to High', value: SortState.asc },
        { name: 'High to Low', value: SortState.desc }
    ];
    public readonly pageSizes = [
         PageSize.Six,
         PageSize.Twelve,
         PageSize.Twenty
    ];
    public readonly gridFormationPrintingEditions = [
        { value: PageSize.Six, cols: 3, rowsHeight: 300, fontSize: {fontSize: '150'} },
        { value: PageSize.Twelve, cols: 4, rowsHeight: 250, fontSize: {fontSize: '80'} },
        { value: PageSize.Twenty, cols: 5, rowsHeight: 200, fontSize: {fontSize: '50'} },
    ];
}
