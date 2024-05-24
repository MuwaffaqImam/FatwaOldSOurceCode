import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FatawaUsersComponent } from './fatawa-users.component';

describe('FatawaUsersComponent', () => {
    let component: FatawaUsersComponent;
    let fixture: ComponentFixture<FatawaUsersComponent>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            declarations: [FatawaUsersComponent],
        }).compileComponents();
    });

    beforeEach(() => {
        fixture = TestBed.createComponent(FatawaUsersComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
