import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DurationByLCComponent } from './duration-by-lc.component';

describe('DurationByLcComponent', () => {
  let component: DurationByLCComponent;
  let fixture: ComponentFixture<DurationByLCComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DurationByLCComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DurationByLCComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
