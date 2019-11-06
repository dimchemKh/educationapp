import { AuthorModel } from '../authors/AuthorModel';

export class PrintingEditionModelItem {

    public id: number;
    public title: string;
    public description: string;
    public authors: Array<AuthorModel>;
    public currency: number;
    public price: number;
    public printingEditionType: number;

}
