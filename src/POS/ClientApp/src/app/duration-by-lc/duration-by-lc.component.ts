import { Component, Input } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';

import { IDurationByLC } from "./duration-by-lc.model";
import { DurationByLCService } from "./duration-by-lc.service";

@Component({
  selector: 'app-duration-by-lc',
  templateUrl: './duration-by-lc.component.html',
  styleUrls: ['./duration-by-lc.component.css']
})
export class DurationByLCComponent {
  @Input() estimateFiles!: File[];
  durationByLC: IDurationByLC = {
    numberOfWorkingDays: 21.5,
    workingDayDuration: 8,
    shift: 1.5,
    numberOfEmployees: 4,
    technologicalLaborCosts: 0,
    acceptanceTimeIncluded: true
  };

  constructor(private _durationByLCService: DurationByLCService) { }

  downloadDurationByLC(): void {
    this._durationByLCService.downloadDurationByLc(this.durationByLC, this.estimateFiles);
  }
}
