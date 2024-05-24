import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LandingLayoutComponent } from '../../landing/landing-layout/landing-layout.component';
import {
    RoleAuthGuard,
    SuperAdminAuthGuard,
    StudentAdminAuthGuard,
} from '@app/infrastructure/core/guards';
import {
    components as fatawaLiveComponents,
    routes as fatawaLiveRoutes,
} from './fatawa-live/fatawa-live-routing.module';
import {
    components as fatawaArchivedComponents,
    routes as fatawaArchivedRoutes,
} from './fatawa-archived/fatawa-archived-routing.module';
import {
    components as fatawaDepartmentsComponents,
    routes as fatawaDepartmentsRoutes,
} from './departments/departments-routing.module';
import {
    components as fatawaIncomingQuestionComponents,
    routes as fatawaIncomingQuestionRoutes,
} from './incoming-questions/incomingQuestions-routing.module';

// import {
//     components as fatwaUserscomponent,
//     routes as fatawaUsersRoutes,
// } from './fatawa-users/fatawa-users-routing.module';

import { UserType } from '@app/infrastructure/models/user';
import { FatawaDetailsResolveService } from '@app/infrastructure/core/services/fatawa/fatawa-edit-resolve.service';
import { QuestionToFatwaResolveService } from '@app/infrastructure/core/services/fatawa/question-to-fatwa-resolve.service';
import { FatawaEditorComponent } from './fatawa-editor/fatawa-editor.component';
import { FatawaDefaultSettingsListComponent } from './fatawa-default-settings-list/fatawa-default-settings-list.component';
import { FatawaDefaultSettingsEditorComponent } from './fatawa-default-settings-editor/fatawa-default-settings-editor.component';

import { FatawaImportComponent } from './fatawa-import/fatawa-import.component';
import { FatawaExportComponent } from './fatawa-export/fatawa-export.component';

import { FatawaTranslationComponent } from './fatawa-translation/fatawa-translation.component';

const routes: Routes = [
    {
        path: '',
        component: LandingLayoutComponent,
        children: [
            {
                path: 'departments',
                canActivate: [SuperAdminAuthGuard],
                canActivateChild: [SuperAdminAuthGuard],
                children: fatawaDepartmentsRoutes,
            },
            {
                path: 'fatawa-archived',
                data: {
                    roles: [UserType.SuperAdmin, UserType.Admin],
                },
                canActivate: [RoleAuthGuard],
                canActivateChild: [RoleAuthGuard],
                children: fatawaArchivedRoutes,
            },

            {
                path: 'fatawa-live',
                data: {
                    roles: [UserType.SuperAdmin, UserType.Admin],
                },
                canActivate: [RoleAuthGuard],
                canActivateChild: [RoleAuthGuard],
                children: fatawaLiveRoutes,
            },
            {
                path: 'fatawa-translation',
                data: {
                    roles: [
                        UserType.SuperAdmin,
                        UserType.Admin,
                        UserType.StudentAdmin,
                    ],
                },
                canActivate: [RoleAuthGuard],
                canActivateChild: [RoleAuthGuard],
                component: FatawaTranslationComponent,
            },
            {
                path: 'fatawa-import',
                data: {
                    roles: [
                        UserType.SuperAdmin,
                        UserType.Admin,
                        UserType.StudentAdmin,
                    ],
                },
                canActivate: [RoleAuthGuard],
                canActivateChild: [RoleAuthGuard],
                component: FatawaImportComponent,
            },
            {
                path: 'fatawa-export',
                data: {
                    roles: [
                        UserType.SuperAdmin,
                        UserType.Admin,
                        UserType.StudentAdmin,
                    ],
                },
                canActivate: [RoleAuthGuard],
                canActivateChild: [RoleAuthGuard],
                component: FatawaExportComponent,
            },
            {
                path: 'incomming-questions',
                data: {
                    roles: [
                        UserType.SuperAdmin,
                        UserType.Admin,
                        UserType.StudentAdmin,
                    ],
                },
                canActivate: [RoleAuthGuard],
                canActivateChild: [RoleAuthGuard],
                children: fatawaIncomingQuestionRoutes,
            },
            {
                path: 'addFatwa',
                component: FatawaEditorComponent,
                resolve: {
                    addFatwaDefaultSettings: FatawaDetailsResolveService,
                },
            },
            {
                path: 'editFatwa/:id',
                component: FatawaEditorComponent,
                resolve: {
                    editFatwa: FatawaDetailsResolveService,
                },
            },
            {
                path: 'questionToFatwa/:id',
                component: FatawaEditorComponent,
                resolve: {
                    questionToFatwa: QuestionToFatwaResolveService,
                },
            },
        ],
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class FatawaRoutingModule {}

export const components = [
    fatawaLiveComponents,
    fatawaArchivedComponents,
    fatawaDepartmentsComponents,
    fatawaIncomingQuestionComponents,
    FatawaEditorComponent,
    FatawaImportComponent,
    FatawaExportComponent,
    FatawaDefaultSettingsListComponent,
    FatawaDefaultSettingsEditorComponent,
    FatawaTranslationComponent,
];
