import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SharedFatawacategoriesComponent } from './shared-fatawacategories.component';

describe('SharedFatawacategoriesComponent', () => {
    let component: SharedFatawacategoriesComponent;
    let fixture: ComponentFixture<SharedFatawacategoriesComponent>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            declarations: [SharedFatawacategoriesComponent],
        }).compileComponents();
    });

    beforeEach(() => {
        fixture = TestBed.createComponent(SharedFatawacategoriesComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
