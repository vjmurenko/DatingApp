import {Component, Input, OnInit} from '@angular/core';
import {Member} from '../../_models/Member';
import {MembersService} from '../../_services/members.service';
import {ToastrService} from 'ngx-toastr';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {

  @Input() member: Member;

  constructor(private memberService: MembersService, private toastrService: ToastrService) { }

  ngOnInit(): void {
  }

  likeUser(member: Member){
    this.memberService.addUserLike(member.username).subscribe(() => {
      this.toastrService.success("You have liked " + member.knownAs)
    });
  }

}
