import {
    Component,
    OnInit,
    ChangeDetectionStrategy,
    EventEmitter,
    Input,
    Output,
} from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { State } from '../../Services/CommonMemmber';
import { LanguagesService } from '@app/infrastructure/core/services/language/language.service';
import { LanguageModel } from '@app/infrastructure/models/project/LanguageModel';
import { NotificationService } from '@core/services/notification.service';
import { catchError, map } from 'rxjs/operators';

@Component({
    selector: 'app-user-edit-control',
    templateUrl: './user-edit-control.component.html',
    styleUrls: ['./user-edit-control.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UserEditControlComponent implements OnInit {
    UserEditControl: FormGroup;

    @Input() SearchPlaceHolder: string;
    @Output() notify: EventEmitter<State> = new EventEmitter<State>();
    @Output() searchChange: EventEmitter<string> = new EventEmitter<string>();
    @Output() languageChange: EventEmitter<number> = new EventEmitter<number>();
    @Input() IsShowAddButton = true;
    @Input() IsshowLanguage = false;
    languages: LanguageModel[];

    constructor(
        private formBuilder: FormBuilder,
        private languagesService: LanguagesService,
        private notifymessage: NotificationService,
    ) {}

    ngOnInit(): void {
        this.loadAllLanguages();
        this.UserEditControl = this.formBuilder.group({
            searchInput: [''],
        });
    }
    loadAllLanguages() {
        return this.languagesService
            .getAllLanguages()
            .pipe(
                map((data) => {
                    this.languages = data;
                    //  this.selectedLanguageId = this.userService.getLanguageId();
                }),
                catchError((): any => {
                    this.notifymessage.showTranslateMessage(
                        'ErrorLoadLanguages',
                    );
                }),
            )
            .subscribe((result) => {});
    }
    onClearSearchBox() {
        this.UserEditControl.reset();
        this.notify.emit(State.ClearSearchBox);
    }

    onAddClick() {
        this.notify.emit(State.Add);
    }

    onPrintClick(isAllData: boolean) {
        if (isAllData) {
            this.notify.emit(State.PrintAll);
        } else {
            this.notify.emit(State.Print);
        }
    }

    onExcelClick(isAllData: boolean) {
        if (isAllData) {
            this.notify.emit(State.ExcelAll);
        } else {
            this.notify.emit(State.Excel);
        }
    }

    onKeyUpEnterFilter(searchFilter) {
        setTimeout(() => this.searchChange.emit(searchFilter.target.value));
    }

    applyFilter(searchFilter: string) {
        if (searchFilter) {
            if (searchFilter.length !== 0) {
                setTimeout(() => this.searchChange.emit(searchFilter));
            }
        } else {
            setTimeout(() => this.searchChange.emit(''));
        }
    }
    applyLanguageFilter(searchFilter: number) {
        if (searchFilter) {
            if (searchFilter !== 0) {
                setTimeout(() => this.languageChange.emit(searchFilter));
            }
        } else {
            setTimeout(() => this.languageChange.emit(0));
        }
    }
}
