import { TestBed } from '@angular/core/testing';

import { EnergyAndWaterService } from './energy-and-water.service';

describe('EnergyAndWaterService', () => {
  let service: EnergyAndWaterService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EnergyAndWaterService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
