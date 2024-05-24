import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddLanguageComponent } from './addLanguage/add-language.component';
import { LanguagesListComponent } from './languagesList/languages-list.component';

const routes: Routes = [
    {
        path: '',
        component: LanguagesListComponent,
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class LanguageRoutingModule { }

export const components = [
    LanguagesListComponent,
    AddLanguageComponent,
];
