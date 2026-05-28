import { Component, inject, model, signal } from '@angular/core';

import { tap, Observable } from 'rxjs';

import {
  MAT_DIALOG_DATA,
  MatDialog,
  MatDialogActions,
  MatDialogClose,
  MatDialogContent,
  MatDialogRef,
  MatDialogTitle,
} from '@angular/material/dialog';

import { DeviceService } from '../../services/device.service';
import { DeviceInformationResponse, DeviceInformationResponseWrapper, ActivateDeviceResponseWrapper, RefreshDeviceKeyResponseWrapper } from '../../interfaces';
import { DialogComponent } from '../dialog/dialog.component';
import { LogWindowComponent } from '../dialog/log-window/log-window.component';
import { ConfirmationWindowComponent } from '../dialog/confirmation-window/confirmation-window.component';

@Component({
  selector: 'app-home',
  standalone: false,
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  deviceInfo: DeviceInformationResponse | null = null;

  constructor(
    private deviceService: DeviceService
  ) {
    this.loadSystemInfo().pipe(
      tap((res: DeviceInformationResponseWrapper) => {
        this.deviceInfo = res.data;
        this.isInitialised.set(true);
      })
    ).subscribe();
  }

  protected readonly isLoading = signal(false);
  protected readonly isInitialised = signal(false);

  readonly activationCode = signal('');
  readonly dialog = inject(MatDialog);

  loadSystemInfo(): Observable<DeviceInformationResponseWrapper> {
    return this.deviceService.getSystemInformation();
  }

  activateDevice(): void {
    const dialogRef = this.dialog.open(DialogComponent, {
      width: '700px',
      data: { title: 'Activation code', description: 'What is your activation code?', output: this.activationCode() },
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {
        this.isLoading.set(true);
        this.activationCode.set(result);
        this.deviceService.activateDevice(result).pipe(
          tap((res: ActivateDeviceResponseWrapper) => {

            this.loadSystemInfo().pipe(
              tap((deviceInfo: DeviceInformationResponseWrapper) => {
                this.deviceInfo = deviceInfo.data;
                this.isLoading.set(false);
                
                this.activationCode.set('');

                const logWindowRef = this.dialog.open(LogWindowComponent, {
                  width: '700px', minHeight: "700px",
                  data: { title: 'Activation log', description: 'The activation process is showing the following log:', log: res.data },
                });
              })
            ).subscribe();
          })
        ).subscribe();
      }
    });
  }

  refreshDeviceKey(): void {
    const dialogRef = this.dialog.open(ConfirmationWindowComponent, {
      width: '700px',
      data: { title: 'Refresh device key', description: 'You are about to refresh the device key. Would you like to continue?' },
    });
    
    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {
        this.isLoading.set(true);
        this.deviceService.refreshDeviceKey().pipe(
          tap((res: RefreshDeviceKeyResponseWrapper) => {

            this.loadSystemInfo().pipe(
              tap((deviceInfo: DeviceInformationResponseWrapper) => {
                this.deviceInfo = deviceInfo.data;
                this.isLoading.set(false);

                const logWindowRef = this.dialog.open(LogWindowComponent, {
                  width: '700px', minHeight: "700px",
                  data: { title: 'Activation log', description: 'The activation process is showing the following log:', log: res.data },
                });
              })
            ).subscribe();
          })
        ).subscribe();
      }
    });
  }
}
