import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

import { IConfirmDialogConfig } from '@models/dialog';

@Component({
    templateUrl: './confirm-dialog.component.html',
    styleUrls: ['./confirm-dialog.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ConfirmDialogComponent {
    message: string;

    constructor(
        @Inject(MAT_DIALOG_DATA) public dialog: IConfirmDialogConfig,
        private dialogRef: MatDialogRef<ConfirmDialogComponent>,
    ) { }

    public submit(): void {
        this.dialogRef.close(this.dialog);
    }
}
