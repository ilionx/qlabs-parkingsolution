import { Injectable } from '@angular/core';
import { Actions, Effect } from '@ngrx/effects';
import * as carparkActions from '../actions/carpark.actions';
import { switchMap, map, catchError } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { Carpark } from '../../models/carpark';
import { CarparkService } from '../../services/carpark.service';

@Injectable()
export class CarparkEffects {

  constructor(private actions$: Actions, private carparkService: CarparkService) {}

  @Effect()
  loadCarparks$ = this.actions$.ofType(carparkActions.LOAD_CARPARKS).pipe(
    switchMap(() => {
      return this.carparkService.getCarparks()
        .pipe(
          map(apiResponse => {
            const apiResponseCarpark = apiResponse.value.map( x => x.value);
            return new carparkActions.LoadCarparksSuccess(apiResponseCarpark); }),
          catchError(error => of (new carparkActions.LoadCarparksFail(error)))
        );
    })
  );

  public load(): Observable<Carpark[]> {
    return of([]);
  }


}
