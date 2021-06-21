import {Component, OnInit} from '@angular/core';
import {HttpClient} from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  registerMode: boolean = false;
  users: any = [];

  constructor(private http: HttpClient) {
  }

  ngOnInit() {

  }

  registerToggle(){
    this.registerMode = true;
  }



  cancelRegisterMode(event: boolean){
    this.registerMode = event;
  }

}
