import { TestBed } from '@angular/core/testing';

import { DurationByLCService } from './duration-by-lc.service';

describe('DurationByLcService', () => {
  let service: DurationByLCService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DurationByLCService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
