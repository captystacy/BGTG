import { Component, Input } from '@angular/core';
import { EnergyAndWaterService } from "./energy-and-water.service";

@Component({
  selector: 'app-energy-and-water',
  templateUrl: './energy-and-water.component.html',
  styleUrls: ['./energy-and-water.component.css']
})
export class EnergyAndWaterComponent {
  @Input() estimateFiles?: FileList;

  constructor(private _energyAndWaterService: EnergyAndWaterService) { }

  downloadEnergyAndWater(): void {
    this._energyAndWaterService.downloadEnergyAndWater(this.estimateFiles!);
  }
}
