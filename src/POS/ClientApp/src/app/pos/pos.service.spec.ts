import { TestBed } from '@angular/core/testing';

import { POSService } from './pos.service';

describe('ProjectService', () => {
  let service: POSService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(POSService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
