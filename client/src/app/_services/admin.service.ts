import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../environments/environment';
import {User} from '../_models/User';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  constructor(private http: HttpClient) { }

  getUsersWithRoles(): Observable<User[]>{
    return this.http.get<User[]>(environment.apiUrl + 'admin/users-with-roles');
  }

  updateUserRoles(username: string, roles: string[]) {
    return this.http.post(environment.apiUrl + 'admin/edit-roles/' + username + '?roles=' + roles, {});
  }
}
