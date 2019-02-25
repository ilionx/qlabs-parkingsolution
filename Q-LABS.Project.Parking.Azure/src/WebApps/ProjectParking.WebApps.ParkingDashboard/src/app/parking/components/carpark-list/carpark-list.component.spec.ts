import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CarparkListComponent } from './carpark-list.component';

describe('CarparkListComponent', () => {
  let component: CarparkListComponent;
  let fixture: ComponentFixture<CarparkListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CarparkListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CarparkListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
