import {
    AfterViewInit,
    ChangeDetectionStrategy,
    Component,
    Inject,
    ViewChild,
} from '@angular/core';
import { MAT_SNACK_BAR_DATA, MatSnackBar } from '@angular/material/snack-bar';

@Component({
    selector: 'app-message-snackbar',
    templateUrl: './message-snackbar.component.html',
    styleUrls: ['./message-snackbar.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MessageSnackbarComponent implements AfterViewInit {
    @ViewChild('dismissButton', { static: true }) dismissButton: HTMLElement;

    constructor(
        @Inject(MAT_SNACK_BAR_DATA) public messageList: string[],
        private snackBar: MatSnackBar,
    ) {}

    public ngAfterViewInit(): void {
        this.dismissButton.focus();
    }

    public dismiss(): void {
        this.snackBar.dismiss();
    }
}
