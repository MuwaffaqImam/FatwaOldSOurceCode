import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FatawaDepartmentsComponent } from './fatawa-departments.component';

describe('FatawaDepartmentsComponent', () => {
    let component: FatawaDepartmentsComponent;
    let fixture: ComponentFixture<FatawaDepartmentsComponent>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            declarations: [FatawaDepartmentsComponent],
        }).compileComponents();
    });

    beforeEach(() => {
        fixture = TestBed.createComponent(FatawaDepartmentsComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
