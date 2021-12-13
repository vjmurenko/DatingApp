import {ActivatedRouteSnapshot, Resolve, RouterStateSnapshot} from '@angular/router';
import {Member} from '../_models/Member';
import {Observable} from 'rxjs';
import {Injectable} from '@angular/core';
import {MembersService} from '../_services/members.service';

@Injectable({
  providedIn: 'root'
})
export class MemberDetailedResolver implements Resolve<Member> {
  constructor(private memberService: MembersService) {
  }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<Member> {
    return this.memberService.getMemberByName(route.paramMap.get('username'));
  }

}