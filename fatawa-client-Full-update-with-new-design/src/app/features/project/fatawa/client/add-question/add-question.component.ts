import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NotificationService } from '@app/infrastructure/core/services/notification.service';
import { QuestionService } from '@app/infrastructure/core/services/fatawa/question.service';
import { UserService } from '@app/infrastructure/core/services/auth/user.service';
import { Constants } from '@app/infrastructure/utils/constants';
import { map } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
@Component({
    selector: 'app-add-question',
    templateUrl: './add-question.component.html',
    styleUrls: ['./add-question.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AddQuestionComponent implements OnInit {
    frmAddNew: FormGroup;
    public isInProgress = false;
    public showQuestion = false;
    public dir: string = Constants.DefaultLanguageDirection;

    constructor(
        private formBuilder: FormBuilder,
        private questionService: QuestionService,
        private notify: NotificationService,
        public userService: UserService,
        private toastr: ToastrService,
    ) {}

    ngOnInit(): void {
        this.ngInitialControlForm();
    }

    ngInitialControlForm() {
        this.frmAddNew = this.formBuilder.group({
            FatawaQuestion: ['', Validators.required],
            StatusId: [1, Validators.required],
        });
    }

    OnSubmit() {
        if (!this.userService.isTokenExist()) {
            this.toastr.success('Hello world!', 'Toastr fun!');
        } else {
            this.isInProgress = true;
            this.questionService
                .addQuestion(this.frmAddNew.value)
                .pipe(
                    map((data) => {
                        if (data) {
                            //Invoke For Admin only
                            this.notify.invokeAddedNewQuestion();
                        }
                    }),
                )
                .subscribe((result) => {
                    this.notify.showTranslateMessage(
                        'AddedSuccessfully',
                        false,
                    );
                    this.resetFormBuilder();
                });
        }
    }

    resetFormBuilder() {
        this.frmAddNew.reset();
        this.frmAddNew.controls.StatusId.setValue(1);
        this.isInProgress = false;
    }

    toggle() {
        this.showQuestion = !this.showQuestion;
    }
}
