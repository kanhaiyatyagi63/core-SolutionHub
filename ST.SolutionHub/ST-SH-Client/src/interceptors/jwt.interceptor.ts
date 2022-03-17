import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError, filter, switchMap, take } from 'rxjs/operators';
import { environment } from 'environments/environment';
import { AuthenticationService } from 'services/authentication.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {


  private isRefreshing = false;
  private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);

  constructor(private authenticationService: AuthenticationService) {
  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    debugger;
    // Add Authorization to urls which starts with base url
    if (request.url.startsWith(environment.apiBaseUrl)) {
      // add authorization header with jwt token if available
      const token = this.authenticationService.getJwtToken();
      if (token) {
        request = this.addToken(request, token);
      }
    }

    return next.handle(request).pipe(catchError(error => {
      const newLocal = error instanceof HttpErrorResponse && error.status === 401
        && !request.url.includes('/auth/refresh')
        && !request.url.includes('/auth/logout')
        && !request.url.includes('/auth/login');
      if (newLocal) {
        return this.handle401Error(request, next);
      } else {
        return throwError(error);
      }
    }));
  }

  private addToken(request: HttpRequest<any>, token: string) {
    return request.clone({
      setHeaders: {
        'Authorization': `Bearer ${token}`
      }
    });
  }

  private handle401Error(request: HttpRequest<any>, next: HttpHandler) {
    if (!this.isRefreshing) {
      this.isRefreshing = true;
      this.refreshTokenSubject.next(null);
      return this.authenticationService.refreshToken()
        .pipe(switchMap((token: any) => {
          this.isRefreshing = false;
          this.refreshTokenSubject.next(token.jwt);
          return next.handle(this.addToken(request, token.jwt));
        }));
    } else {
      return this.refreshTokenSubject.pipe(
        filter(token => token != null),
        take(1),
        switchMap(jwt => {
          return next.handle(this.addToken(request, jwt));
        }));
    }
  }
}
