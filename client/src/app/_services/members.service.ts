import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../environments/environment';
import {Observable} from 'rxjs';
import {Member} from '../_models/Member';

@Injectable({
  providedIn: 'root'
})
export class MembersService {


  constructor(private http: HttpClient) {
  }

  getMemberByName(name: string): Observable<Member> {
    return this.http.get<Member>(environment.apiUrl + `users/${name}`);
  }

  getMembers(): Observable<Member[]> {
    return this.http.get<Member[]>(environment.apiUrl + 'users');

  }
}