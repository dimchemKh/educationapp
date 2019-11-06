import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { ApiRoutes } from 'src/environments/api-routes';
import { UserUpdateModel } from 'src/app/shared/models/user/UserUpdateModel';
import { FilterUserModel } from 'src/app/shared/models/filter/filter-user-model';
import { UserModel } from 'src/app/shared/models/user/UserModel';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient, private apiRoutes: ApiRoutes) { }
  getUserOne(): Observable<UserUpdateModel> {
    return this.http.post<UserUpdateModel>(this.apiRoutes.userRoute + 'get', {
      withCredentials: true
    });
  }
  getAllUsers(filterModel: FilterUserModel): Observable<UserModel> {
    return this.http.post<UserModel>(this.apiRoutes.userRoute + 'getAll', filterModel, {
      withCredentials: true
    }).pipe();
  }
  updateUser(userModel: UserUpdateModel): Observable<UserUpdateModel> {
    return this.http.post<UserUpdateModel>(this.apiRoutes.userRoute + 'update', userModel);
  }
  removeUser(userId: number) {
    debugger
    return this.http.delete(this.apiRoutes.userRoute + 'delete?userid=' + userId);
  }
}
