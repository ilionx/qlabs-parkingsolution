import { createSelector } from '@ngrx/store';

// import * as fromRoot from '../../../store';
import * as fromFeature from '../reducers';
import * as fromParkingspot from '../reducers/parking.reducer';

export const getParkingSpotState = createSelector(
  fromFeature.getParkingState,
  (state: fromFeature.ParkingState) => state.parkingspots
);

export const getParkingSpotEntities = createSelector(
  getParkingSpotState,
  fromParkingspot.getParkingSpotEntities
);


export const getAllParkingSpots = createSelector(getParkingSpotEntities, entities => {
  console.log("getParkingSpotEntities",entities);
  return Object.keys(entities).map(id => entities[parseInt(id, 10)]);
});
