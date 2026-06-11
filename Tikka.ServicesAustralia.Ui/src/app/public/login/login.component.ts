import { ChangeDetectionStrategy, Component, inject, model, signal } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { tap, catchError, throwError, of } from 'rxjs';
import { Router } from '@angular/router';

import { MatSnackBar } from '@angular/material/snack-bar';

import { ApiException } from '../../layout/interfaces'
import { LoginRequest } from '../interfaces';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  standalone: false,
})
export class LoginComponent {

  loginForm: FormGroup = new FormGroup({
    usernameOrEmail: new FormControl(null, [Validators.required]),
    password: new FormControl(null, [Validators.required]),
    stayLoggedIn: new FormControl(false),
  });

  protected readonly isLoading = signal(false);
  validationError!: ApiException | null;

  constructor(
    private authService: AuthService,
    private router: Router,
    private snackbar: MatSnackBar,
  ) { }

  login() {
    if (!this.loginForm.valid) {
      return;
    }

    this.isLoading.set(true);
    this.validationError = null;

    const request: LoginRequest = {
      usernameOrEmail: this.loginForm.value.usernameOrEmail,
      password: this.loginForm.value.password,
      stayLoggedIn: this.loginForm.value.stayLoggedIn,
    };

    this.authService.login(request).pipe(
      // route to protected/dashboard, if login was successfull
      tap(() => {
        this.isLoading.set(false);
        this.router.navigate(['../../layout/home'])
      }),
      catchError(res => {
        if (res.status === 0) {
          // Client-side or network error
          console.error('An error occurred:', res.error);
        } else {
          // Backend returned an unsuccessful response code
          this.validationError = res.error;
          this.isLoading.set(false);
          this.validationError = {
            message: 'Login failed',
            details: 'Login failed',
          };
        }
        return of(res);
      })
    ).subscribe();
  }

}
