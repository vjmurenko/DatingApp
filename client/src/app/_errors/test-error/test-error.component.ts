import {Component, OnInit} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {User} from '../../_models/User';

@Component({
  selector: 'app-test-error',
  templateUrl: './test-error.component.html',
  styleUrls: ['./test-error.component.css']
})
export class TestErrorComponent implements OnInit {

  private baseUrl: string = 'https://localhost:5001/api/Buggy';
  public errorsArray: string[] = [];

  constructor(private http: HttpClient) {
  }

  ngOnInit(): void {
  }

  get401Unauthorized() {
    this.http.get(this.baseUrl + '/secret').subscribe(result => {
      console.log(result);
    });
  }

  get404NotFound() {
    this.http.get(this.baseUrl + '/notfound').subscribe(result => {
      console.log(result);
    });
    ;
  }

  get400BadRequest() {
    this.http.get(this.baseUrl + '/badrequest').subscribe(result => {
      console.log(result);
    });
  }

  get500ServerError() {
    this.http.get(this.baseUrl + '/serverError').subscribe(result => {
      console.log(result);
    });
    ;
  }

  getValidationErrors() {
    const user = {
      password: '1'
    };
    this.http.post('https://localhost:5001/api/account/register', user).subscribe(result => {

    }, errors => {
      this.errorsArray = errors;
    });
  }
}
