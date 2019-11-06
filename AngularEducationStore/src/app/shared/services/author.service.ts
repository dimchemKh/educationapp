import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiRoutes } from 'src/environments/api-routes';
import { FilterAuthorModel } from 'src/app/shared/models/filter/filter-author-model';
import { AuthorModel } from '../models/authors/AuthorModel';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class AuthorService {

  constructor(private http: HttpClient, private apiRoutes: ApiRoutes) { }

  getAuthors(filterModel: FilterAuthorModel): Observable<AuthorModel> {
    return this.http.post<AuthorModel>(this.apiRoutes.authorsRoute + 'get', filterModel);
  }
  getAllAuthors(filterModel: FilterAuthorModel): Observable<AuthorModel> {
    return this.http.post<AuthorModel>(this.apiRoutes.authorsRoute + 'getAll', filterModel);
  }
}
