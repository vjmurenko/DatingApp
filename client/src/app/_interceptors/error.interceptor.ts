import {Injectable} from '@angular/core';
import {HttpRequest, HttpHandler, HttpEvent, HttpInterceptor} from '@angular/common/http';
import {Observable, of, throwError} from 'rxjs';
import {catchError} from 'rxjs/operators';
import {ToastrService} from 'ngx-toastr';
import {NavigationExtras, Router} from '@angular/router';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private toaster: ToastrService, private router: Router) {
  }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {

    return next.handle(request).pipe(catchError(error => {

      if (error) {
        switch (error.status) {
          case 400:
            if (error.error.errors) {
              const errorsArray = [];

              var a = error.error.errors;
              for (const key in error.error.errors){
                if(error.error.errors[key])
                  errorsArray.push(error.error.errors[key]);
              }

              throw errorsArray.flat();
            } else {
              this.toaster.error(error.statusText, error.status);
            }
            break;
          case 401:
            this.toaster.error(error.statusText, error.status);
            break;
          case 404:
            this.router.navigateByUrl('/not-found');

            break;
          case 500:
            const extras: NavigationExtras = {state: {error: error.error}};
            this.router.navigateByUrl('server-error', extras);
            break;
          default:
            this.toaster.error('Unexpected error');
            console.log(error);
            break;
        }
      }
      return throwError(error);
    }));
  }
}
