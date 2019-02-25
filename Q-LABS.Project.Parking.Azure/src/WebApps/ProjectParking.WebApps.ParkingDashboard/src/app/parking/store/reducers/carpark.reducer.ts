import * as fromCarpark from '../actions/carpark.actions';
import { Carpark } from '../../models';

export interface CarparkState {
  entities: { [id: number]: Carpark};
  loaded: boolean;
  loading: boolean;
}

export const initialState: CarparkState = {
  entities: {},
  loaded: false,
  loading: false
};

export function reducer (
  state = initialState,
  action: fromCarpark.CarparksAction
): CarparkState {

  switch (action.type) {
    case fromCarpark.LOAD_CARPARKS: {
      return {
        ...state,
        loading: true
      };
    }

    case fromCarpark.LOAD_CARPARKS_SUCCESS: {
      const carparks = action.payload;

      const entities = carparks.reduce(
        // tslint:disable-next-line:no-shadowed-variable
        (entities: { [id: number]: Carpark }, carpark: Carpark) => {
          return {
            ...entities,
            [carpark.id]: carpark,
          };
        },
        {
          ...state.entities,
        }
      );

      return {
        ...state,
        loading: false,
        loaded: true,
        entities,
      };
    }

    case fromCarpark.LOAD_CARPARKS_FAIL: {
      return {
        ...state,
        loaded: false,
        loading: false
      }
    }
  }

  return state;

}

export const getCarparkEntities = (state: CarparkState) => state.entities;
export const getCarparkLoading = (state: CarparkState) => state.loading;
export const getCarparkLoaded = (state: CarparkState) => state.loaded;
