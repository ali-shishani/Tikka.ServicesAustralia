import { ChangeDetectionStrategy, Component, inject, model, signal } from '@angular/core';
import { FormGroup, FormControl, Validators, FormGroupDirective, NgForm } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { tap, catchError, throwError, of } from 'rxjs';

import {
  MAT_DIALOG_DATA,
  MatDialogRef,
} from '@angular/material/dialog';
import { ErrorStateMatcher } from '@angular/material/core';

import { getUsersResponse, getUsersResponseWrapper, ApiException, UpdateUserRequest } from '../../../interfaces'
import { UsersService } from '../../../services/users.service'

export interface EditUserData {
  title: string;
  record: getUsersResponse;
}

export class MyErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
    const isSubmitted = form && form.submitted;
    return !!(control && control.invalid && (control.dirty || control.touched || isSubmitted));
  }
}

@Component({
  selector: 'app-edit-user',
  standalone: false,
  templateUrl: './edit-user.component.html',
  styleUrl: './edit-user.component.css'
})
export class EditUserComponent {
  readonly dialogRef = inject(MatDialogRef<EditUserComponent>);
  readonly data = inject<EditUserData>(MAT_DIALOG_DATA);
  readonly record: getUsersResponse = this.data.record;

  validationErrors: ApiException[] = [];

  protected readonly isLoading = signal(false);

  matcher = new MyErrorStateMatcher();
  userForm = new FormGroup({
    email: new FormControl(''),
    userName: new FormControl(''),
    dateOfBirth: new FormControl(new Date(), [Validators.required]),
    gender: new FormControl('', [Validators.required]),
    isEmailConfirmed: new FormControl(false),
  });

  constructor(
    private usersService: UsersService
  ) {
    this.userForm.get('email')?.setValue(this.record.email);
    this.userForm.get('userName')?.setValue(this.record.userName);
    this.userForm.get('dateOfBirth')?.setValue(this.record.dateOfBirth);
    this.userForm.get('gender')?.setValue(this.record.gender == 'Male' ? '0' : '1');
    this.userForm.get('isEmailConfirmed')?.setValue(this.record.isEmailConfirmed);
  }

  submit(): void {
    if (!this.userForm.valid) {
      return;
    }

    this.isLoading.set(true);
    this.validationErrors = [];

    var updateUserRequest: UpdateUserRequest =
    {
      userId: this.record.id,
      username: this.userForm.value.userName!,
      dateOfBirth: this.userForm.value.dateOfBirth!,
      gender: parseInt(this.userForm.value.gender!),
      isEmailConfirmed: this.userForm.value.isEmailConfirmed!,
    };

    this.usersService.update(updateUserRequest).pipe(
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

  onNoClick(): void {
    this.dialogRef.close();
  }
}
