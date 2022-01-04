import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../environments/environment';
import {Message} from '../_models/Message';
import {BehaviorSubject, Observable} from 'rxjs';
import {PaginatedResult, Pagination} from '../_models/pagination';
import {getPageParams, getPaginatedResult} from './paginationHelper';
import {HubConnection, HubConnectionBuilder} from '@microsoft/signalr';
import {User} from '../_models/User';
import {take} from 'rxjs/operators';
import {Group} from '../_models/Group';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  private hubConnection: HubConnection;
  private messageThreadSource = new BehaviorSubject<Message[]>([]);
  messageThread$ = this.messageThreadSource.asObservable();


  constructor(private _httpClient: HttpClient) {
  }

  getMessagesForUser(pageSize, pageNumber, container): Observable<PaginatedResult<Message[]>> {
    let params = getPageParams(pageSize, pageNumber);
    params = params.append('Container', container);

    return getPaginatedResult(environment.apiUrl + 'messages', this._httpClient, params);
  }

  async sendMessage(username: string, content: string): Promise<Message> {
    return this.hubConnection.invoke('SendMessage', {recipientUserName: username, content}).catch(error => console.log(error));
  }

  deleteMessage(id: number) {
    return this._httpClient.delete(environment.apiUrl + `messages/${id}`);
  }

  creeateHubConnection(user: User, otherUserName: string) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(environment.hubUrl + 'message?user=' + otherUserName, {
          accessTokenFactory: () => user.token
        }
      )
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch(error => console.log(error));

    this.hubConnection.on('ReceiveMessageThread', messages => {
      this.messageThreadSource.next(messages);
    });

    this.hubConnection.on('NewMessage', message => {
      this.messageThread$.pipe(take(1)).subscribe(messages => {
        this.messageThreadSource.next([...messages, message]);
      });
    });

    this.hubConnection.on('UpdatedGroup', (group: Group) => {
      if (group.connections.some(c => c.userName === otherUserName)) {
        this.messageThread$.pipe(take(1)).subscribe(messages => {
          messages.forEach(message => {
            if (!message.dateRead) {
              message.dateRead = new Date(Date.now());
            }
          });
          this.messageThreadSource.next([...messages]);
        });
      }
    });
  }

  stopHubConnection() {
    if (this.hubConnection) {
      this.hubConnection.stop().catch(error => console.log(error));
    }
  }
}
