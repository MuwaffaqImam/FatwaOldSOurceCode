import {
    ChangeDetectionStrategy,
    ChangeDetectorRef,
    Component,
    OnInit,
} from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { UserService } from '@app/infrastructure/core/services/auth/user.service';
import { FatawaService } from '@app/infrastructure/core/services/fatawa/fatawa.service';
import { FatawaModel } from '@app/infrastructure/models/project/fatawa/FatawaModel';
import { LanguageModel } from '@app/infrastructure/models/project/LanguageModel';
import { mergeMap } from 'rxjs/operators';

@Component({
    selector: 'app-fatawa-description',
    templateUrl: './fatawa-description.component.html',
    styleUrls: ['./fatawa-description.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FatawaDescriptionComponent implements OnInit {
    public dataSource: any;
    public pageSize = 3;
    public pageIndex = 0;
    public totalSize = 0;
    public fatwa: FatawaModel;
    public fatwaRelations: FatawaModel[];
    public fatwalanguages: LanguageModel[] = [];
    private fatwaId;
    private departmenId: number = 0;
    private mathhabId: number = 0;
    private fatwaTypeId: number = 0;
    private languageId: number;
    // public fontSize: number = 20;
    // public lineHeight: number = 23;
    private increaseAmount = 1.1;
    private decreaseAmount = 0.9;

    constructor(
        private activatedRoute: ActivatedRoute,
        private fatawaService: FatawaService,
        private userService: UserService,
        private ref: ChangeDetectorRef,
    ) {}

    ngOnInit(): void {
        this.languageId = this.userService.getLanguageId();
        this.activatedRoute.data
            .pipe(
                mergeMap((data) => {
                    this.fatwa = data.fatwaDesc;
                    return this.getFatwaLanguages();
                }),
                mergeMap((data) => {
                    this.fatwalanguages = data;
                    return this.getFatawaRelations();
                }),
            )
            .subscribe((data: FatawaModel[]) => {
                this.fatwaRelations = data;
                this.ref.markForCheck();
            });

        this.dataSource = new MatTableDataSource<FatawaModel>([]);
        // this.dataSource.paginator = this.paginator;
        this.getFatawa(
            this.departmenId,
            this.fatwaTypeId,
            this.pageIndex,
            this.pageSize,
        );
    }
    getFatawa(
        departmenId: number,
        typeId: number,
        pageIndex: number,
        pageSize: number,
    ) {
        this.fatawaService
            .getClientFatawaFiltered(
                departmenId,
                typeId,
                pageIndex + 1,
                pageSize,
            )
            .subscribe(
                (data) => {
                    this.dataSource.data = data.dataRecord;
                    this.totalSize = data.countRecord;
                    //this.changeDetectorRef.detectChanges();
                },
                () => {
                    //   this.notify.showTranslateMessage('ErrorOnGittingFatawas');
                },
            );
    }
    getFatwaByLanguage(langId) {
        return this.fatawaService.getFatawaByLanguage(this.fatwa.id, langId);
    }

    getFatwaLanguages() {
        return this.fatawaService.getFatawaLanguages(this.fatwa.id);
    }

    getFatawaRelations() {
        return this.fatawaService.getFatawaRelations(
            this.fatwa.id,
            this.languageId,
        );
    }

    getLanguageImageResource(language: LanguageModel): string {
        return `assets/images/${language.languageCode}.png`;
    }

    onLanguageChange(langId: number) {
        this.languageId = langId;
        this.getFatwaByLanguage(langId).subscribe((res) => {
            this.fatwa = res;
            this.ref.detectChanges();
        });
    }

    // decrease() {
    //     this.fontSize = this.fontSize * this.decreaseAmount;
    //     this.lineHeight = this.fontSize + 7;
    // }

    // increase() {
    //     this.fontSize = this.fontSize * this.increaseAmount;
    //     this.lineHeight = this.fontSize + 7;
    // }

    // reset() {
    //     this.fontSize = 17;
    //     this.lineHeight = this.fontSize + 7;
    // }
}
