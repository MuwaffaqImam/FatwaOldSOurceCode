import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FatawaImportComponent } from './fatawa-import.component';

describe('FatawaImportComponent', () => {
    let component: FatawaImportComponent;
    let fixture: ComponentFixture<FatawaImportComponent>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            declarations: [FatawaImportComponent],
        }).compileComponents();
    });

    beforeEach(() => {
        fixture = TestBed.createComponent(FatawaImportComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
