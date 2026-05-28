import { Component, inject, model, signal, ViewChild } from '@angular/core';
import { map, Observable, of, switchMap, tap } from 'rxjs';

import { MatTable } from '@angular/material/table';

import { getUsersResponse, getUsersResponseWrapper } from '../../interfaces'
import { UsersService } from '../../services/users.service'

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
    return dateValue.split('T')[0];
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

  addData() {
    this.table.renderRows();
  }
}
