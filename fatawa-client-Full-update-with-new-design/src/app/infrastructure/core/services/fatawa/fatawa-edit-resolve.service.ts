import { Injectable } from '@angular/core';
import {
    ActivatedRouteSnapshot,
    Resolve,
    RouterStateSnapshot,
} from '@angular/router';
import { FatawaModel } from '@app/infrastructure/models/project/fatawa/FatawaModel';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { UserService } from '../auth/user.service';
import { FatawaService } from './fatawa.service';

@Injectable({
    providedIn: 'root',
})
export class FatawaDetailsResolveService implements Resolve<FatawaModel> {
    constructor(
        private fatawaService: FatawaService,
        private userService: UserService,
    ) {}

    resolve(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot,
    ): Observable<FatawaModel> {
        if (route.params.id) {
            return this.fatawaService.getFatawa(route.params.id).pipe(take(1));
        } else {
            return this.fatawaService.getFatawaDefaultSettingsByUser();
        }
    }
}
