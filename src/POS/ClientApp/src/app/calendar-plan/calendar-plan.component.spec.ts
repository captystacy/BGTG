import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CalendarPlanComponent } from './calendar-plan.component';

describe('CalendarPlanComponent', () => {
  let component: CalendarPlanComponent;
  let fixture: ComponentFixture<CalendarPlanComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CalendarPlanComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CalendarPlanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
