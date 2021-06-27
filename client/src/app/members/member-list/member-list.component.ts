import { Component, OnInit } from '@angular/core';
import {MembersService} from '../../_services/members.service';
import {Member} from '../../_models/Member';
import {Observable} from 'rxjs';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

  constructor(private _memberService: MembersService) { }

  memberList$: Observable<Member[]>;

  ngOnInit() {
     this.memberList$ = this._memberService.getMembers();
  }

}
