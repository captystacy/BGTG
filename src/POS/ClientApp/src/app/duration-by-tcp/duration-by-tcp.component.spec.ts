import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DurationByTcpComponent } from './duration-by-tcp.component';

describe('DurationByTcpComponent', () => {
  let component: DurationByTcpComponent;
  let fixture: ComponentFixture<DurationByTcpComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DurationByTcpComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DurationByTcpComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
