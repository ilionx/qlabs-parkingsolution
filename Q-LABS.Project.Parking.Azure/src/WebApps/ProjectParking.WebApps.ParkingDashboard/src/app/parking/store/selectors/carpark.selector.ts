import { createSelector } from '@ngrx/store';

// import * as fromRoot from '../../../store';
import * as fromFeature from '../reducers';
import * as fromCarpark from '../reducers/carpark.reducer';

import { Carpark } from '../../models';

export const getCarparkState = createSelector(
  fromFeature.getParkingState,
  (state: fromFeature.ParkingState) => state.carparks
);

export const getCarparkEntities = createSelector(
  getCarparkState,
  fromCarpark.getCarparkEntities
);

export const getCarparksLoaded = createSelector(
  getCarparkState,
  fromCarpark.getCarparkLoaded
);

export const getCarparksLoading = createSelector(
  getCarparkState,
  fromCarpark.getCarparkLoading
);

export const getAllCarparks = createSelector(getCarparkEntities, entities => {
  return Object.keys(entities).map(id => entities[parseInt(id, 10)]);
});
