import {
    ChangeDetectionStrategy,
    Component,
    OnDestroy,
    OnInit,
} from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Data, NavigationEnd, Router } from '@angular/router';
import { UserService } from '@app/infrastructure/core/services/auth/user.service';
import { LanguagesService } from '@app/infrastructure/core/services/language/language.service';
import { LanguageModel } from '@app/infrastructure/models/project/LanguageModel';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { filter, map, mergeMap, tap } from 'rxjs/operators';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppComponent implements OnInit, OnDestroy {
    public title: string = 'ALHANAFY';

    private routerEventsSubscription: Subscription;

    constructor(
        private activatedRoute: ActivatedRoute,
        private router: Router,
        private titleService: Title,
        private userService: UserService,
        public translate: TranslateService,
        public languagesService: LanguagesService,
    ) {
        this.languagesService
            .getAllLanguages()
            .pipe(
                tap((languageModels: LanguageModel[]) => {
                    const availableLanguages: string[] = languageModels.map(
                        (l) => l.languageCode,
                    );
                    translate.addLangs(availableLanguages);
                    translate.setDefaultLang(availableLanguages[0]);
                    // this for Guest user
                    if (languageModels[0]) {
                        this.userService.setLanguageId(
                            languageModels[0].id.toString(),
                        );
                        this.userService.setLanguageDir(
                            languageModels[0].languageDirection,
                        );
                    }
                }),
            )
            .subscribe();
    }

    public ngOnInit(): void {
        this.setPageTitle();
    }

    public ngOnDestroy(): void {
        this.routerEventsSubscription.unsubscribe();
    }

    private setPageTitle(): void {
        this.routerEventsSubscription = this.router.events
            .pipe(
                filter((event) => event instanceof NavigationEnd),
                map(() => this.activatedRoute),
                map((route) => {
                    while (route.firstChild) {
                        route = route.firstChild;
                    }
                    return route;
                }),
                filter((route) => route.outlet === 'primary'),
                mergeMap((route) => route.data),
            )
            .subscribe((event: Data) => {
                const title = event.title
                    ? // eslint-disable-next-line @typescript-eslint/restrict-template-expressions
                      `${this.title} - ${event.title}`
                    : this.title;
                this.titleService.setTitle(`${title}`);
            });
    }
}
