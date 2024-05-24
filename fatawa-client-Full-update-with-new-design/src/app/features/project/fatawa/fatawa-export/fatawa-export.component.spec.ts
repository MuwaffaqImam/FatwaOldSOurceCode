import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FatawaExportComponent } from './fatawa-export.component';

describe('FatawaExportComponent', () => {
    let component: FatawaExportComponent;
    let fixture: ComponentFixture<FatawaExportComponent>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            declarations: [FatawaExportComponent],
        }).compileComponents();
    });

    beforeEach(() => {
        fixture = TestBed.createComponent(FatawaExportComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
