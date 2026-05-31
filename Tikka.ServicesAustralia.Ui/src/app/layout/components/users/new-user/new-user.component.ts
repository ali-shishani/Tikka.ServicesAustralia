import { ChangeDetectionStrategy, Component, inject, model, signal } from '@angular/core';
import { FormGroup, FormControl, Validators, FormGroupDirective, NgForm } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { tap, catchError, throwError, of } from 'rxjs';

import { ErrorStateMatcher } from '@angular/material/core';
import {
  MAT_DIALOG_DATA,
  MatDialogRef,
} from '@angular/material/dialog';

import { getUsersResponse, getUsersResponseWrapper, ApiException } from '../../../interfaces'
import { UsersService } from '../../../services/users.service'
import { AuthService } from '../../../../public/services/auth.service';
import { CustomValidators } from '../../../../public/custom-validator';
import { RegisterRequest } from '../../../../public/interfaces';

export interface NewUserData {
  title: string;
  output: string;
}

export class MyErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
    const isSubmitted = form && form.submitted;
    return !!(control && control.invalid && (control.dirty || control.touched || isSubmitted));
  }
}

@Component({
  selector: 'app-new-user',
  standalone: false,
  templateUrl: './new-user.component.html',
  styleUrl: './new-user.component.css'
})
export class NewUserComponent {
  readonly dialogRef = inject(MatDialogRef<NewUserComponent>);
  readonly data = inject<NewUserData>(MAT_DIALOG_DATA);
  readonly output = model(this.data.output);

  validationErrors: ApiException[] = [];

  protected readonly isLoading = signal(false);

  matcher = new MyErrorStateMatcher();
  userForm = new FormGroup({
    email: new FormControl(null, [Validators.required, Validators.email]),
    username: new FormControl(null, [Validators.required]),
    dateOfBirth: new FormControl(null, [Validators.required]),
    gender: new FormControl(null, [Validators.required]),
    password: new FormControl(null, [Validators.required]),
    passwordConfirm: new FormControl(null, [Validators.required])
  },
    // add custom Validators to the form, to make sure that password and passwordConfirm are equal
    { validators: CustomValidators.passwordsMatching }
  )

  constructor(
    private authService: AuthService,
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

    var registerRequest: RegisterRequest =
    {
      dateOfBirth: this.userForm.value.dateOfBirth!,
      gender: parseInt(this.userForm.value.gender!),
      username: this.userForm.value.username!,
      email: this.userForm.value.email!,
      password: this.userForm.value.password!,
    };

    this.usersService.register(registerRequest).pipe(
      tap((res: getUsersResponseWrapper) => {
        const newUser = res.data;
        this.isLoading.set(false);
        this.dialogRef.close(newUser);
      }),
      catchError(res => {
        if (res.status === 0) {
          // Client-side or network error
          console.error('An error occurred:', res.error);
        } else {
          // Backend returned an unsuccessful response code
          const resWrapper: getUsersResponseWrapper = res.error;
          this.validationErrors = resWrapper.errors;
          this.isLoading.set(false);
        }
        return of(res);
      })
    ).subscribe();
  }
}
