import { Component, inject, signal } from '@angular/core';
import { MediaMatcher } from '@angular/cdk/layout';
import { Router } from '@angular/router';
import { tap } from 'rxjs/operators';

import {
  MAT_DIALOG_DATA,
  MatDialog,
  MatDialogActions,
  MatDialogClose,
  MatDialogContent,
  MatDialogRef,
  MatDialogTitle,
} from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';

import { AuthService } from './public/services/auth.service';
import { UsersService } from './layout/services/users.service'
import { ChangePasswordComponent } from './layout/components/users/change-password/change-password.component'

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.css'
})
export class AppComponent {
  showFiller = false;

  protected readonly isMobile = signal(true);

  private readonly _mobileQuery: MediaQueryList;
  private readonly _mobileQueryListener: () => void;

  readonly changePasswordDialog = inject(MatDialog);

  constructor(
    private authService: AuthService,
    private router: Router,
    private snackbar: MatSnackBar
  ) {
    const media = inject(MediaMatcher);

    this._mobileQuery = media.matchMedia('(max-width: 600px)');
    this.isMobile.set(this._mobileQuery.matches);
    this._mobileQueryListener = () => this.isMobile.set(this._mobileQuery.matches);
    this._mobileQuery.addEventListener('change', this._mobileQueryListener);
  }

  isUserLoggedIn() {
    if (this.authService.getToken()) return true;
    return false;
  }

  logout() {
    this.authService.logOut().pipe(
      // route to protected/dashboard, if logout was successfull
      tap(() => this.router.navigate(['']))
    ).subscribe();
  }

  ChangePassword() {
    const dialogRef = this.changePasswordDialog.open(ChangePasswordComponent, {
      width: '400px',
      data: { title: 'Change password', output: null },
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {
        this.snackbar.open('Change password Successfull', 'Close', { duration: 2000, horizontalPosition: 'right', verticalPosition: 'top' });
      }
    });
  }
}
