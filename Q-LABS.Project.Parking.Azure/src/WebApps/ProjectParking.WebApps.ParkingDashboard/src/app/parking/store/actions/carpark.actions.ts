import { Action } from '@ngrx/store';

import { Carpark } from '../../models';

export const LOAD_CARPARKS = '[CARPARKS] LOAD';
export const LOAD_CARPARKS_SUCCESS = '[CARPARKS] LOAD SUCCESS';
export const LOAD_CARPARKS_FAIL = '[CARPARKS] LOAD FAIL';

export class LoadCarparks implements Action {
  readonly type = LOAD_CARPARKS;
}

export class LoadCarparksSuccess implements Action {
  readonly type = LOAD_CARPARKS_SUCCESS;

  constructor(public payload: Carpark[]) {}
}

export class LoadCarparksFail implements Action {
  readonly type = LOAD_CARPARKS_FAIL;

  constructor (public error: any) {}
}

export type CarparksAction =
  | LoadCarparks
  | LoadCarparksSuccess
  | LoadCarparksFail;
