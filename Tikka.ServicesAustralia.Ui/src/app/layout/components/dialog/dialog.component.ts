import { ChangeDetectionStrategy, Component, inject, model, signal } from '@angular/core';
import {
  MAT_DIALOG_DATA,
  MatDialogRef,
} from '@angular/material/dialog';

export interface DialogData {
  title: string;
  description: string;
  output: string;
}

@Component({
  selector: 'app-dialog',
  standalone: false,
  templateUrl: './dialog.component.html',
  styleUrl: './dialog.component.css'
})
export class DialogComponent {
  readonly dialogRef = inject(MatDialogRef<DialogComponent>);
  readonly data = inject<DialogData>(MAT_DIALOG_DATA);
  readonly output = model(this.data.output);

  isValid(): boolean {
    if (this.output()) {
      return true;
    }

    return false;
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}
