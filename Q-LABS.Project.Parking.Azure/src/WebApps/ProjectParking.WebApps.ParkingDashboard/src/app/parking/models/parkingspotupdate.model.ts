export interface ParkingSpotUpdate {
  carparkId: number;
  spotId: number;
  status: SpotStatus;
  updatedOn: string;
  source: MessageSource;
}

export enum MessageSource {
  MessageBus = 1,
  System = 2
}

export enum SpotStatus {
  Available = 0,
  Unavailable = 1,
  Unknown = 2
}
