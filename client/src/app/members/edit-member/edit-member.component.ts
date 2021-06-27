import {Component, HostListener, OnInit, ViewChild} from '@angular/core';
import {AccountService} from '../../_services/account.service';
import {MembersService} from '../../_services/members.service';
import {User} from '../../_models/User';
import {Member} from '../../_models/Member';
import {take} from 'rxjs/operators';
import {ToastrService} from 'ngx-toastr';
import {NgForm} from '@angular/forms';

@Component({
  selector: 'app-edit-member',
  templateUrl: './edit-member.component.html',
  styleUrls: ['./edit-member.component.css']
})
export class EditMemberComponent implements OnInit {
  user: User;
  member: Member;
  @ViewChild('editForm') editForm: NgForm;

  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(private _accountService: AccountService,
              private _memberService: MembersService,
              private _toastrService: ToastrService) {
    this._accountService.currentUser$
      .pipe(take(1))
      .subscribe(user => this.user = user);
  }

  ngOnInit(): void {
    this.loadMember();

  }

  loadMember() {
    this._memberService.getMemberByName(this.user.username).subscribe(member => this.member = member);
  }

  updateMember() {
    this._memberService.updateMember(this.member).subscribe(() => {
      this._toastrService.success('Update member');
      this.editForm.reset(this.member);
    });
  }
}
