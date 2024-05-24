import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { FatawaLiveListComponent } from './fatawa-live-list.component';

describe('FatawaLiveListComponent', () => {
    let component: FatawaLiveListComponent;
    let fixture: ComponentFixture<FatawaLiveListComponent>;

    beforeEach(
        waitForAsync(() => {
            TestBed.configureTestingModule({
                declarations: [FatawaLiveListComponent],
            }).compileComponents();
        }),
    );

    beforeEach(() => {
        fixture = TestBed.createComponent(FatawaLiveListComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
