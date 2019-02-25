import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { Carpark } from '../../models';
import { MatPaginator, MatSort } from '@angular/material';
import { TableDataSource } from './table-datasource';

@Component({
  selector: 'qnh-carpark-list',
  templateUrl: './carpark-list.component.html',
  styleUrls: ['./carpark-list.component.scss']
})
export class CarparkListComponent implements OnInit {

  @Input()
  carparks: Carpark[];

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  dataSource: TableDataSource;

  /** Columns displayed in the table. Columns IDs can be added, removed, or reordered. */
  displayedColumns = ['id', 'name'];

  ngOnInit() {
    console.log("this.carpakrs?", this.carparks);
    this.dataSource = new TableDataSource(this.carparks, this.paginator, this.sort);
  }

}
