import { environment } from './../environments/environment';
import { AuthenticateRequestModel, AuthenticateResponseModel, SocialUser } from './../models/authenticationModel';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { catchError, mapTo, tap } from 'rxjs/operators';
import jwt_decode from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  private readonly JWT_TOKEN = 'JWT_TOKEN';
  private readonly REFRESH_TOKEN = 'REFRESH_TOKEN';

  private userSubject$ = new BehaviorSubject<AuthenticateResponseModel>(null);
  public user$ = this.userSubject$.asObservable();

  public userNameSubject$ = new BehaviorSubject<string>(null);
  public userNameDetai$ = this.userNameSubject$.asObservable();

  constructor(private http: HttpClient) {
  }


  public login(authenticateRequestModel: AuthenticateRequestModel): Observable<boolean> {
    const formData: FormData = new FormData();
    formData.append('userName', authenticateRequestModel.userName);
    formData.append('password', authenticateRequestModel.password);

    return this.http.post<any>(`${environment.apiBaseUrl}/auth/login`, formData)
      .pipe(
        tap((user: AuthenticateResponseModel) => {
          this.doLoginUser(user);
        }),
        mapTo(true),
        catchError(error => {
          throw error;
        }));
  }

  public externalLogin(model: SocialUser) {
    return this.http.post<any>(`${environment.apiBaseUrl}/auth/externalLogin`, model)
      .pipe(
        tap((user: AuthenticateResponseModel) => {
          this.doLoginUser(user);
        }),
        mapTo(true),
        catchError(error => {
          throw error;
        }));
  }

  public logout() {
    const formData: FormData = new FormData();
    formData.append('refreshToken', this.getRefreshToken());
    formData.append('token', this.getJwtToken());
    return this.http.post<any>(`${environment.apiBaseUrl}/auth/logout`, formData)
      .pipe(
        tap(() => {
          this.doLogoutUser();
        }),
        mapTo(true),
        catchError(error => {
          this.doLogoutUser();
          return of(false);
        }));
  }


  public isLoggedIn() {
    return !!this.getJwtToken();
  }

  public refreshToken() {
    const formData: FormData = new FormData();
    formData.append('refreshToken', this.getRefreshToken());
    formData.append('token', this.getJwtToken());
    return this.http.post<any>(`${environment.apiBaseUrl}/auth/refresh`, formData)
      .pipe(
        tap((user: AuthenticateResponseModel) => {
          this.doLoginUser(user);
        }),
        mapTo(true),
        catchError(error => {
          this.doLogoutUser();
          return of(false);
        }));
  }

  public isTokenExpired(): boolean {
    const token = this.getJwtToken();
    if (!token) { return true; }

    const date = this.getTokenExpirationDate(token);
    if (date === undefined) { return false; }
    return !(date.valueOf() > new Date().valueOf());
  }


  // Used in interceptor
  public getJwtToken() {
    return localStorage.getItem(this.JWT_TOKEN);
  }

  private doLoginUser(authenticateResponseModel: AuthenticateResponseModel) {
    this.storeTokens(authenticateResponseModel);
    this.userSubject$.next(authenticateResponseModel);
  }

  private doLogoutUser() {
    // Clear all data
    localStorage.clear();
    this.removeTokens();
    this.userSubject$.next(null);
  }

  private getRefreshToken() {
    return localStorage.getItem(this.REFRESH_TOKEN);
  }

  public storeTokens(authenticateResponseModel: AuthenticateResponseModel) {
    localStorage.setItem(this.JWT_TOKEN, authenticateResponseModel.token);
    localStorage.setItem(this.REFRESH_TOKEN, authenticateResponseModel.refreshToken);
  }


  private removeTokens() {
    localStorage.removeItem(this.JWT_TOKEN);
    localStorage.removeItem(this.REFRESH_TOKEN);
  }

  private getTokenExpirationDate(token: string): Date {
    const decoded: any = jwt_decode(token);
    if (decoded.exp === undefined) {
      return null;
    }
    const date = new Date(0);
    date.setUTCSeconds(decoded.exp);
    return date;
  }

}

