import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AssignMuftiComponent } from './assign-mufti.component';

describe('AssignMuftiComponent', () => {
    let component: AssignMuftiComponent;
    let fixture: ComponentFixture<AssignMuftiComponent>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            declarations: [AssignMuftiComponent],
        }).compileComponents();
    });

    beforeEach(() => {
        fixture = TestBed.createComponent(AssignMuftiComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
