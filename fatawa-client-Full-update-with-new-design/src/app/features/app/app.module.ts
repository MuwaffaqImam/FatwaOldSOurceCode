import { HttpClient, HttpClientModule } from '@angular/common/http';
import { HashLocationStrategy, LocationStrategy } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatPaginatorIntl } from '@angular/material/paginator';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatPaginatorIntlService } from '@app/infrastructure/shared/Services/mat-paginator-intl.service';
import { JwtModule } from '@auth0/angular-jwt';
import { CoreModule } from '@core/core.module';
import { environment } from '@env/environment';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { SharedModule } from '@shared/shared.module';
import {
    FacebookLoginProvider,
    GoogleLoginProvider,
    SocialAuthServiceConfig,
    SocialLoginModule,
} from 'angularx-social-login';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { NgProgressModule } from 'ngx-progressbar';
import { NgProgressHttpModule } from 'ngx-progressbar/http';
import {
    AppRoutingModule,
    components as mainComponents,
} from './app-routing.module';
import { AppComponent } from './app.component';
import { RedirectRouteComponent } from './redirectRoute/redirectRoute.component';
import { FormsModule } from '@angular/forms';
import { ClipboardModule } from 'ngx-clipboard';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
@NgModule({
    declarations: [AppComponent, mainComponents, RedirectRouteComponent],
    imports: [
        FormsModule,
        ClipboardModule,
        BrowserAnimationsModule,
        BrowserModule,
        CoreModule,
        HttpClientModule,
        CKEditorModule,
        TranslateModule.forRoot({
            loader: {
                provide: TranslateLoader,
                useFactory: (http: HttpClient) => new TranslateHttpLoader(http),
                deps: [HttpClient],
            },
        }),
        JwtModule.forRoot({
            config: {
                tokenGetter: () => sessionStorage.getItem('authToken'),
                allowedDomains: [environment.apiRoute],
            },
        }),
        NgProgressHttpModule,
        NgProgressModule.withConfig({
            color: '#FF0000',
            spinner: false,
        }),
        SharedModule.forRoot(),
        AppRoutingModule,
        PerfectScrollbarModule,
        SocialLoginModule,
    ],
    exports: [ClipboardModule],
    providers: [
        {
            provide: MatPaginatorIntl,
            useClass: MatPaginatorIntlService,
        },
        { provide: LocationStrategy, useClass: HashLocationStrategy },
        {
            provide: 'SocialAuthServiceConfig',
            useValue: {
                autoLogin: false,
                providers: [
                    {
                        id: GoogleLoginProvider.PROVIDER_ID,
                        provider: new GoogleLoginProvider(
                            environment.googleClientId,
                        ),
                    },
                    {
                        id: FacebookLoginProvider.PROVIDER_ID,
                        provider: new FacebookLoginProvider(
                            environment.facebookClientId,
                        ),
                    },
                ],
            } as SocialAuthServiceConfig,
        },
    ],
    bootstrap: [AppComponent],
})
export class AppModule {}
