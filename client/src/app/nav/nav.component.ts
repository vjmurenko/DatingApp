import {Component, OnInit} from '@angular/core';
import {AccountService} from '../_services/account.service';
import {User} from '../_models/User';
import {Router, RouterLinkActive} from '@angular/router';
import {ToastrService} from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};

  constructor(public accountService: AccountService, private router: Router, private toaster: ToastrService) {
  }

  ngOnInit() {
  }

  login() {
    this.accountService.login(this.model).subscribe(result => {
      this.router.navigateByUrl('/members');
      this.toaster.success('Success login');
    }, error => {
      console.log(error);
      this.toaster.error(error.error);


    });
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }
}
