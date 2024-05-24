import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FatawaFullListComponent } from './fatawa-full-list.component';

describe('FatawaFullListComponent', () => {
    let component: FatawaFullListComponent;
    let fixture: ComponentFixture<FatawaFullListComponent>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            declarations: [FatawaFullListComponent],
        }).compileComponents();
    });

    beforeEach(() => {
        fixture = TestBed.createComponent(FatawaFullListComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
