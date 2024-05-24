import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TagsListComponent } from './tags-list/tags-list.component';
import { TagEditorComponent } from './tag-editor/tag-editor.component';

export const routes: Routes = [
    {
        path: '',
        redirectTo: 'tags-list',
        pathMatch: 'full',
    },
    {
        path: 'tags-list',
        component: TagsListComponent,
    },
    {
        path: 'tag-editor/:id',
        component: TagEditorComponent,
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class TagRoutingModule {}

export const components = [TagsListComponent, TagEditorComponent];
