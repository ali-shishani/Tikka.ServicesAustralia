import { LOCALSTORAGE_TOKEN_KEY, LOCALSTORAGE_REFRESHTOKEN_KEY, LOCALSTORAGE_TREFRESHTOKENEXPIRY_KEY } from './../../app.module';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, of, switchMap, tap } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { MatSnackBar } from '@angular/material/snack-bar';
import { LoginRequest, LoginResponse, RegisterRequest, RegisterResponse, LogoutRequest } from '../interfaces';

export const fakeLoginResponse: LoginResponse = {
  // fakeAccessToken.....should all come from real backend
  token: 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c',
  refreshToken: 'fakeRefreshToken...should al come from real backend',
  refreshTokenExpireTime: '2026-08-08'
}

export const fakeRegisterResponse: RegisterResponse = {
  success: true,
  message: 'Registration sucessfull.',
  email: 'user@example.com'
}

export const config = {
  authBaseUrl: 'https://localhost:7228',
  baseUrl: 'https://localhost:7228',
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(
    private http: HttpClient,
    private snackbar: MatSnackBar,
    private jwtService: JwtHelperService
  ) { }

  /*
   Due to the '/api' the url will be rewritten by the proxy, e.g. to http://localhost:8080/api/auth/login
   this is specified in the src/proxy.conf.json
   the proxy.conf.json listens for /api and changes the target. You can also change this in the proxy.conf.json

   The `..of()..` can be removed if you have a real backend, at the moment, this is just a faked response
  */
  login(loginRequest: LoginRequest): Observable<LoginResponse> {
    // return of(fakeLoginResponse).pipe(
    //   tap((res: LoginResponse) => localStorage.setItem(LOCALSTORAGE_TOKEN_KEY, res.accessToken)),
    //   tap(() => this.snackbar.open('Login Successfull', 'Close', {
    //     duration: 2000, horizontalPosition: 'right', verticalPosition: 'top'
    //   }))
    // );
    // return this.http.post<LoginResponse>('/api/auth/login', loginRequest).pipe(
    // tap((res: LoginResponse) => localStorage.setItem(LOCALSTORAGE_TOKEN_KEY, res.accessToken)),
    // tap(() => this.snackbar.open('Login Successfull', 'Close', {
    //  duration: 2000, horizontalPosition: 'right', verticalPosition: 'top'
    // }))
    // );
    return this.http.post<LoginResponse>(config.authBaseUrl + '/api/account/login', loginRequest).pipe(
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
    return this.http.post(config.authBaseUrl + '/api/account/logout', LogoutRequest, { responseType: 'text' }).pipe(
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
    // TODO
    // return of(fakeRegisterResponse).pipe(
    //   tap((res: RegisterResponse) => this.snackbar.open(`User created successfully`, 'Close', {
    //     duration: 2000, horizontalPosition: 'right', verticalPosition: 'top'
    //   })),
    // );
    // return this.http.post<RegisterResponse>('/api/auth/register', registerRequest).pipe(
    // tap((res: RegisterResponse) => this.snackbar.open(`User created successfully`, 'Close', {
    //  duration: 2000, horizontalPosition: 'right', verticalPosition: 'top'
    // }))
    // )
    return this.http.post<RegisterResponse>(config.authBaseUrl + '/api/account/register', registerRequest).pipe(
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
}
