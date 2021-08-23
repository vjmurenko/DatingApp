import {Component, OnInit} from '@angular/core';
import {Member} from '../_models/Member';
import {MembersService} from '../_services/members.service';
import {Pagination} from '../_models/pagination';
import {PageChangedEvent} from 'ngx-bootstrap/pagination';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {

  memberList: Partial<Member[]> = [];
  predicate: string = 'liked';
  pageNumber: number = 1;
  pageSize: number = 5;
  pagination: Pagination;

  constructor(private memberService: MembersService) {
  }

  ngOnInit() {
    this.loadLikedUsers();
  }

  loadLikedUsers() {
    this.memberService.getUserLikes(this.predicate, this.pageSize, this.pageNumber).subscribe(result => {
      this.memberList = result.result;
      this.pagination = result.pagination;
    });
  }

  pageChanged(event: PageChangedEvent) {
    this.pageNumber = event.page;
    this.loadLikedUsers();
  }
}
