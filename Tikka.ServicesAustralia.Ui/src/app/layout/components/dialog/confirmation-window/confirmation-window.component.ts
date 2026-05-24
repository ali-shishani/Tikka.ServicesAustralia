import { ChangeDetectionStrategy, Component, inject, model, signal } from '@angular/core';
import {
  MAT_DIALOG_DATA,
  MatDialogRef,
} from '@angular/material/dialog';

export interface ConfirmationWindowData {
  title: string;
  description: string;
}

@Component({
  selector: 'app-confirmation-window',
  standalone: false,
  templateUrl: './confirmation-window.component.html',
  styleUrl: './confirmation-window.component.css'
})
export class ConfirmationWindowComponent {
  readonly dialogRef = inject(MatDialogRef<ConfirmationWindowComponent>);
  readonly data = inject<ConfirmationWindowData>(MAT_DIALOG_DATA);

  onNoClick(): void {
    this.dialogRef.close();
  }
}
