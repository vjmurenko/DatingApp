import {Component, OnInit} from '@angular/core';
import {MessageService} from '../_services/message.service';
import {Message} from '../_models/Message';
import {Pagination} from '../_models/pagination';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {

  constructor(private _messageService: MessageService) {
  }

  messages: Message[] = [];
  pagination: Pagination;
  container = 'Unread';
  pageNumber = 1;
  pageSize = 10;
  loading: boolean = false;

  ngOnInit() {
    this.loadMessages();

  }

  loadMessages() {
    this.loading = true;
    this._messageService.getMessagesForUser(this.pageSize, this.pageNumber, this.container).subscribe(response => {
      this.messages = response.result;
      this.pagination = response.pagination;
      this.loading = false;
    });
  }

  pageChanged(event: any){
    this.pageNumber = event.page;
    this.loadMessages();
  }

  deleteMessage(id: number) {
    this._messageService.deleteMessage(id).subscribe(() => {
      this.messages.splice(this.messages.findIndex(m => m.id === id), 1);
    })
  }
}
