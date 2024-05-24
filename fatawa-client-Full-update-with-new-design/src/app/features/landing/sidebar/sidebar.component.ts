import {
    ChangeDetectorRef,
    Component,
    NgZone,
    OnDestroy,
    ViewChild,
    HostListener,
    Directive,
    AfterViewInit,
} from '@angular/core';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { MediaMatcher } from '@angular/cdk/layout';
import { MenuItems } from '@models/menu-items';
import { AuthService } from '@app/infrastructure/core/services/auth/auth.service';
import { Router } from '@angular/router';

@Component({
    selector: 'app-sidebar',
    templateUrl: './sidebar.component.html',
    styleUrls: [],
})
export class AppSidebarComponent implements OnDestroy {
    public config: PerfectScrollbarConfigInterface = {};
    mobileQuery: MediaQueryList;

    private mobileQueryListener: () => void;
    status: boolean = true;
    itemSelect: number[] = [];

    subclickEvent() {
        this.status = true;
    }

    constructor(
        private authService: AuthService,
        private router: Router,
        changeDetectorRef: ChangeDetectorRef,
        media: MediaMatcher,
        public menuItems: MenuItems,
    ) {
        this.mobileQuery = media.matchMedia('(min-width: 768px)');
        this.mobileQueryListener = () => changeDetectorRef.detectChanges();

        this.mobileQuery.addListener(() => {
            this.mobileQueryListener();
        });
    }

    ngOnDestroy(): void {
        this.mobileQuery.removeListener(() => {
            this.mobileQueryListener();
        });
    }

    public logout(): void {
        this.authService.loggedOut();
        this.router.navigateByUrl('/auth');
    }
}
