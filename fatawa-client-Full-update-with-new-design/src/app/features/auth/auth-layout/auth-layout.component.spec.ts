import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';

import { AuthLayoutComponent } from './auth-layout.component';
import { BannerComponent } from '@shared/components/banner/banner.component';

describe('AuthLayoutComponent', () => {
    let component: AuthLayoutComponent;
    let fixture: ComponentFixture<AuthLayoutComponent>;

    beforeEach(
        waitForAsync(() => {
            TestBed.configureTestingModule({
                declarations: [AuthLayoutComponent, BannerComponent],
                imports: [RouterTestingModule],
            }).compileComponents();
        }),
    );

    beforeEach(() => {
        fixture = TestBed.createComponent(AuthLayoutComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
