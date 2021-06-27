import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../environments/environment';
import {Observable, of} from 'rxjs';
import {Member} from '../_models/Member';
import {map} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  members: Member[] = [];

  constructor(private http: HttpClient) {
  }

  getMemberByName(name: string): Observable<Member> {
    const member = this.members.find(m => m.username === name);

    if (member !== undefined) {
      return of(member);
    }

    return this.http.get<Member>(environment.apiUrl + `users/${name}`);
  }

  getMembers(): Observable<Member[]> {
    if (this.members.length > 0) {
      return of(this.members);
    }
    return this.http.get<Member[]>(environment.apiUrl + 'users')
      .pipe(map(members => this.members = members));
  }

  updateMember(member: Member): Observable<any> {
    return this.http.put(environment.apiUrl + 'users', member).pipe(map(() => {
      const index = this.members.indexOf(member);
      this.members[index] = member;
    }));
  }

}