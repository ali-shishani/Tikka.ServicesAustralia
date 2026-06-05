import { ChangeDetectionStrategy, Component, inject, model, signal } from '@angular/core';
import { FormGroup, FormControl, Validators, FormGroupDirective, NgForm } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { tap, catchError, throwError, of } from 'rxjs';

import { ErrorStateMatcher } from '@angular/material/core';
import {
  MAT_DIALOG_DATA,
  MatDialogRef,
} from '@angular/material/dialog';

import { ChangePasswordValidators } from '../../../../public/custom-validator';
import { ApiException, ChangePasswordRequest, changePasswordResponseWrapper } from '../../../interfaces'
import { UsersService } from '../../../services/users.service'

export interface ChangePasswordData {
  title: string;
}

export class MyErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
    const isSubmitted = form && form.submitted;
    return !!(control && control.invalid && (control.dirty || control.touched || isSubmitted));
  }
}

@Component({
  selector: 'app-change-password',
  standalone: false,
  templateUrl: './change-password.component.html',
  styleUrl: './change-password.component.css'
})
export class ChangePasswordComponent {
  readonly dialogRef = inject(MatDialogRef<ChangePasswordComponent>);
  readonly data = inject<ChangePasswordData>(MAT_DIALOG_DATA);

  validationErrors: ApiException[] = [];

  protected readonly isLoading = signal(false);

  matcher = new MyErrorStateMatcher();
  userForm = new FormGroup({
    currentPassword: new FormControl(null, [Validators.required]),
    newPassword: new FormControl(null, [Validators.required]),
    passwordConfirm: new FormControl(null, [Validators.required])
  },
    // add custom Validators to the form, to make sure that password and passwordConfirm are equal
    {
      validators: ChangePasswordValidators.passwordsMatching
    });

  constructor(
    private usersService: UsersService
  ) {
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  submit(): void {
    if (!this.userForm.valid) {
      return;
    }

    this.isLoading.set(true);
    this.validationErrors = [];

    var request: ChangePasswordRequest =
    {
      currentPassword: this.userForm.value.currentPassword!,
      newPassword: this.userForm.value.newPassword!,
    };

    this.usersService.changePassword(request).pipe(
      tap((res: changePasswordResponseWrapper) => {
        const result = res.data;
        this.isLoading.set(false);
        this.dialogRef.close(result);
      }),
      catchError(res => {
        if (res.status === 0) {
          // Client-side or network error
          console.error('An error occurred:', res.error);
        } else {
          // Backend returned an unsuccessful response code
          const resWrapper: changePasswordResponseWrapper = res.error;
          this.validationErrors = resWrapper.errors;
          this.isLoading.set(false);
        }
        return of(res);
      })
    ).subscribe();
  }
}
