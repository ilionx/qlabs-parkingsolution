import { ParkingActionsUnion, ParkingActionTypes } from "../actions";
import { SpotStatus } from "../../models";

export interface Item {
  carparkId: number;
  spots: { [spotId: number]: Item2 };
}

export interface Item2 {
  spotId: number;
  updatedOn: string;
  status: SpotStatus;
}

export interface ParkingSpotState {
  carparks: { [carparkId: number]: Item};
  // entities: { [[carpark: number, spot:number]]: any};
}

export const initialState: ParkingSpotState = {
  carparks: {}
  // entities: {}
};

export function reducer(
  state = initialState,
  action: ParkingActionsUnion
): ParkingSpotState {

  switch (action.type) {

    case ParkingActionTypes.SpotUpdate: {

      const payload = action.payload;

      let existingItem = state.carparks[payload.carparkId];
      if (existingItem === undefined) {
        existingItem = {
          carparkId: payload.carparkId,
          spots: {}
        };
      }
        existingItem.spots[payload.spotId] = {
          spotId: payload.spotId,
          updatedOn: payload.updatedOn,
          status: payload.status,
        };

      console.log("existingItem",existingItem);

      const carparks = {
        ...state.carparks,
        [payload.carparkId]: existingItem
      };

      // const entities = [
      //   ...state.parkingspots
      // ];

      //  const update = action.payload;
      // const carparks = action.payload;

      // const entities = carparks.reduce(
      //   // tslint:disable-next-line:no-shadowed-variable
      //   (entities: { [id: number]: Carpark }, carpark: Carpark) => {
      //     return {
      //       ...entities,
      //       [carpark.id]: carpark,
      //     };
      //   },
      //   {
      //     ...state.entities,
      //   }
      // );


      return {
        ...state,
        carparks
      };
    }
    default: {
      return state;
    }
  }

}

export const getParkingSpotEntities = (state: ParkingSpotState) => state.carparks;
