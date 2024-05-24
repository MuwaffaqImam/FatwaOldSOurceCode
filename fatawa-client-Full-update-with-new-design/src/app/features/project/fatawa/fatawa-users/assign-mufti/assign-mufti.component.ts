import {
    Component,
    OnInit,
    ChangeDetectionStrategy,
    Inject,
} from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FatawaDepartmentService } from '@app/infrastructure/core/services/fatawa/department.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
// import { DepartmentNodeModel } from '@app/infrastructure/models/project/department/departmentModel';
import { NumberSymbol } from '@angular/common';
import { UserService } from '@app/infrastructure/core/services/auth/user.service';
import { LanguagesService } from '@app/infrastructure/core/services/language/language.service';
import { catchError, map, take } from 'rxjs/operators';
import { LanguageModel } from '@app/infrastructure/models/project/LanguageModel';
import { NotificationService } from '@app/infrastructure/core/services/notification.service';
import { UserNodeModel } from '@app/infrastructure/models/project/fatawa/MuftiTreeModal';
@Component({
    selector: 'app-assign-mufti',
    templateUrl: './assign-mufti.component.html',
    styleUrls: ['./assign-mufti.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AssignMuftiComponent implements OnInit {
    frmAddNew: FormGroup;
    public isInProgress = false;

    muftiList = [];
    constructor(
        @Inject(MAT_DIALOG_DATA)
        public userTreeModel: UserNodeModel,
        private formBuilder: FormBuilder,
        private dialogRef: MatDialogRef<AssignMuftiComponent>,
        private userService: UserService,
        private notify: NotificationService,
    ) {}

    get ID() {
        return this.frmAddNew.controls.Id.value;
    }

    ngOnInit(): void {
        // this.loadAllLanguages();
        this.getFatawaMuftiList();
        this.ngInitialControlForm();
        this.setDepartmentDetails();
    }

    ngInitialControlForm() {
        this.frmAddNew = this.formBuilder.group({
            Id: [0],
            ParentId: [Validators.required],
            mufitUserId: ['', Validators.required],
            NodeName: [''],
            DepartmentNumber: ['', Validators.required],
            DepartmentMain: ['', Validators.required],
            NodeLevelNumber: [Validators.required],
        });
    }

    setDepartmentDetails() {
        this.frmAddNew.controls.Id.setValue(Number(this.userTreeModel.id));
        this.frmAddNew.controls.ParentId.setValue(
            Number(this.userTreeModel.parentId),
        );
        this.frmAddNew.controls.mufitUserId.setValue(
            Number(this.userTreeModel.mufitUserId),
        );

        this.frmAddNew.controls.NodeName.setValue('');

        this.frmAddNew.controls.DepartmentNumber.setValue(
            this.userTreeModel.nodeNumber,
        );
        this.frmAddNew.controls.DepartmentMain.setValue(
            this.userTreeModel.nodeMain,
        );
        this.frmAddNew.controls.NodeLevelNumber.setValue(
            Number(this.userTreeModel.nodeLevelNumber),
        );
    }

    getFatawaMuftiList() {
        if (this.userTreeModel.nodeLevelNumber === 1) {
            return this.userService
                .getAllSuperAdminsAsync()
                .pipe(
                    map((data) => {
                        this.muftiList = data;
                    }),
                    catchError((): any => {
                        this.notify.showTranslateMessage('ErrorLoadLanguages');
                    }),
                )
                .subscribe((result) => {});
        } else if (this.userTreeModel.nodeLevelNumber === 2) {
            return this.userService
                .getAllAdminsAsync()
                .pipe(
                    map((data) => {
                        this.muftiList = data;
                    }),
                    catchError((): any => {
                        this.notify.showTranslateMessage('ErrorLoadLanguages');
                    }),
                )
                .subscribe((result) => {});
        } else {
            return this.userService
                .getAllResearchHelperAsync()
                .pipe(
                    map((data) => {
                        this.muftiList = data;
                    }),
                    catchError((): any => {
                        this.notify.showTranslateMessage('ErrorLoadLanguages');
                    }),
                )
                .subscribe((result) => {});
        }
    }
    // loadAllLanguages() {
    //     return this.languagesService
    //         .getAllLanguages()
    //         .pipe(
    //             map((data) => {
    //                 this.languages = data;
    //                 this.selectedLanguageId = this.userService.getLanguageId();
    //             }),
    //             catchError((): any => {
    //                 this.notify.showTranslateMessage('ErrorLoadLanguages');
    //             }),
    //         )
    //         .subscribe((result) => {});
    // }

    // public languageChanged(language: LanguageModel): void {
    //     this.fatawaDepartmentService
    //         .getDepartmentByLanguageId(this.ID, language.id)
    //         .pipe(take(1))
    //         .subscribe((departmentModel: DepartmentNodeModel) => {
    //             this.departmentTreeModel = departmentModel;
    //             this.setDepartmentDetails();
    //         });
    // }

    OnSubmit() {
        this.isInProgress = true;
        if (this.ID === 0) {
            this.userService.adduserMufti(this.frmAddNew.value).subscribe(
                (Id: any) => {
                    if (Id) {
                        this.frmAddNew.controls.Id.setValue(Id);
                        this.dialogRef.close(this.frmAddNew.value);
                        this.frmAddNew.reset();
                    }
                },
                () => {
                    this.dialogRef.close(this.frmAddNew.value);
                },
            );
        }
        this.resetFormBuilder();
    }

    resetFormBuilder() {
        this.frmAddNew.reset();
        this.isInProgress = false;
    }
}
