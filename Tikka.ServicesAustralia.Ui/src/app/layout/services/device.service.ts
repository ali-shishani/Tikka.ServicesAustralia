import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http'
import { map, Observable, of, switchMap, tap } from 'rxjs';

import { MatSnackBar } from '@angular/material/snack-bar';

import { environment } from '../../../environments/environment';
import { DeviceInformationResponseWrapper, ActivateDeviceResponseWrapper, RefreshDeviceKeyResponseWrapper } from '../interfaces';

@Injectable({
  providedIn: 'root'
})
export class DeviceService {

  constructor(
    private http: HttpClient,
    private snackbar: MatSnackBar
  ) { }

  protected readonly environment = environment;

  getSystemInformation(): Observable<DeviceInformationResponseWrapper> {
    return this.http.get<DeviceInformationResponseWrapper>(environment.apiUrl + '/api/device/getinfo').pipe(
      tap((res) => {
      }),
    );
  }

  activateDevice(activationCode: string): Observable<ActivateDeviceResponseWrapper> {
    const params = new HttpParams().set('activationCode', activationCode);
    return this.http.put<ActivateDeviceResponseWrapper>(environment.apiUrl + '/api/device/activate', {}, { params }).pipe(
      tap((res) => {
      }),
    );
  }

  refreshDeviceKey(): Observable<RefreshDeviceKeyResponseWrapper> {
    return this.http.put<RefreshDeviceKeyResponseWrapper>(environment.apiUrl + '/api/device/refreshkey', {}).pipe(
      tap((res) => {
      }),
    );
  }
}
