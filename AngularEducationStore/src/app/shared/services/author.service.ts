import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiRoutes } from 'src/environments/api-routes';


@Injectable({
  providedIn: 'root'
})
export class AuthorService {

  constructor(private http: HttpClient, private apiRoutes: ApiRoutes) { }

  requestAuthors() {
    return this.http.post<any>(this.apiRoutes.authorsRoute + 'get', {});
  }
}
