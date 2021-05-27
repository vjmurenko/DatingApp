import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {pipe, ReplaySubject} from 'rxjs';
import {map} from 'rxjs/operators';
import {User} from '../_models/User';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private baseUrl: string = 'https://localhost:5001/api/';

  private currentUserSource = new ReplaySubject<User>(1);
  public currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) {
  }

  public login(model: any) {
    return this.http.post(this.baseUrl + 'account/login', model).pipe(
      map((response: User) => {
        const user = response;
        if (user) {
          localStorage.setItem("user", JSON.stringify(user));
          this.currentUserSource.next(user);
        }
      })
    );
  }

  public register(model: any){
    return this.http.post(this.baseUrl + 'account/register', model).pipe(
      map((response: User) => {
        const user = response;
        if(user) {
          localStorage.setItem("user", JSON.stringify(user));
          this.currentUserSource.next(user);
        }
      })
    )
  }

  public setCurrentUser(user: User){
    this.currentUserSource.next(user);
  }

  public logout(){
    localStorage.removeItem("user");
    this.currentUserSource.next(null);
  }

}
