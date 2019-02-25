import { Action } from '@ngrx/store';
import { ParkingSpotUpdate } from '../../models';

export enum ParkingActionTypes {
  SpotUpdate = '[Parking] Spot Update'
}

export class ParkingSpotUpdateAction implements Action {
  readonly type = ParkingActionTypes.SpotUpdate;

  constructor(public payload: ParkingSpotUpdate) {}
}

export type ParkingActionsUnion =
  | ParkingSpotUpdateAction;
