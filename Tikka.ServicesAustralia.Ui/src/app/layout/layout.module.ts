import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LayoutRoutingModule } from './layout-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatTableModule } from '@angular/material/table';
import { MatChipsModule } from '@angular/material/chips';
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


@NgModule({
  declarations: [
    HomeComponent,
    DialogComponent,
    LogWindowComponent,
    ConfirmationWindowComponent,
    UsersComponent,
    SettingsComponent
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
    MatDialogModule,
    MatDialogActions,
    MatDialogClose,
    MatDialogContent,
    MatDialogTitle,
    MatProgressSpinnerModule,
    MatProgressBarModule,
    MatTableModule,
    MatChipsModule,
  ]
})
export class LayoutModule { }
