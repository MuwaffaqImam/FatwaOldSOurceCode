import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FatawaTranslationComponent } from './fatawa-translation.component';

describe('FatawaTranslationComponent', () => {
    let component: FatawaTranslationComponent;
    let fixture: ComponentFixture<FatawaTranslationComponent>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            declarations: [FatawaTranslationComponent],
        }).compileComponents();
    });

    beforeEach(() => {
        fixture = TestBed.createComponent(FatawaTranslationComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
