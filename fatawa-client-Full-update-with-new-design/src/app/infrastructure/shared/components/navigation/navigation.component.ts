import { Component, ChangeDetectionStrategy } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { take } from 'rxjs/operators';
import { AuthService } from '@core/services/auth/auth.service';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';
import { ConfirmDialogConfig } from '@models/dialog';

@Component({
    selector: 'app-navigation',
    templateUrl: './navigation.component.html',
    styleUrls: ['./navigation.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NavigationComponent {
    constructor(
        private authService: AuthService,
        private dialog: MatDialog,
        private router: Router,
    ) {}

    public openDeleteDialog(): void {
        const userDialog = new ConfirmDialogConfig({
            id: 0,
            title: 'This will end your login session.',
            action: 'Log Out',
        });
        const dialogRef = this.dialog.open(ConfirmDialogComponent, {
            data: userDialog,
            autoFocus: false,
        });
        dialogRef
            .beforeClosed()
            .pipe(take(1))
            .subscribe((response) => {
                if (response) {
                    this.logout();
                }
            });
    }

    private logout(): void {
        this.authService.loggedOut();
        this.router.navigateByUrl('/auth');
    }
}
