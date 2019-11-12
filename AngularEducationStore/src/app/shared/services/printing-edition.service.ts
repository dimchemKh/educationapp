import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { FilterPrintingEditionModel } from 'src/app/shared/models/filter/filter-printing-edition-model';
import { ApiRoutes } from 'src/environments/api-routes';
import { Observable } from 'rxjs';
import { PrintingEditionModel } from 'src/app/shared/models/printing-editions/PrintingEditionModel';
import { PageSize } from 'src/app/shared/enums/page-size';
import { PrintingEditionModelItem } from '../models/printing-editions/PrintingEditionModelItem';



@Injectable({
  providedIn: 'root'
})
export class PrintingEditionService {

  constructor(private http: HttpClient, private apiRoutes: ApiRoutes) { }

  getPrintingEditions(role: string, filterModel: FilterPrintingEditionModel): Observable<PrintingEditionModel> {
    return this.http.post<PrintingEditionModel>(this.apiRoutes.printingEditionRoute + 'get?role=' + role, filterModel);
  }
  getPrintingEditionDetails(printingEditionId: number, currency: number = 1): Observable<PrintingEditionModel> {
    // tslint:disable-next-line: max-line-length
    return this.http.get<PrintingEditionModel>(this.apiRoutes.printingEditionRoute + 'details?printingEditionId=' + printingEditionId + '&currency=' + currency);
  }
  createPrintingEdition(printingEdition: PrintingEditionModelItem): Observable<PrintingEditionModel> {
    return this.http.post<PrintingEditionModel>(this.apiRoutes.printingEditionRoute + 'create', printingEdition);
  }
  updatePrintingEdition(printingEdition: PrintingEditionModelItem): Observable<PrintingEditionModel> {
    return this.http.put<PrintingEditionModel>(this.apiRoutes.printingEditionRoute + 'update', printingEdition);
  }
  removePrintingEdition(printingEditionId: number): Observable<PrintingEditionModel> {
    return this.http.delete<PrintingEditionModel>(this.apiRoutes.printingEditionRoute + 'delete?printingEditionId=' + printingEditionId);
  }
  getIconStyle(pageSize: number) {
    if (pageSize === PageSize.Six) {
      return { fontSize: '150px' };
    }
    if (pageSize === PageSize.Twelve) {
      return { fontSize: '80px' };
    }
    if (pageSize === PageSize.Twenty) {
      return { fontSize: '50px' };
    }
  }
}
