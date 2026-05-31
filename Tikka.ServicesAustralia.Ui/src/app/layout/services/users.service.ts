import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http'

import { map, Observable, of, switchMap, tap, catchError } from 'rxjs';

import { MatSnackBar } from '@angular/material/snack-bar';

import { environment } from '../../../environments/environment';
import { getUsersResponseWrapper } from '../interfaces';
import { RegisterRequest } from '../../public/interfaces';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  constructor(
    private http: HttpClient,
    private snackbar: MatSnackBar
  ) { }

  protected readonly environment = environment;

  getAll(): Observable<getUsersResponseWrapper> {
    return this.http.get<getUsersResponseWrapper>(environment.authApiUrl + '/api/user/getall').pipe(
      tap((res) => {
      }),
    );
  }

  register(registerRequest: RegisterRequest): Observable<getUsersResponseWrapper> {
    return this.http.post<getUsersResponseWrapper>(environment.authApiUrl + '/api/user/register', registerRequest).pipe(
      tap((res) => {
      }),
    );
  }
}
