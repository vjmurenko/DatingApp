import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../environments/environment';
import {Message} from '../_models/Message';
import {Observable} from 'rxjs';
import {PaginatedResult, Pagination} from '../_models/pagination';
import {getPageParams, getPaginatedResult} from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  constructor(private _httpClient: HttpClient) {
  }

  getMessagesForUser(pageSize, pageNumber, container): Observable<PaginatedResult<Message[]>> {
    let params = getPageParams(pageSize, pageNumber);
    params = params.append('Container', container);

    return getPaginatedResult(environment.apiUrl + 'messages', this._httpClient, params);
  }

  getMessageThread(username: string): Observable<Message[]> {
    return this._httpClient.get<Message[]>(environment.apiUrl + `messages/thread/${username}`);
  }

  sendMessage(username: string, content: string): Observable<Message> {
    return this._httpClient.post<Message>(environment.apiUrl + 'messages', {recipientUserName: username, content});
  }

  deleteMessage(id: number) {
    return this._httpClient.delete(environment.apiUrl + `messages/${id}`);
  }

}
