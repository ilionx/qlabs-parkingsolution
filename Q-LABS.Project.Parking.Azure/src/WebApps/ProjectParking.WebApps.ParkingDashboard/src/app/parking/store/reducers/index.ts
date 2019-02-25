import { ActionReducerMap, createFeatureSelector } from '@ngrx/store';

import * as fromCarpark from './carpark.reducer';
import * as fromParkingSpot from './parking.reducer';

export interface ParkingState {
  carparks: fromCarpark.CarparkState;
  parkingspots: fromParkingSpot.ParkingSpotState;
}

export const reducers: ActionReducerMap<ParkingState> = {
  carparks: fromCarpark.reducer,
  parkingspots: fromParkingSpot.reducer
};

export const getParkingState = createFeatureSelector<ParkingState>('parking');


// import * as fromParking from './parking.reducer';

// import * as fromCarpark from './carpark.reducer';

// import * as fromRoot from '../../../store/reducers';
// import { ActionReducerMap, createFeatureSelector } from '@ngrx/store';

// export interface ParkingState {
//   parking: fromParking.State;
//   carparks: fromCarpark.CarparkState;
// }

// export interface State extends fromRoot.State {
//   parking: ParkingState;
// }

// export const reducers: ActionReducerMap<ParkingState> = {
//   parking: fromParking.reducer,
//   carparks: fromCarpark.reducer
// }

// export const getParkingState = createFeatureSelector<ParkingState>(
//   'parking'
// );

// export * from './carpark.reducer';
