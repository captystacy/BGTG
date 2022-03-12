import { TestBed } from '@angular/core/testing';

import { DurationByTCPService } from './duration-by-tcp.service';

describe('DurationByTcpService', () => {
  let service: DurationByTCPService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DurationByTCPService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
