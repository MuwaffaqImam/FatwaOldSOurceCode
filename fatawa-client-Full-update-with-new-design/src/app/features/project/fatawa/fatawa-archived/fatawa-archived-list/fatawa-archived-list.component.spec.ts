import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { FatawaArchivedListComponent } from './fatawa-archived-list.component';

describe('FatawaArchivedListComponent', () => {
    let component: FatawaArchivedListComponent;
    let fixture: ComponentFixture<FatawaArchivedListComponent>;

    beforeEach(
        waitForAsync(() => {
            TestBed.configureTestingModule({
                declarations: [FatawaArchivedListComponent],
            }).compileComponents();
        }),
    );

    beforeEach(() => {
        fixture = TestBed.createComponent(FatawaArchivedListComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
