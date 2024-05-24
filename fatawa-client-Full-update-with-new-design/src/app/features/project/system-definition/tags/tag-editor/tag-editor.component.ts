import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { TagService } from '@app/infrastructure/core/services/system-definition/tag.service';
import { Router, ActivatedRoute } from '@angular/router';
import { TagModel } from '@app/infrastructure/models/project/system-definition/tagModel';

@Component({
    selector: 'app-tag-editor',
    templateUrl: './tag-editor.component.html',
    styleUrls: ['./tag-editor.component.scss'],
})
export class TagEditorComponent implements OnInit {
    id: string;
    editorTagForm: FormGroup;
    submitted = false;
    data: TagModel;
    constructor(
        private fb: FormBuilder,
        private tagService: TagService,
        private router: Router,
        private activatedroute: ActivatedRoute,
    ) {
        this.editorTagForm = this.fb.group({
            tagName: ['', [Validators.required]],
            enabled: true,
            id: Number,
        });
    }

    ngOnInit(): void {
        this.id = this.activatedroute.snapshot.paramMap.get('id');
        if (this.id !== '0') {
            this.tagService.GetTag(this.id).subscribe((res) => {
                this.editorTagForm.setValue({
                    tagName: res.tagName,
                    enabled: res.enabled,
                    id: res.id,
                });
            });
        }
    }

    get form() {
        return this.editorTagForm.controls;
    }

    submitTag(): void {
        this.submitted = true;
        if (this.editorTagForm.invalid) {
            return;
        }
        this.tagService.AddTag(this.editorTagForm.value).subscribe((res) => {
            this.router.navigate(['system-definition/tags']);
        });
    }
}
