import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing';

import { CaseListPresentationComponent } from './case-list-presentation.component';
import { CaseListSmartComponent } from './case-list-smart.component';
import { environment } from '@env/environment.test';
import { Logger } from '@utils/logger';

!environment.testIntegration
    ? Logger.log('Integration skipped')
    : describe('[Integration] CaseListSmartComponent', () => {
          let component: CaseListSmartComponent;
          let fixture: ComponentFixture<CaseListSmartComponent>;

          beforeEach(
              waitForAsync(() => {
                  TestBed.configureTestingModule({
                      declarations: [
                          CaseListPresentationComponent,
                          CaseListSmartComponent,
                      ],
                      imports: [HttpClientTestingModule, RouterTestingModule],
                  }).compileComponents();
              }),
          );

          beforeEach(() => {
              fixture = TestBed.createComponent(CaseListSmartComponent);
              component = fixture.componentInstance;
              fixture.detectChanges();
          });

          it('should create', () => {
              expect(component).toBeTruthy();
          });
      });
