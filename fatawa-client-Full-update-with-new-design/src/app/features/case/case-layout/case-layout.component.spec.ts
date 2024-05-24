import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';

import { BannerComponent } from '@shared/components/banner/banner.component';
import { CaseLayoutComponent } from './case-layout.component';
import { environment } from '@env/environment.test';
import { Logger } from '@utils/logger';

!environment.testIntegration
    ? Logger.log('Integration skipped')
    : describe('[Integration] CaseLayoutComponent', () => {
          let component: CaseLayoutComponent;
          let fixture: ComponentFixture<CaseLayoutComponent>;

          beforeEach(
              waitForAsync(() => {
                  TestBed.configureTestingModule({
                      declarations: [CaseLayoutComponent, BannerComponent],
                      imports: [RouterTestingModule],
                  }).compileComponents();
              }),
          );

          beforeEach(() => {
              fixture = TestBed.createComponent(CaseLayoutComponent);
              component = fixture.componentInstance;
              fixture.detectChanges();
          });

          it('should create', () => {
              expect(component).toBeTruthy();
          });
      });
