import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EnergyAndWaterComponent } from './energy-and-water.component';

describe('EnergyAndWaterComponent', () => {
  let component: EnergyAndWaterComponent;
  let fixture: ComponentFixture<EnergyAndWaterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EnergyAndWaterComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EnergyAndWaterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
