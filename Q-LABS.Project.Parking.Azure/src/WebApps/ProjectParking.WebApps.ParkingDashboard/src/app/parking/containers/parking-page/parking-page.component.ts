import { Component, OnInit } from "@angular/core";
import { Store } from "@ngrx/store";

import * as fromParking from '../../store';
import { Observable } from "rxjs";
import { Carpark, ParkingSpotUpdate } from "../../models";
import { SocketService } from "../../../socket.service";

// import { Store } from "@ngrx/store";
// import { ParkingSpotUpdate } from "../../models";

@Component({
  selector: 'qnh-parking-page',
  templateUrl: './parking-page.component.html',
  styleUrls: ['./parking-page.component.scss']
})
export class ParkingPageComponent implements OnInit {


  constructor(private store: Store<fromParking.ParkingState>, private socketService: SocketService) {}

  carparks$: Observable<Carpark[]>;

  ngOnInit() {

    this.carparks$ = this.store.select(fromParking.getAllCarparks);

    this.store.select(fromParking.getAllParkingSpots).subscribe(spots => {
      console.log("spots", spots);
    });

      // this.socketService.listen('NotifyParkingStatusForCarpark').subscribe(data => console.log('socket data', data));

    this.socketService.listen('NotifyParkingSpotUpdate').subscribe((data: ParkingSpotUpdate) => {
      console.log("data before dispatch", data);
      this.store.dispatch(new fromParking.ParkingSpotUpdateAction(data));
    }
    );

    this.store.dispatch(new fromParking.LoadCarparks());

    // function repeat(store: Store<fromParking.State>) {
    //   console.log('DISPATCH ACTION TO REDRAW');

    //   const spotUpdate: ParkingSpotUpdate = {
    //     carparkId: 1,
    //     spotId: 1,
    //     lastUpdated: (new Date()).toString(),
    //     status: 'Active'
    //   };

    //   store.dispatch( new fromParking.ParkingSpotUpdateAction(spotUpdate));
    //   setTimeout(() => {
    //     repeat(store);
    //   }, 1000);
    // }

    // repeat(this.store);



  }
}
