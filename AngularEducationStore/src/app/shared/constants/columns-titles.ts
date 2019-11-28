import { Injectable } from '@angular/core';

Injectable();

export class ColumnsTitles {
    public readonly columnsAuthors = ['id', 'name', 'products', ' '];

    public readonly columnsOrdersAdmin = ['id', 'date', 'userName', 'userEmail', 'printingEditionType',
                                        'printingEditionTitle', 'quantity', 'amount', 'transactionStatus'];
    
    public readonly columnsOrdersUser = ['id', 'date', 'printingEditionType', 'printingEditionTitle',
                                        'quantity', 'amount', 'transactionStatus'];

    public readonly columnsUsers = ['name', 'userEmail', 'status', ' '];

    public readonly columnsPrintingEditions = ['id', 'title', 'description', 'category', 'author', 'price', ' '];

    public readonly columnsCart  = ['product', 'price', 'qty', 'amount', ' '];
}
