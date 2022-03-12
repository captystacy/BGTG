import { Component, AfterViewInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

import { DurationByTCPService } from "./duration-by-tcp.service";

const APPENDIX_A_CATEGORIES: string[] =
  [
    'Уличные трубопроводы водо-, газоснабжения и канализации, сооружаемые в траншеях с откосами',
    'Уличные трубопроводы водо-, газоснабжения и канализации, сооружаемые в траншеях с креплением стенок',
    'Уличные тепловые сети, сооружаемые в траншеях с откосами',
    'Уличные тепловые сети, сооружаемые в траншеях с креплением стенок'
  ];

const APPENDIX_B_CATEGORIES: string[] =
  [
    'Наружные трубопроводы',
    'Распределительная газовая сеть'
  ];

const COMPONENT_MATERIALS_APPENDIX_A_2_AND_3: string[] =
  ['сборных железобетонных лотковых элементов'];

const COMPONENT_MATERIALS_APPENDIX_B_1: string[] =
  [
    'стальных труб в две нитки',
    'стальных труб в одну нитку',
    'полиэтиленовых труб в одну нитку'
  ];

const DEFAULT_COMPONENT_MATERIALS: string[] =
  [
    'стальных труб',
    'полиэтиленовых труб',
    'чугунных труб',
    'асбестоцементных труб',
    'керамических труб',
    'бестонных труб',
    'железобетонных труб',
    'стеклопластиковых труб'
  ];

@Component({
  selector: 'app-duration-by-tcp',
  templateUrl: './duration-by-tcp.component.html',
  styleUrls: ['./duration-by-tcp.component.css']
})
export class DurationByTCPComponent implements AfterViewInit {
  durationByTCPForm = new FormGroup({
    appendixKey: new FormControl('', [Validators.required]),
    pipelineCategoryName: new FormControl({ value: '', disabled: true }, [Validators.required]),
    pipelineMaterial: new FormControl({ value: '', disabled: true }, [Validators.required]),
    pipelineDiameter: new FormControl({ value: '', disabled: true }, [Validators.required, Validators.min(0.01)]),
    pipelineLength: new FormControl({ value: '', disabled: true }, [Validators.required, Validators.min(0.01)]),
  });

  get appendixKey() {
    return this.durationByTCPForm.get('appendixKey')!;
  }

  get pipelineCategoryName() {
    return this.durationByTCPForm.get('pipelineCategoryName')!;
  }

  get pipelineMaterial() {
    return this.durationByTCPForm.get('pipelineMaterial')!;
  }

  get pipelineDiameter() {
    return this.durationByTCPForm.get('pipelineDiameter')!;
  }

  get pipelineLength() {
    return this.durationByTCPForm.get('pipelineLength')!;
  }

  readonly appendixes =
    [
      { title: 'Нормы продолжительности строительства городских инженерных сетей и сооружений', value: 'A' },
      { title: 'Нормы продолжительности строительства объектов коммунального хозяйства', value: 'B' }
    ];

  pipelineCategories: string[] = [];

  pipelineMaterials: string[] = [];

  constructor(private _durationByTCPService: DurationByTCPService) { }

  ngAfterViewInit(): void {
    this.appendixKey.valueChanges.subscribe(newValue => {
      if (newValue === this.appendixes[0].value) {
        this.pipelineCategories = APPENDIX_A_CATEGORIES;
      } else {
        this.pipelineCategories = APPENDIX_B_CATEGORIES;
      }

      this.pipelineCategoryName.enable({ emitEvent: false });
      this.pipelineMaterial.disable({ emitEvent: false });
      this.pipelineDiameter.disable({ emitEvent: false });
      this.pipelineLength.disable({ emitEvent: false });
    });

    this.pipelineCategoryName.valueChanges.subscribe(newValue => {
      if (newValue === APPENDIX_A_CATEGORIES[2] || newValue === APPENDIX_A_CATEGORIES[3]) {
        this.pipelineMaterials = COMPONENT_MATERIALS_APPENDIX_A_2_AND_3;
      } else if (newValue === APPENDIX_B_CATEGORIES[1]) {
        this.pipelineMaterials = COMPONENT_MATERIALS_APPENDIX_B_1;
      } else {
        this.pipelineMaterials = DEFAULT_COMPONENT_MATERIALS;
      }

      this.pipelineMaterial.enable({ emitEvent: false });
      this.pipelineDiameter.disable({ emitEvent: false });
      this.pipelineLength.disable({ emitEvent: false });
    });

    this.pipelineMaterial.valueChanges.subscribe(newValue => {
      this.pipelineDiameter.enable({ emitEvent: false });
      this.pipelineLength.enable({ emitEvent: false });
    });
  }

  downloadDurationByTCP(): void {
    this._durationByTCPService.downloadDurationByTCP(this.durationByTCPForm.value);
  }
}
