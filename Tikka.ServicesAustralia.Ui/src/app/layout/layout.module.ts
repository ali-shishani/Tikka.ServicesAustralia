import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { provideNativeDateAdapter, MAT_DATE_LOCALE } from '@angular/material/core';

import { LayoutRoutingModule } from './layout-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatTableModule } from '@angular/material/table';
import { MatChipsModule } from '@angular/material/chips';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import {
  MatDialog,
  MatDialogActions,
  MatDialogClose,
  MatDialogContent,
  MatDialogModule,
  MatDialogRef,
  MatDialogTitle,
} from '@angular/material/dialog';

import { HomeComponent } from './components/home/home.component';
import { DialogComponent } from './components/dialog/dialog.component';
import { LogWindowComponent } from './components/dialog/log-window/log-window.component';
import { ConfirmationWindowComponent } from './components/dialog/confirmation-window/confirmation-window.component';
import { UsersComponent } from './components/users/users.component';
import { SettingsComponent } from './components/settings/settings.component';
import { NewUserComponent } from './components/users/new-user/new-user.component';
import { EditUserComponent } from './components/users/edit-user/edit-user.component';

@NgModule({
  declarations: [
    HomeComponent,
    DialogComponent,
    LogWindowComponent,
    ConfirmationWindowComponent,
    UsersComponent,
    SettingsComponent,
    NewUserComponent,
    EditUserComponent
  ],
  providers: [
    { provide: MAT_DATE_LOCALE, useValue: 'en-AU' },
    provideNativeDateAdapter()
  ],
  imports: [
    CommonModule,
    // Import our Routes for this module
    LayoutRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    // Angular Material Imports
    MatButtonModule,
    MatToolbarModule,
    MatIconModule,
    MatCardModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatDialogModule,
    MatDialogActions,
    MatDialogClose,
    MatDialogContent,
    MatDialogTitle,
    MatProgressSpinnerModule,
    MatProgressBarModule,
    MatTableModule,
    MatChipsModule,
    MatSlideToggleModule,
  ]
})
export class LayoutModule { }
