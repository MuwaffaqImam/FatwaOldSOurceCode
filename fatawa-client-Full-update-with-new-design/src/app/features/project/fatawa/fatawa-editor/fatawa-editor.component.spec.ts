import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { FatawaEditorComponent } from './fatawa-editor.component';

describe('FatawaEditorComponent', () => {
    let component: FatawaEditorComponent;
    let fixture: ComponentFixture<FatawaEditorComponent>;

    beforeEach(
        waitForAsync(() => {
            TestBed.configureTestingModule({
                declarations: [FatawaEditorComponent],
            }).compileComponents();
        }),
    );

    beforeEach(() => {
        fixture = TestBed.createComponent(FatawaEditorComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
