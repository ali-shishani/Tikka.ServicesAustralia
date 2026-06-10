import { LOCALSTORAGE_TOKEN_KEY, LOCALSTORAGE_REFRESHTOKEN_KEY, LOCALSTORAGE_TREFRESHTOKENEXPIRY_KEY } from './../../app.module';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router} from '@angular/router';
import { map, Observable, of, switchMap, tap, catchError, throwError, BehaviorSubject } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { MatSnackBar } from '@angular/material/snack-bar';
import { LoginRequest, LoginResponse, RegisterRequest, RegisterResponse, LogoutRequest, RefreshTokenRequest, RefreshTokenResponse } from '../interfaces';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(
    private http: HttpClient,
    private router: Router,
    private snackbar: MatSnackBar,
    private jwtService: JwtHelperService
  ) { }

  protected readonly environment = environment;

  // Track if a refresh token API call is currently in progress
  public isRefreshing$ = new BehaviorSubject<boolean>(false);

  // Holds the new token stream to notify queued parallel requests
  public refreshTokenSubject$ = new BehaviorSubject<string | null>(null);

  /*
   Due to the '/api' the url will be rewritten by the proxy, e.g. to http://localhost:8080/api/auth/login
   this is specified in the src/proxy.conf.json
   the proxy.conf.json listens for /api and changes the target. You can also change this in the proxy.conf.json

   The `..of()..` can be removed if you have a real backend, at the moment, this is just a faked response
  */
  login(loginRequest: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(environment.authApiUrl + '/api/account/login', loginRequest).pipe(
      tap((res) => {
        localStorage.setItem(LOCALSTORAGE_TOKEN_KEY, res.token);
        localStorage.setItem(LOCALSTORAGE_REFRESHTOKEN_KEY, res.refreshToken);
        localStorage.setItem(LOCALSTORAGE_TREFRESHTOKENEXPIRY_KEY, res.refreshTokenExpireTime);
      }),
      tap(() => this.snackbar.open('Login Successfull', 'Close', {
        duration: 2000, horizontalPosition: 'right', verticalPosition: 'top'
      }))
    );
  }

  logOut(): Observable<string> {
    const token = localStorage.getItem(LOCALSTORAGE_TOKEN_KEY);
    const refreshToken = localStorage.getItem(LOCALSTORAGE_REFRESHTOKEN_KEY);
    const refreshTokenExpiry = localStorage.getItem(LOCALSTORAGE_TREFRESHTOKENEXPIRY_KEY);
    const LogoutRequest: LogoutRequest = {
      token: token!,
      refreshToken: refreshToken!,
      refreshTokenExpireTime: refreshTokenExpiry!,
    };
    return this.http.post(environment.authApiUrl + '/api/account/logout', LogoutRequest, { responseType: 'text' }).pipe(
      tap(() => {
        localStorage.removeItem(LOCALSTORAGE_TOKEN_KEY);
        localStorage.removeItem(LOCALSTORAGE_REFRESHTOKEN_KEY);
        localStorage.removeItem(LOCALSTORAGE_TREFRESHTOKENEXPIRY_KEY);
        this.snackbar.open('Logout Successfull', 'Close', { duration: 2000, horizontalPosition: 'right', verticalPosition: 'top' });
      })
    );
  }

  /*
   The `..of()..` can be removed if you have a real backend, at the moment, this is just a faked response
  */
  register(registerRequest: RegisterRequest): Observable<RegisterResponse> {
    return this.http.post<RegisterResponse>(environment.authApiUrl + '/api/account/register', registerRequest).pipe(
      tap((res: RegisterResponse) => this.snackbar.open(`User created successfully`, 'Close', {
        duration: 2000, horizontalPosition: 'right', verticalPosition: 'top'
      }))
    );
  }

  /*
   Get the user fromt the token payload
   */
  getLoggedInUser() {
    const decodedToken = this.jwtService.decodeToken();
    return decodedToken.user;
  }

  /*
   Get the user fromt the token payload
   */
  getToken() {
    return localStorage.getItem(LOCALSTORAGE_TOKEN_KEY);
  }

  /*
   refresh the token
   */
  refreshToken(): Observable<RefreshTokenResponse>{
    const request: RefreshTokenRequest = {
      token: localStorage.getItem(LOCALSTORAGE_TOKEN_KEY)!,
      refreshToken: localStorage.getItem(LOCALSTORAGE_REFRESHTOKEN_KEY)!,
    };

    return this.http.post<RefreshTokenResponse>(environment.authApiUrl + '/api/account/refresh-token', request).pipe(
      tap((res: RefreshTokenResponse) => {
        localStorage.removeItem(LOCALSTORAGE_TOKEN_KEY);
        localStorage.removeItem(LOCALSTORAGE_REFRESHTOKEN_KEY);
        localStorage.removeItem(LOCALSTORAGE_TREFRESHTOKENEXPIRY_KEY);
        localStorage.setItem(LOCALSTORAGE_TOKEN_KEY, res.token);
        localStorage.setItem(LOCALSTORAGE_REFRESHTOKEN_KEY, res.refreshToken);
        localStorage.setItem(LOCALSTORAGE_TREFRESHTOKENEXPIRY_KEY, res.refreshTokenExpireTime);
        this.isRefreshing$.next(false);
      }),
      catchError((refreshError) => {
        // If the refresh token itself is expired or invalid, clear all
        localStorage.removeItem(LOCALSTORAGE_TOKEN_KEY);
        localStorage.removeItem(LOCALSTORAGE_REFRESHTOKEN_KEY);
        localStorage.removeItem(LOCALSTORAGE_TREFRESHTOKENEXPIRY_KEY);
        this.isRefreshing$.next(false);
        this.router.navigate(['']);
        return throwError(() => refreshError);
      }
    ));
  }
}
