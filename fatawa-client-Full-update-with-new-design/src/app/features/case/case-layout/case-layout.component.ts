import { Component, ChangeDetectionStrategy } from '@angular/core';

/**
 * Wrapper component for all `CaseModule` routes.
 */
@Component({
    templateUrl: './case-layout.component.html',
    styleUrls: ['./case-layout.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CaseLayoutComponent {}
