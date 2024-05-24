import { Injectable } from '@angular/core';
import { MatPaginatorIntl } from '@angular/material/paginator';
import { TranslateService } from '@ngx-translate/core';

@Injectable({
    providedIn: 'root',
})
export class MatPaginatorIntlService extends MatPaginatorIntl {
    constructor(private translate: TranslateService) {
        super();

        //execute when initial grid
        this.getAndInitTranslations();

        //execute when language change
        this.translate.onLangChange.subscribe((e: Event) => {
            this.getAndInitTranslations();
        });
    }

    public getAndInitTranslations(): void {
        this.translate
            .get([
                'ItemsPerPage',
                'FirstPage',
                'PreviousPage',
                'NextPage',
                'LastPage',
            ])
            .toPromise()
            .then((translationData: string) => {
                this.itemsPerPageLabel = translationData['ItemsPerPage'];
                this.firstPageLabel = translationData['FirstPage'];
                this.previousPageLabel = translationData['PreviousPage'];
                this.nextPageLabel = translationData['NextPage'];
                this.lastPageLabel = translationData['LastPage'];
            });
        //Added this to reflect text when change language
        this.changes.next();
    }

    public getRangeLabel = (page, pageSize, length): string => {
        const from = this.translate.instant('From');
        const to = this.translate.instant('To');
        const CountAllRows = this.translate.instant('CountAllRows');

        if (length === 0 || pageSize === 0) {
            return '0 ' + CountAllRows + ' ' + length;
        }
        length = Math.max(length, 0);
        const startIndex = page * pageSize;

        const endIndex =
            startIndex < length
                ? Math.min(startIndex + pageSize, length)
                : startIndex + pageSize;
        return (
            from +
            ' ' +
            (startIndex + 1) +
            ' ' +
            to +
            ' ' +
            endIndex +
            ' ' +
            CountAllRows +
            ' ' +
            length
        );
    };
}
