import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BaseModel, UserLoginModel, UserRegistrationModel } from 'src/app/shared/models';
import { ApiRoutes } from 'src/environments/api-routes';
import { AuthModel } from 'src/app/shared/models/auth/auth.model';


@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(
    private http: HttpClient,
    private apiRoutes: ApiRoutes,
  ) {

  }
  
  refresh(): Observable<object> {
    return this.http.get(this.apiRoutes.accountRoute + 'refresh',
      {
        withCredentials: true
      });
  }

  signInUser(userModel: UserLoginModel): Observable<AuthModel> {
    let httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
      withCredentials: true
    };
    
    return this.http.post<AuthModel>(this.apiRoutes.accountRoute + 'signIn', userModel, httpOptions);
  }

  signUpUser(user: UserRegistrationModel): Observable<BaseModel> {
    return this.http.post<BaseModel>(this.apiRoutes.accountRoute + 'signUp', user);
  }

  forgotPassword(userModel: UserLoginModel): Observable<BaseModel> {
    return this.http.post<BaseModel>(this.apiRoutes.accountRoute + 'forgotPassword', userModel);
  }
}
