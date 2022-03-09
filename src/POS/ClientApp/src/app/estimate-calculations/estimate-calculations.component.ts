import { Component } from '@angular/core';

@Component({
  selector: 'app-estimate-calculations',
  templateUrl: './estimate-calculations.component.html',
  styleUrls: ['./estimate-calculations.component.css']
})
export class EstimateCalculationsComponent {
  estimateFiles: File[] = [];

  estimateFilesSelected(event: any) {
    this.estimateFiles = event.target.files;
  }
}
