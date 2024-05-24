import { DOCUMENT } from '@angular/common';
import {
    Component,
    OnInit,
    ChangeDetectionStrategy,
    NgZone,
    ChangeDetectorRef,
} from '@angular/core';

import { CdkScrollable, ScrollDispatcher } from '@angular/cdk/scrolling';

@Component({
    selector: 'app-scroll-top',
    templateUrl: './scroll-top.component.html',
    styleUrls: ['./scroll-top.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ScrollTopComponent implements OnInit {
    showScroll = false;

    constructor(
        private scrollDispatcher: ScrollDispatcher,
        private zone: NgZone,
        private cdr: ChangeDetectorRef,
    ) {}

    ngOnInit() {
        this.scrollDispatcher.scrolled().subscribe((cdk: CdkScrollable) => {
            const scrollPosition =
                cdk.getElementRef().nativeElement.scrollTop > 0;
            this.zone.run(() => {
                this.showScroll = scrollPosition;
                this.cdr.detectChanges();
            });
        });
    }

    gotoTop() {
        document.getElementById('page-content').scrollIntoView({
            behavior: 'smooth',
        });
    }
}
