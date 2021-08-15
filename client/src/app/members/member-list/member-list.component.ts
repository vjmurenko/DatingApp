import {Component, OnInit} from '@angular/core';
import {MembersService} from '../../_services/members.service';
import {Member} from '../../_models/Member';
import {Pagination} from '../../_models/pagination';
import {UserParams} from '../../_models/UserParams';
import {User} from '../../_models/User';
import {AccountService} from '../../_services/account.service';
import {take} from 'rxjs/operators';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  members: Member[];
  pagination: Pagination;
  userParams: UserParams;
  user: User;
  genderList = [{value: 'male', display: 'Males'}, {value: 'female', display: 'Females'}];

  constructor(private _memberService: MembersService) {
    this.userParams = this._memberService.getUserParams();
  }

  ngOnInit() {
    this.loadMembers();
  }

  loadMembers() {
    this._memberService.setUserParams(this.userParams);
    this._memberService.getMembers(this.userParams).subscribe(response => {
      this.members = response.result;
      this.pagination = response.pagination;
    });
  }

  pageChanged(event: any) {
    this.userParams.pageNumber = event.page;
    this._memberService.setUserParams(this.userParams);
    this.loadMembers();
  }

  resetFilters() {
    this.userParams = this._memberService.resetUserParams();
    this.loadMembers();
  }
}
