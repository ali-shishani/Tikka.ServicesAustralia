import { ChangeDetectionStrategy, Component, inject, model, signal } from '@angular/core';
import {
  MAT_DIALOG_DATA,
  MatDialogRef,
} from '@angular/material/dialog';

export interface LogWindowData {
  title: string;
  description: string;
  log: string;
}

@Component({
  selector: 'app-log-window',
  standalone: false,
  templateUrl: './log-window.component.html',
  styleUrl: './log-window.component.css'
})
export class LogWindowComponent {
  readonly dialogRef = inject(MatDialogRef<LogWindowComponent>);
  readonly data = inject<LogWindowData>(MAT_DIALOG_DATA);
  readonly output = model(this.data.log);

  onNoClick(): void {
    this.dialogRef.close();
  }
}
