import { Component, inject, model, signal, ViewChild } from '@angular/core';
import { map, Observable, of, switchMap, tap, catchError } from 'rxjs';

import { MatTable } from '@angular/material/table';
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

import { getUsersResponse, getUsersResponseWrapper } from '../../interfaces'
import { UsersService } from '../../services/users.service'
import { ConfirmationWindowComponent } from '../dialog/confirmation-window/confirmation-window.component'
import { NewUserComponent } from './new-user/new-user.component'
import { EditUserComponent } from './edit-user/edit-user.component'

@Component({
  selector: 'app-users',
  standalone: false,
  templateUrl: './users.component.html',
  styleUrl: './users.component.css'
})
export class UsersComponent {
  columnsToDisplay: string[] = ['email', 'userName', 'action'];
  columnsToDisplayWithExpand = [...this.columnsToDisplay, 'expand'];
  dataSource!: getUsersResponse[];
  expandedElement!: getUsersResponse | null;

  protected readonly isLoading = signal(false);
  protected readonly isInitialised = signal(false);

  @ViewChild(MatTable) table!: MatTable<getUsersResponse>;

  readonly userEditDialog = inject(MatDialog);

  constructor(
    private usersService: UsersService,
    private snackbar: MatSnackBar
  ) {
    this.loadUsers().pipe(
      tap((res: getUsersResponseWrapper) => {
        this.dataSource = res.data;
        this.isInitialised.set(true);
      })
    ).subscribe();
  }

  loadUsers(): Observable<getUsersResponseWrapper> {
    return this.usersService.getAll();
  }

  dateOfBirthText(dateValue: Date): string {
    const dateText = dateValue.toLocaleString().split('T')[0];
    if (dateText) {
      return dateText.split('-')[2] + '/' + dateText.split('-')[1] + '/' + dateText.split('-')[0];
    }
    return dateText;
    
    // return dateValue.toLocaleString();
  }

  emailConfirmedText(emailConfirmedValue: boolean): string {
    return emailConfirmedValue ? 'Yes' : 'No';
  }

  /** Checks whether an element is expanded. */
  isExpanded(element: getUsersResponse) {
    return this.expandedElement === element;
  }

  /** Toggles the expanded state of an element. */
  toggle(element: getUsersResponse) {
    this.expandedElement = this.isExpanded(element) ? null : element;
  }

  newUser() {
    const dialogRef = this.userEditDialog.open(NewUserComponent, {
      width: '700px',
      data: { title: 'New user', output: null },
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {
        this.dataSource.push(result)
        this.table.renderRows();
        this.isLoading.set(false);
        this.snackbar.open('Create Successfull', 'Close', { duration: 2000, horizontalPosition: 'right', verticalPosition: 'top' });
      }
    });
  }

  refresh() {
    this.isInitialised.set(false);
    this.loadUsers().pipe(
      tap((res: getUsersResponseWrapper) => {
        this.dataSource = res.data;
        this.isInitialised.set(true);
      })
    ).subscribe();
  }

  editUser(user: getUsersResponse) {
    const dialogRef = this.userEditDialog.open(EditUserComponent, {
      width: '700px',
      data: { title: 'Edit user', record: user },
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {
        this.dataSource = this.dataSource.map(record =>
          record.id === user.id ? result : record
        );
        this.expandedElement = result;
        this.table.renderRows();
        this.isLoading.set(false);
        this.snackbar.open('Update Successfull', 'Close', { duration: 2000, horizontalPosition: 'right', verticalPosition: 'top' });
      }
    });
  }

  deleteUser(user: getUsersResponse) {
    const dialogRef = this.userEditDialog.open(ConfirmationWindowComponent, {
      width: '700px',
      data: { title: 'Delete user', description: 'You are about to delete user "' + user.userName + '". Would you like to continue?' },
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {
        this.isLoading.set(true);
        this.usersService.delete(user.id).pipe(
          tap(res => {
            this.dataSource = this.dataSource.filter(record =>
              record.id !== user.id
            );
            this.table.renderRows();
            this.isLoading.set(false);
            this.snackbar.open('Update Successfull', 'Close', { duration: 2000, horizontalPosition: 'right', verticalPosition: 'top' });
          }),
        ).subscribe();
      }
    });
  }

  sendConfirmationEmail(user: getUsersResponse) {
    const dialogRef = this.userEditDialog.open(ConfirmationWindowComponent, {
      width: '700px',
      data: { title: 'Send confirmation email', description: 'You are about to send a confirmation email to user "' + user.userName + '". Would you like to continue?' },
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {
        this.isLoading.set(true);
        this.usersService.requestConfirmationCode(user.email).pipe(
          tap(res => {
            this.table.renderRows();
            this.isLoading.set(false);
            this.snackbar.open('Confirmation code request Successfull', 'Close', { duration: 2000, horizontalPosition: 'right', verticalPosition: 'top' });
          }),
          catchError(res => {
            if (res.status === 0) {
              // Client-side or network error
              console.error('An error occurred:', res.error);
            } else {
              // Backend returned an unsuccessful response code
              this.snackbar.open(res.error, 'Close', { duration: 2000, horizontalPosition: 'right', verticalPosition: 'top' });
              this.isLoading.set(false);
            }
            return of(res);
          })
        ).subscribe();
      }
    });
  }
}
