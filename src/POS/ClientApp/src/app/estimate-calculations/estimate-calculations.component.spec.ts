import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EstimateCalculationsComponent } from './estimate-calculations.component';

describe('EstimateCalculationsComponent', () => {
  let component: EstimateCalculationsComponent;
  let fixture: ComponentFixture<EstimateCalculationsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EstimateCalculationsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EstimateCalculationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
