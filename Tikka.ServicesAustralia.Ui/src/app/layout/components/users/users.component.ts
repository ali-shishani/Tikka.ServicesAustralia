import { Component, inject, model, signal, ViewChild } from '@angular/core';
import { map, Observable, of, switchMap, tap } from 'rxjs';

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

import { getUsersResponse, getUsersResponseWrapper } from '../../interfaces'
import { UsersService } from '../../services/users.service'
import { NewUserComponent } from './new-user/new-user.component'
import { EditUserComponent } from './edit-user/edit-user.component'

@Component({
  selector: 'app-users',
  standalone: false,
  templateUrl: './users.component.html',
  styleUrl: './users.component.css'
})
export class UsersComponent {
  columnsToDisplay: string[] = ['userName', 'email', 'action'];
  columnsToDisplayWithExpand = [...this.columnsToDisplay, 'expand'];
  dataSource!: getUsersResponse[];
  expandedElement!: getUsersResponse | null;

  protected readonly isLoading = signal(false);
  protected readonly isInitialised = signal(false);

  @ViewChild(MatTable) table!: MatTable<getUsersResponse>;

  readonly userEditialog = inject(MatDialog);

  constructor(
    private usersService: UsersService
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

  dateOfBirthText(dateValue: string): string {
    const dateText = dateValue.split('T')[0];
    if (dateText) {
      return dateText.split('-')[2] + '/' + dateText.split('-')[1] + '/' + dateText.split('-')[0];
    }
    return dateText;
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
    const dialogRef = this.userEditialog.open(NewUserComponent, {
      width: '700px',
      data: { title: 'New user', output: null },
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {
        this.dataSource.push(result)
        this.table.renderRows();
        this.isLoading.set(false);
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
    const dialogRef = this.userEditialog.open(EditUserComponent, {
      width: '700px',
      data: { title: 'Edit user', record: user },
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {
        this.dataSource = this.dataSource.map(record =>
          record.id === user.id ? result : record
        );
        this.table.renderRows();
        this.isLoading.set(false);
      }
    });
  }
}
