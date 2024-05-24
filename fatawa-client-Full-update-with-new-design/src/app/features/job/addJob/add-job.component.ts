import {
    Component,
    OnInit,
    ChangeDetectionStrategy,
    Inject,
} from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { JobModel } from '@models/project/JobModel';
import { MatDialogRef } from '@angular/material/dialog';
import { JobsService } from '@core/services/job/job.service';

@Component({
    selector: 'app-add-job',
    templateUrl: './add-job.component.html',
    styleUrls: ['./add-job.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AddJobComponent implements OnInit {
    frmAddNew: FormGroup;
    public isInProgress = false;

    constructor(
        @Inject(MAT_DIALOG_DATA) public jobModel: any,
        private formBuilder: FormBuilder,
        private jobsService: JobsService,
        private dialogRef: MatDialogRef<AddJobComponent>,
    ) { }

    get ID() {
        return this.frmAddNew.controls.Id.value;
    }

    ngOnInit(): void {
        this.ngInitialControlForm();
        this.setJobDetails();
    }

    ngInitialControlForm() {
        this.frmAddNew = this.formBuilder.group({
            Id: [0],
            JobName: ['', Validators.required],
        });
    }

    setJobDetails() {
        if (this.jobModel) {
            this.frmAddNew.controls.Id.setValue(this.jobModel.id);
            this.frmAddNew.controls.JobName.setValue(this.jobModel.jobName);
        }
    }

    OnSubmit() {
        this.isInProgress = true;
        if (this.ID === 0) {
            this.jobsService.Post(this.frmAddNew.value).subscribe(
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
            this.jobsService.Put(this.ID, this.frmAddNew.value).subscribe(
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
