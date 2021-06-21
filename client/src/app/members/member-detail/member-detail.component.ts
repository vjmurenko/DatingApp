import {Component, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {MembersService} from '../../_services/members.service';

import {Member} from '../../_models/Member';
import {NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions} from '@kolkov/ngx-gallery';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  member: Member;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];

  constructor(private route: ActivatedRoute, private membersService: MembersService) {

  }

  ngOnInit() {
    this.loadMember();

    this.galleryOptions = [{
      width: '500px',
      height: '500px',
      imagePercent: 100,
      thumbnailsColumns: 4,
      imageAnimation: NgxGalleryAnimation.Slide,
      preview: false
    }
    ];
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

  loadMember() {
    this.membersService
      .getMemberByName(this.route.snapshot.paramMap.get('username'))
      .subscribe(result => {
          this.member = result;
          this.galleryImages = this.getImages();
        }
      );
  }
}
