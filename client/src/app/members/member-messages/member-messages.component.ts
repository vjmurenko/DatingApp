import {ChangeDetectionStrategy, Component, Input, OnInit, ViewChild} from '@angular/core';
import {Message} from '../../_models/Message';
import {MessageService} from '../../_services/message.service';
import {NgForm} from '@angular/forms';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit {

  @Input() username: string;
  @Input() messages: Message[];
  @ViewChild('messageForm') messageForm: NgForm;
  messageContent: string;
  loading: boolean = false;

  constructor(public messageService: MessageService) {
  }

  ngOnInit() {
  }

  sendMessage() {
    this.loading = true;
    this.messageService.sendMessage(this.username, this.messageContent).then(response => {
      this.messageForm.reset();
    }).finally(() => this.loading = false);
  }
}
