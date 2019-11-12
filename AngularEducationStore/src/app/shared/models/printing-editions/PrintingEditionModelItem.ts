import { AuthorModelItem } from '../authors/AuthorModelItem';

export class PrintingEditionModelItem {

    public id: number;
    public title: string;
    public description: string;
    public authors: Array<AuthorModelItem>;
    public currency = 0;
    public price: number;
    public printingEditionType: number;

}
