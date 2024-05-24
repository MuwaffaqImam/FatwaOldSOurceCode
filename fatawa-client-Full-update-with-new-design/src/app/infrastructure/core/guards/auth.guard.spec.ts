import { HttpClientTestingModule } from '@angular/common/http/testing';
import { Router } from '@angular/router';
import { TestBed, getTestBed } from '@angular/core/testing';

import { AuthGuard } from './auth.guard';
import { environment } from '@env/environment.test';
import { Logger } from '@utils/logger';

!environment.testUnit
    ? Logger.log('Unit skipped')
    : describe('[Unit] AuthGuard', () => {
          let guard: AuthGuard;
          let injector: TestBed;
          const routerMock = {
              navigateByUrl: jasmine.createSpy('navigateByUrl'),
          };

          beforeEach(() => {
              TestBed.configureTestingModule({
                  providers: [
                      AuthGuard,
                      { provide: Router, useValue: routerMock },
                  ],
                  imports: [HttpClientTestingModule],
              });
              injector = getTestBed();
              guard = injector.inject(AuthGuard);
              let store = {};
              const mockSessionStorage = {
                  getItem: (key: string): string =>
                      key in store ? store[key] : null,
                  setItem: (key: string, value: string) => {
                      store[key] = `${value}`;
                  },
                  removeItem: (key: string) => {
                      delete store[key];
                  },
                  clear: () => {
                      store = {};
                  },
              };
              spyOn(sessionStorage, 'getItem').and.callFake(
                  mockSessionStorage.getItem,
              );
              spyOn(sessionStorage, 'setItem').and.callFake(
                  mockSessionStorage.setItem,
              );
              spyOn(sessionStorage, 'removeItem').and.callFake(
                  mockSessionStorage.removeItem,
              );
              spyOn(sessionStorage, 'clear').and.callFake(
                  mockSessionStorage.clear,
              );
          });
          it('should be created', () => {
              expect(guard).toBeTruthy();
          });
          describe('canActivate()', () => {
              it('should exist', () => {
                  const spy = spyOn(guard, 'canActivate');
                  expect(spy).toBeTruthy();
              });

              it('should return false when there is no user token', () => {
                  guard.canActivate().subscribe((response) => {
                      expect(response).toEqual(false);
                  });
              });

              it("should redirect to the '/auth' route when there is no user token", () => {
                  guard.canActivate().subscribe();
                  expect(routerMock.navigateByUrl).toHaveBeenCalledWith(
                      '/auth',
                  );
              });

              it('should return true for a logged in user', () => {
                  sessionStorage.setItem('authToken', '123');
                  guard.canActivate().subscribe((response) => {
                      expect(response).toEqual(true);
                  });
              });
          });

          describe('canActivateChild()', () => {
              it('should exist', () => {
                  const spy = spyOn(guard, 'canActivateChild');
                  expect(spy).toBeTruthy();
              });

              it('should call canActivate()', () => {
                  const canActivateChildSpy = spyOn(guard, 'canActivateChild');
                  const canActivateSpy = spyOn(guard, 'canActivate');
                  guard.canActivateChild();
                  canActivateChildSpy.and.returnValue(guard.canActivate());
                  expect(canActivateChildSpy).toHaveBeenCalled();
                  expect(canActivateSpy).toHaveBeenCalled();
              });
          });
      });
