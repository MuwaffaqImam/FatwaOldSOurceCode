import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SharedSliderComponent } from './shared-slider.component';

describe('SharedSliderComponent', () => {
    let component: SharedSliderComponent;
    let fixture: ComponentFixture<SharedSliderComponent>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            declarations: [SharedSliderComponent],
        }).compileComponents();
    });

    beforeEach(() => {
        fixture = TestBed.createComponent(SharedSliderComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
