import { BaseModel } from 'src/app/shared/models/base/BaseModel';
import { PrintingEditionModelItem } from 'src/app/shared/models/printing-editions/PrintingEditionModelItem';

export class PrintingEditionModel extends BaseModel {
  [x: string]: any;
    public items = new Array<PrintingEditionModelItem>();
    public itemsCount: number;
}
