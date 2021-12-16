import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {pipe, ReplaySubject} from 'rxjs';
import {map} from 'rxjs/operators';
import {User} from '../_models/User';
import {environment} from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private currentUserSource = new ReplaySubject<User>(1);

  public currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) {
  }

  public login(model: any) {
    return this.http.post(environment.apiUrl + 'account/login', model).pipe(
      map((response: User) => {
        const user = response;
        if (user) {
          this.setCurrentUser(user);
        }
      })
    );
  }

  public register(model: any){
    return this.http.post(environment.apiUrl + 'account/register', model).pipe(
      map((response: User) => {
        const user = response;
        if(user) {
          this.setCurrentUser(user);
        }
      })
    )
  }

  public setCurrentUser(user: User){
    user.roles = [];

    const roles = this.getDecadedToken(user.token).role;

    Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);

    localStorage.setItem("user", JSON.stringify(user));
    this.currentUserSource.next(user);
  }

  public logout(){
    localStorage.removeItem("user");
    this.currentUserSource.next(null);
  }

  getDecadedToken(token){
    return JSON.parse(atob(token.split('.')[1]))
  }
}
