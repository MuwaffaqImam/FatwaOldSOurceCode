import {
    HttpClientTestingModule,
    HttpTestingController,
} from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { of as observableOf } from 'rxjs';

import { ApiService } from '../api/api.service';
import { CaseService } from './case.service';
import { environment } from '@env/environment.test';
import { Logger } from '@utils/logger';
import { ICaseDTO } from '@models/project/case';

!environment.testUnit
    ? Logger.log('Unit skipped')
    : describe('[Unit] CaseService', () => {
          let service: CaseService;
          let apiService: ApiService;
          let httpTestingController: HttpTestingController;
          beforeEach(() => {
              TestBed.configureTestingModule({
                  imports: [HttpClientTestingModule],
                  providers: [ApiService, CaseService],
              });
              // Returns a service with the MockBackend so we can test with dummy responses
              service = TestBed.inject(CaseService);
              apiService = TestBed.inject(ApiService);
              // Inject the http service and test controller for each test
              httpTestingController = TestBed.inject(HttpTestingController);
          });
          it('should be created', () => {
              expect(service).toBeTruthy();
          });
          describe('getCaseList()', () => {
              it('should exist', () => {
                  const spy = spyOn(service, 'getCaseList');
                  expect(spy).toBeTruthy();
              });
              it('should return a list of cases', () => {
                  const expectedValue: ICaseDTO = {
                      results: [
                          {
                              id: 1,
                              name: 'Test T Tester',
                              dob: '1981-08-27T10:44:12Z',
                              gender: 'Male',
                              status: 'completed',
                              decision: 'release',
                          },
                      ],
                  };
                  let response: ICaseDTO;
                  spyOn(service, 'getCaseList').and.returnValue(
                      observableOf(expectedValue),
                  );
                  service.getCaseList().subscribe((res) => {
                      response = res;
                  });
                  expect(response).toEqual(expectedValue);
              });
          });
      });
