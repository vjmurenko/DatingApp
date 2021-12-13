import {HttpClient, HttpParams} from '@angular/common/http';
import {PaginatedResult} from '../_models/pagination';
import {map} from 'rxjs/operators';

export function getPageParams(pageSize: number, pageNumber: number): HttpParams {
  let params = new HttpParams();

  if (pageNumber != null && pageSize != null) {
    params = params.append('pageNumber', pageNumber.toString());
    params = params.append('pageSize', pageSize.toString());
  }
  return params;
}

export function getPaginatedResult<T>(url: string, http: HttpClient, params: HttpParams) {
  let paginatedResult = new PaginatedResult<T>();

  return http.get<T>(url, {observe: 'response', params})
    .pipe(map(result => {
      paginatedResult.result = result.body;

      if (result.headers.get('Pagination') != null) {
        paginatedResult.pagination = JSON.parse(result.headers.get('Pagination'));
      }

      return paginatedResult;
    }));
}