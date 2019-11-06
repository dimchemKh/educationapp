import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { FilterPrintingEditionModel } from 'src/app/shared/models/filter/filter-printing-edition-model';
import { ApiRoutes } from 'src/environments/api-routes';
import { Observable } from 'rxjs';
import { PrintingEditionModel } from 'src/app/shared/models/printing-editions/PrintingEditionModel';
import { PageSize } from 'src/app/shared/enums/page-size';



@Injectable({
  providedIn: 'root'
})
export class PrintingEditionService {

  constructor(private http: HttpClient, private apiRoutes: ApiRoutes) { }

  getPrintingEditions(filterModel: FilterPrintingEditionModel): Observable<PrintingEditionModel> {
    return this.http.post<PrintingEditionModel>(this.apiRoutes.printingEditionRoute + 'get', filterModel);
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
