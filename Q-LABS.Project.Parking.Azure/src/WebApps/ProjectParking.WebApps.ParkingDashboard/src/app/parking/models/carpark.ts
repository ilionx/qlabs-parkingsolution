import { CarparkLocation } from './carpark-location';

export interface Carpark {
  id: number;
  name: string;
  location: CarparkLocation;
  availableParkingSpots: number;

}
