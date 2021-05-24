import {Component, OnInit} from '@angular/core';
import {AccountService} from '../_services/account.service';
import {User} from '../_models/User';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};

  constructor(public accountService: AccountService) {
  }

  ngOnInit() {
  }

  login() {
    this.accountService.login(this.model).subscribe(result => {
      console.log(result);
    }, error => {
      console.log(error);
    });
  }

  logout() {
    this.accountService.logout();
  }
}
