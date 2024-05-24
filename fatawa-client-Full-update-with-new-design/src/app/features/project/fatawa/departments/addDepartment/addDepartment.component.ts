import {
    Component,
    OnInit,
    ChangeDetectionStrategy,
    Inject,
} from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FatawaDepartmentService } from '@app/infrastructure/core/services/fatawa/department.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DepartmentNodeModel } from '@app/infrastructure/models/project/department/departmentModel';
import { NumberSymbol } from '@angular/common';
import { UserService } from '@app/infrastructure/core/services/auth/user.service';
import { LanguagesService } from '@app/infrastructure/core/services/language/language.service';
import { catchError, map, take } from 'rxjs/operators';
import { LanguageModel } from '@app/infrastructure/models/project/LanguageModel';
import { NotificationService } from '@app/infrastructure/core/services/notification.service';

@Component({
    selector: 'app-add-department',
    templateUrl: './addDepartment.component.html',
    styleUrls: ['./addDepartment.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class addDepartmentComponent implements OnInit {
    frmAddNew: FormGroup;
    public isInProgress = false;
    languages: LanguageModel[];
    selectedLanguageId: number;

    constructor(
        @Inject(MAT_DIALOG_DATA)
        public departmentTreeModel: DepartmentNodeModel,
        private formBuilder: FormBuilder,
        private fatawaDepartmentService: FatawaDepartmentService,
        private dialogRef: MatDialogRef<addDepartmentComponent>,
        private userService: UserService,
        private languagesService: LanguagesService,
        private notify: NotificationService,
    ) {}

    get ID() {
        return this.frmAddNew.controls.Id.value;
    }

    ngOnInit(): void {
        this.loadAllLanguages();
        this.ngInitialControlForm();
        this.setDepartmentDetails();
    }

    ngInitialControlForm() {
        this.frmAddNew = this.formBuilder.group({
            Id: [0],
            ParentId: [Validators.required],
            NodeName: ['', Validators.required],
            DepartmentNumber: ['', Validators.required],
            DepartmentMain: ['', Validators.required],
            NodeLevelNumber: [Validators.required],
            FatawaDepartmentTranslateId: [Validators.required],
            languageId: [''],
        });
    }

    setDepartmentDetails() {
        this.frmAddNew.controls.Id.setValue(
            Number(this.departmentTreeModel.id),
        );
        this.frmAddNew.controls.ParentId.setValue(
            Number(this.departmentTreeModel.parentId),
        );
        this.frmAddNew.controls.NodeName.setValue(
            this.departmentTreeModel.nodeName,
        );
        this.frmAddNew.controls.DepartmentNumber.setValue(
            this.departmentTreeModel.nodeNumber,
        );
        this.frmAddNew.controls.DepartmentMain.setValue(
            this.departmentTreeModel.nodeMain,
        );
        this.frmAddNew.controls.NodeLevelNumber.setValue(
            Number(this.departmentTreeModel.nodeLevelNumber),
        );
        this.frmAddNew.controls.FatawaDepartmentTranslateId.setValue(
            Number(this.departmentTreeModel.fatawaDepartmentTranslateId),
        );
        this.frmAddNew.controls.languageId.setValue(
            Number(this.departmentTreeModel.languageId),
        );
    }

    loadAllLanguages() {
        return this.languagesService
            .getAllLanguages()
            .pipe(
                map((data) => {
                    this.languages = data;
                    this.selectedLanguageId = this.userService.getLanguageId();
                }),
                catchError((): any => {
                    this.notify.showTranslateMessage('ErrorLoadLanguages');
                }),
            )
            .subscribe((result) => {});
    }

    public languageChanged(language: LanguageModel): void {
        this.fatawaDepartmentService
            .getDepartmentByLanguageId(this.ID, language.id)
            .pipe(take(1))
            .subscribe((departmentModel: DepartmentNodeModel) => {
                this.departmentTreeModel = departmentModel;
                this.setDepartmentDetails();
            });
    }

    OnSubmit() {
        this.isInProgress = true;
        if (this.ID === 0) {
            this.fatawaDepartmentService
                .addDepartment(this.frmAddNew.value)
                .subscribe(
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
        } else {
            this.fatawaDepartmentService
                .updateDepartment(this.frmAddNew.value)
                .subscribe(
                    (next) => {
                        this.dialogRef.close(this.frmAddNew.value);
                        this.frmAddNew.reset();
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
