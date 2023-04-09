import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { Router } from '@angular/router';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private router: Router) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if(localStorage.getItem('token') != null) {
      const cloneReq = request.clone({
        headers: request.headers.set('Authorization', 'Bearer ' + localStorage.getItem('token'))
      });
      return next.handle(cloneReq).pipe(
        tap(
          success => {},
          error => {
            if(error.status == 401) {
              localStorage.removeItem('token');
              this.router.navigateByUrl('user/login');
            }
          }
        )
      )
    }
    else {
      return next.handle(request.clone());
    }
  }
}
