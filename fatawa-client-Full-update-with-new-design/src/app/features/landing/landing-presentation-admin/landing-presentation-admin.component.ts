import { Component, ChangeDetectionStrategy } from '@angular/core';

/**
 * Wrapper component for all `CaseModule` routes.
 */
@Component({
    selector: 'app-landing-presentation-admin',
    templateUrl: './landing-presentation-admin.component.html',
    styleUrls: ['./landing-presentation-admin.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LandingPresentationAdminComponent {}
