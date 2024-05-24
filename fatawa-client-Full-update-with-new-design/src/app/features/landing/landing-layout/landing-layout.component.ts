import { MediaMatcher } from '@angular/cdk/layout';
import {
    AfterViewInit,
    ChangeDetectionStrategy,
    ChangeDetectorRef,
    Component,
    OnDestroy,
} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { LanguageModel } from '@app/infrastructure/models/project/LanguageModel';
import { Constants } from '@app/infrastructure/utils/constants';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';

@Component({
    templateUrl: './landing-layout.component.html',
    styleUrls: ['./landing-layout.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LandingLayoutComponent implements OnDestroy, AfterViewInit {
    dir: string = Constants.DefaultLanguageDirection;
    green: boolean;
    blue: boolean;
    dark: boolean;
    minisidebar: boolean;
    boxed: boolean;
    danger: boolean;
    showHide: boolean;
    sidebarOpened;
    isShowSearchBar: boolean = false;
    mobileQuery: MediaQueryList;

    private mobileQueryListener: () => void;
    public config: PerfectScrollbarConfigInterface = {};

    constructor(
        changeDetectorRef: ChangeDetectorRef,
        media: MediaMatcher,
        public dialog: MatDialog,
    ) {
        this.mobileQuery = media.matchMedia('(min-width: 768px)');
        this.mobileQueryListener = () => changeDetectorRef.detectChanges();

        this.mobileQuery.addListener(() => {
            this.mobileQueryListener();
        });
    }

    openDialog(): void {
        // const dialogRef = this.dialog.open(ContactDialogComponent, {
        //   width: '250px',
        // });
    }

    ngAfterViewInit() {}

    setSearchVisibility(showSearchBar: boolean) {
        this.isShowSearchBar = showSearchBar;
    }

    setBoxStyle(event) {
        this.boxed = event.checked;
    }

    setColorStyle(event, styleIndex: number) {
        this.green = this.danger = this.blue = this.dark = false;
        switch (styleIndex) {
            case 1:
                this.danger = event.checked;
                break;
            case 2:
                this.green = event.checked;
                break;
            case 3:
                this.blue = event.checked;
                break;
            case 4:
                this.dark = event.checked;
                break;
            default:
                this.blue = true;
                break;
        }
    }

    ngOnDestroy(): void {
        this.mobileQuery.removeListener(() => {
            this.mobileQueryListener();
        });
    }

    userLanguageChanged(userLanguage: LanguageModel): void {
        this.dir = userLanguage.languageDirection;
    }
}
