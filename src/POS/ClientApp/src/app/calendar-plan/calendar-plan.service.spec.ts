import { TestBed } from '@angular/core/testing';

import { CalendarPlanService } from './calendar-plan.service';

describe('CalendarPlanService', () => {
  let service: CalendarPlanService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CalendarPlanService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
