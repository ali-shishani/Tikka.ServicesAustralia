import { Component, ViewChild } from '@angular/core';
import { MatTable } from '@angular/material/table';

export interface getUsersResponse {
  username: string;
  email: string;
  isEmailConfirmed: boolean;
}

const ELEMENT_DATA: getUsersResponse[] = [
  { isEmailConfirmed: true, username: 'Hydrogen', email: 'H' },
  { isEmailConfirmed: true, username: 'Helium', email: 'He' },
  { isEmailConfirmed: true, username: 'Lithium', email: 'Li' },
  { isEmailConfirmed: true, username: 'Beryllium', email: 'Be' },
  { isEmailConfirmed: true, username: 'Boron', email: 'B' },
  { isEmailConfirmed: true, username: 'Carbon', email: 'C' },
  { isEmailConfirmed: true, username: 'Nitrogen', email: 'N' },
  { isEmailConfirmed: true, username: 'Oxygen', email: 'O' },
  { isEmailConfirmed: true, username: 'Fluorine', email: 'F' },
  { isEmailConfirmed: true, username: 'Neon', email: 'Ne' },
];

@Component({
  selector: 'app-users',
  standalone: false,
  templateUrl: './users.component.html',
  styleUrl: './users.component.css'
})
export class UsersComponent {
  displayedColumns: string[] = ['username', 'email', 'isEmailConfirmed', 'action'];
  dataSource = [...ELEMENT_DATA];

  @ViewChild(MatTable) table!: MatTable<getUsersResponse>;

  addData() {
    const randomElementIndex = Math.floor(Math.random() * ELEMENT_DATA.length);
    this.dataSource.push(ELEMENT_DATA[randomElementIndex]);
    this.table.renderRows();
  }

  removeData() {
    this.dataSource.pop();
    this.table.renderRows();
  }
}
