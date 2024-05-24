import { Injectable } from '@angular/core';
import {
    ActivatedRouteSnapshot,
    Resolve,
    RouterStateSnapshot,
} from '@angular/router';
import { FatawaModel } from '@app/infrastructure/models/project/fatawa/FatawaModel';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { FatawaService } from './fatawa.service';

@Injectable({
    providedIn: 'root',
})
export class FatawaDefaultSettingsResolveService
    implements Resolve<FatawaModel> {
    constructor(private fatawaService: FatawaService) {}

    resolve(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot,
    ): Observable<FatawaModel> {
        return this.fatawaService
            .getFatawaDefaultSettings(route.params.id)
            .pipe(take(1));
    }
}
