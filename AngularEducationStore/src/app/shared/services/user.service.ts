import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiRoutes } from 'src/environments/api-routes';
import { FilterUserModel, UserUpdateModel, UserModel, UserModelItem } from 'src/app/shared/models';

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
  blockUser(userModel: UserModelItem): Observable<UserModel> {
    return this.http.post<UserModel>(this.apiRoutes.userRoute + 'block', userModel);
  }
  removeUser(userId: number) {
    return this.http.delete(this.apiRoutes.userRoute + 'delete?userid=' + userId);
  }
}
