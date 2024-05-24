import {
    ChangeDetectionStrategy,
    Component,
    EventEmitter,
    Input,
    OnInit,
    Output,
} from '@angular/core';
import { FormGroup } from '@angular/forms';

import { FormService } from '@core/services/form.service';
import { IFormConfig } from '@models/form/form';

@Component({
    selector: 'app-dynamic-form',
    templateUrl: './dynamic-form.component.html',
    styleUrls: ['./dynamic-form.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DynamicFormComponent implements OnInit {
    @Input() public formConfig: IFormConfig;
    @Input() public formTitle: string;

    @Output() public emitSubmit = new EventEmitter<void>();

    public form: FormGroup;

    constructor(private formService: FormService) {}

    public ngOnInit(): void {
        this.form = this.createForm();
    }

    public submit(): void {
        this.emitSubmit.emit();
    }

    private createForm(): FormGroup {
        return this.formService.buildForm(this.formConfig);
    }
}
