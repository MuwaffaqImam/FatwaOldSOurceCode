// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
    production: false,
    name: 'Development',
    apiRoute: 'https://localhost:44370/api',
    hubRoute: 'https://localhost:44370/hubs',
    sentry: {
        DSN:
            'https://3e218e103eea4edc8577ca7aca6f031e@o313160.ingest.sentry.io/5352536',
        enabled: false,
        dialogEnabled: false,
    },
    googleClientId:
        '844201171157-hhs5sqrs4vdr729njmbnfdl3dd5dvc99.apps.googleusercontent.com',
    googleClientSecret: 'WzzRxuZFFk2MFfWEI0EzMVKT',
    facebookClientId: '315159433446779',
    facebookClientSecret: 'f89697ed41008a7248c287841edbacfc',
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
