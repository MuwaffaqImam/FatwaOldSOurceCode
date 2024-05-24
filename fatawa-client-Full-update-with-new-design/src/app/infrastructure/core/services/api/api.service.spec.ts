import {
    HttpClientTestingModule,
    HttpTestingController,
} from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { ApiService } from './api.service';
import { ICaseResponse } from '@models/project/case';
import { environment } from '@env/environment.test';
import { Logger } from '@utils/logger';

!environment.testUnit
    ? Logger.log('Unit skipped')
    : describe('[Unit] ApiService', () => {
          beforeEach(() => {
              TestBed.configureTestingModule({
                  providers: [ApiService],
                  imports: [HttpClientTestingModule],
              });
          });
          it('should be created', () => {
              expect(ApiService).toBeTruthy();
          });
          describe('get<T>()', () => {
              function setup() {
                  const service: ApiService = TestBed.inject(ApiService);
                  const httpTestingController: HttpTestingController = TestBed.inject(
                      HttpTestingController,
                  );
                  return { service, httpTestingController };
              }
              it('should be created', () => {
                  const { service } = setup();
                  const spy = spyOn(service, 'get');
                  expect(spy).toBeTruthy();
              });
              it('should have a request method of GET', () => {
                  const route = 'http://0.0.0.0:3000/api/cases';
                  const { service, httpTestingController } = setup();
                  const testResponse: ICaseResponse = {
                      results: [
                          {
                              id: 1,
                              firstName: 'Test',
                              lastName: 'Tester',
                              middleName: 'T',
                              dob: '1981-08-27T10:44:12Z',
                              gender: 'Male',
                              status: 'completed',
                              decision: 'release',
                          },
                      ],
                  };
                  service.get<ICaseResponse>(route).subscribe();
                  const req = httpTestingController.expectOne(route);
                  expect(req.request.method).toBe('GET');
              });
              it('should contain the expected response', () => {
                  const route = 'http://0.0.0.0:5300/api/cases';
                  const { service, httpTestingController } = setup();
                  const testResponse: ICaseResponse = {
                      results: [
                          {
                              id: 1,
                              firstName: 'Test',
                              lastName: 'Tester',
                              middleName: 'T',
                              dob: '1981-08-27T10:44:12Z',
                              gender: 'Male',
                              status: 'completed',
                              decision: 'release',
                          },
                      ],
                  };
                  service.get<ICaseResponse>(route).subscribe((response) => {
                      expect(response).toEqual(testResponse);
                  });
                  const req = httpTestingController.expectOne(route);
                  req.flush(testResponse);
              });
              afterEach(() => {
                  const { httpTestingController } = setup();
                  httpTestingController.verify();
              });
          });
      });
