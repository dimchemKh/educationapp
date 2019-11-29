import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiRoutes } from 'src/environments/api-routes';
import { FilterAuthorModel, AuthorModelItem, AuthorModel } from 'src/app/shared/models';
import { Observable, BehaviorSubject } from 'rxjs';
import { scan } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthorService {

  private authorsSubj: BehaviorSubject<AuthorModelItem[]>;


  constructor(
    private http: HttpClient,
    private apiRoutes: ApiRoutes
  ) {
    this.authorsSubj = new BehaviorSubject<AuthorModelItem[]>([]);
  }
  getAuthorsSubj(): Observable<AuthorModelItem[]> {
    return this.authorsSubj.asObservable();
  }

  getAuthorsSubjScan(): Observable<any> {
    return this.authorsSubj.asObservable().pipe(
      scan((acc, curr) => {
        return [...acc, ...curr];
      }, [])
    );
  }

  nextAuthorSubj(models: Array<AuthorModelItem>): void {
    return this.authorsSubj.next(models);
  }

  getAuthorsInPrintingEditions(filterModel: FilterAuthorModel): Observable<AuthorModel> {
    return this.http.post<AuthorModel>(this.apiRoutes.authorsRoute + 'get', filterModel);
  }

  getAllAuthors(filterModel: FilterAuthorModel): Observable<AuthorModel> {
    return this.http.post<AuthorModel>(this.apiRoutes.authorsRoute + 'getAll', filterModel);
  }

  createAuthor(author: AuthorModelItem): Observable<AuthorModel> {
    return this.http.post<AuthorModel>(this.apiRoutes.authorsRoute + 'create', author);
  }

  updateAuthor(author: AuthorModelItem): Observable<AuthorModel> {
    return this.http.put<AuthorModel>(this.apiRoutes.authorsRoute + 'update', author);
  }

  removeAuthor(authorId: number): Observable<AuthorModel> {
    return this.http.delete<AuthorModel>(this.apiRoutes.authorsRoute + 'delete?authorid=' + authorId);
  }
}
