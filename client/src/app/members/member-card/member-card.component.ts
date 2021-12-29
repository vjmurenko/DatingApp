import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Member} from '../../_models/Member';
import {MembersService} from '../../_services/members.service';
import {ToastrService} from 'ngx-toastr';
import {PresenceService} from '../../_services/presence.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {
users: string[] = [];
  @Input() member: Member;
  @Output() updatePage = new EventEmitter();

  constructor(public presense: PresenceService,
              private memberService: MembersService,
              private toastrService: ToastrService) {
  }

  ngOnInit(): void {
  }

  likeUser(member: Member) {
    this.memberService.addUserLike(member.username).subscribe(message => {
      this.toastrService.success(message.value + ' ' + member.knownAs);
      this.updatePage.emit();
    });
  }
}
