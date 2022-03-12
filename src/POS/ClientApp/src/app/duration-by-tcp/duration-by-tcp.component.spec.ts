import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DurationByTCPComponent } from './duration-by-tcp.component';

describe('DurationByTCPComponent', () => {
  let component: DurationByTCPComponent;
  let fixture: ComponentFixture<DurationByTCPComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DurationByTCPComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DurationByTCPComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
