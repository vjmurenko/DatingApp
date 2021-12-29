import {Component, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {MembersService} from '../../_services/members.service';

import {Member} from '../../_models/Member';
import {NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions} from '@kolkov/ngx-gallery';
import {TabDirective, TabsetComponent} from 'ngx-bootstrap/tabs';
import {MessageService} from '../../_services/message.service';
import {Message} from '../../_models/Message';
import {AccountService} from '../../_services/account.service';
import {map, take} from 'rxjs/operators';
import {User} from '../../_models/User';
import {PresenceService} from '../../_services/presence.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit, OnDestroy {
  member: Member;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  messages: Message[] = [];
  user: User;

  @ViewChild('tabSet', {static: true}) tabSet: TabsetComponent;

  constructor(
    public presence: PresenceService,
    private route: ActivatedRoute,
    private membersService: MembersService,
    private _messageService: MessageService,
    private accountService: AccountService,
    private router: Router) {
    accountService.currentUser$.pipe(take(1)).subscribe(user => {
      this.user = user;
    });
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
  }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.member = data.member;
    });

    this.route.queryParams.subscribe(params => {
      params.tab ? this.selectTab(params.tab) : this.selectTab(0);
    });

    this.galleryOptions = [{
      width: '500px',
      height: '500px',
      imagePercent: 100,
      thumbnailsColumns: 4,
      imageAnimation: NgxGalleryAnimation.Slide,
      preview: false
    }
    ];

    this.galleryImages = this.getImages();
  }

  getImages(): NgxGalleryImage[] {
    const imageUrls = [];
    for (const photo of this.member.photos) {
      imageUrls.push({
        small: photo?.url,
        medium: photo?.url,
        big: photo?.url,
      });
    }
    return imageUrls;
  }

  onTabActivated(tabDirective: TabDirective) {
    if (tabDirective.heading === 'Messages' && this.messages.length === 0) {
      this._messageService.creeateHubConnection(this.user, this.member.username);
    } else {
      this._messageService.stopHubConnection();
    }
  }

  selectTab(tab: number) {
    this.tabSet.tabs[tab].active = true;
  }

  ngOnDestroy(): void {
    this._messageService.stopHubConnection();
  }
}
