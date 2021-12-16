import {Directive, Input, TemplateRef, ViewContainerRef} from '@angular/core';
import {AccountService} from '../_services/account.service';
import {User} from '../_models/User';

@Directive({
  selector: '[appHasRole]'
})
export class HasRoleDirective {
  @Input() appHasRole: string[];
  user: User;

  constructor(private viewContainerRef: ViewContainerRef,
              private templateRef: TemplateRef<any>,
              private accountService: AccountService) {

    this.accountService.currentUser$.subscribe(user => {
      this.user = user;
    });
  }

  ngOnInit() {
    if (!this.user?.roles || this.user == null) {
      this.viewContainerRef.clear();
    }

    if (this.user?.roles.some(r => this.appHasRole.includes(r))) {
      this.viewContainerRef.createEmbeddedView(this.templateRef);
    } else {
      this.viewContainerRef.clear();
    }
  }
}
