import { Injectable } from '@angular/core';

export interface BadgeItem {
    type: string;
    value: string;
}
export interface Saperator {
    name: string;
    type?: string;
}
export interface SubChildren {
    state: string;
    name: string;
    type?: string;
}
export interface ChildrenItems {
    state: string;
    name: string;
    type?: string;
    child?: SubChildren[];
}

export interface Menu {
    state: string;
    name: string;
    type: string;
    icon: string;
    badge?: BadgeItem[];
    saperator?: Saperator[];
    children?: ChildrenItems[];
}

const MENUITEMS = [
    {
        state: '',
        name: '',
        type: 'saperator',
        icon: 'av_timer',
    },
    {
        state: 'landing',
        name: 'Landing',
        type: 'sub',
        icon: 'apps',
        children: [
            {
                state: 'landing-presentation',
                name: 'LandingPresentation',
                type: 'link',
            },
            // {
            //     state: 'app-fatawa-cards-list',
            //     name: 'Fatawa Cards',
            //     type: 'link',
            // },
            // { state: 'jobs-list', name: 'Job List', type: 'link' },
            // { state: 'chat', name: 'Chat', type: 'link' },
        ],
    },
    {
        state: 'users',
        name: 'Users',
        type: 'sub',
        icon: 'bubble_chart',
        children: [
            { state: 'users-list', name: 'UsersList', type: 'link' },
            { state: 'fatwa-users', name: 'TheFatwaUsers', type: 'link' },
        ],
    },
    {
        state: 'fatawa',
        name: 'Fatawa',
        type: 'sub',
        icon: 'bubble_chart',
        children: [
            { state: 'departments', name: 'Departments', type: 'link' },
            { state: 'fatawa-archived', name: 'FatawaArchived', type: 'link' },
            { state: 'fatawa-live', name: 'FatawaLive', type: 'link' },
            {
                state: 'incomming-questions',
                name: 'IncomingQuestions',
                type: 'link',
            },
            {
                state: 'fatawa-translation',
                name: 'TranslationFatawa',
                type: 'link',
            },
            {
                state: 'fatawa-import',
                name: 'ImportFatawa',
                type: 'link',
            },
            {
                state: 'fatawa-export',
                name: 'ExportFatawa',
                type: 'link',
            },
        ],
    },
    {
        state: 'system-definition',
        name: 'SystemSettings',
        type: 'sub',
        icon: 'bubble_chart',
        children: [
            {
                state: 'general-settings',
                name: 'GeneralSettings',
                type: 'link',
            },
            {
                state: 'languages-settings',
                name: 'languagesSettings',
                type: 'link',
            },
        ],
    },
];

@Injectable({
    providedIn: 'root',
})
export class MenuItems {
    getMenuitem(): Menu[] {
        return MENUITEMS;
    }
}
