import { Component, OnInit } from '@angular/core';
import {MembersService} from '../../_services/members.service';
import {Member} from '../../_models/Member';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

  constructor(private _memberService: MembersService) { }

  memberList: Member[] = [];

  ngOnInit() {
    this._memberService.getMembers().subscribe(members => {
      this.memberList = members;
    })
  }

}
