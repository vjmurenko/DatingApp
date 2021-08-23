import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {environment} from '../../environments/environment';
import {Observable, of} from 'rxjs';
import {Member} from '../_models/Member';
import {map, take} from 'rxjs/operators';
import {PaginatedResult, Pagination} from '../_models/pagination';
import {UserParams} from '../_models/UserParams';
import {AccountService} from './account.service';
import {User} from '../_models/User';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  members: Member[] = [];
  membersCash = new Map();
  userParams: UserParams;
  user: User;

  constructor(private http: HttpClient, private _accountSevice: AccountService) {
    this._accountSevice.currentUser$.pipe(take(1)).subscribe(result => {
      this.user = result;
      this.userParams = new UserParams(this.user);
    });
  }

  setUserParams(params: UserParams) {
    this.userParams = params;
  }

  getUserParams() {
    return this.userParams;
  }

  resetUserParams() {
    let userParams = new UserParams(this.user);
    return userParams;

  }

  getMemberByName(name: string): Observable<Member> {
    const user = [...this.membersCash.values()]
      .reduce((arr, elem) => arr.concat(elem.result), [])
      .find(u => u.username === name);

    if (user) {
      return of(user);
    }

    return this.http.get<Member>(environment.apiUrl + `users/${name}`);
  }

  getMembers(userParams: UserParams): Observable<PaginatedResult<Member[]>> {
    let params = MembersService.getFilterMemberParams(userParams);
    let url = environment.apiUrl + 'users';
    let response = this.membersCash.get(Object.values(userParams).join('-'));

    if (response) {
      return of(response);
    }

    return this.getPaginatedResult<Member[]>(url, params)
      .pipe(map(response => {
        this.membersCash.set(Object.values(userParams).join('-'), response);

        return response;
      }));
  }

  updateMember(member: Member): Observable<any> {
    return this.http.put(environment.apiUrl + 'users', member).pipe(map(() => {
      const index = this.members.indexOf(member);
      if (index === -1) {
        this.members.push(member);
      } else {
        this.members[index] = member;
      }

    }));
  }

  setMainPhoto(photoId: number): Observable<void> {
    return this.http.post<void>(environment.apiUrl + `users/set-main-photo/${photoId}`, {});
  }

  deletePhoto(photoId: number): Observable<void> {
    return this.http.delete<void>(environment.apiUrl + `users/delete-photo/${photoId}`);
  }

  addUserLike(userName: string) {
    return this.http.post(environment.apiUrl + `likes/${userName}`, {});
  }

  getUserLikes(predicate: string, pageSize: number, pageNumber: number): Observable<PaginatedResult<Partial<Member[]>>> {
    let params = MembersService.getPageParams(pageSize, pageNumber);
    params = params.append('predicate', predicate);

    return this.getPaginatedResult<Partial<Member[]>>(environment.apiUrl + 'likes', params);
  }

  private getPaginatedResult<T>(url: string, params: HttpParams) {
    let paginatedResult = new PaginatedResult<T>();

    return this.http.get<T>(url, {observe: 'response', params})
      .pipe(map(result => {
        paginatedResult.result = result.body;

        if (result.headers.get('Pagination') != null) {
          paginatedResult.pagination = JSON.parse(result.headers.get('Pagination'));
        }

        return paginatedResult;
      }));
  }

  private static getFilterMemberParams(userParams: UserParams): HttpParams {
    let params = MembersService.getPageParams(userParams.pageSize, userParams.pageNumber);
    params = params.append('gender', userParams.gender);
    params = params.append('maxAge', userParams.maxAge.toString());
    params = params.append('minAge', userParams.minAge.toString());
    params = params.append('orderBy', userParams.orderBy);

    return params;
  }

  private static getPageParams(pageSize: number, pageNumber: number): HttpParams {
    let params = new HttpParams();

    if (pageNumber != null && pageSize != null) {
      params = params.append('pageNumber', pageNumber.toString());
      params = params.append('pageSize', pageSize.toString());
    }
    return params;
  }
}