import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-duration-by-tcp',
  templateUrl: './duration-by-tcp.component.html',
  styleUrls: ['./duration-by-tcp.component.css']
})
export class DurationByTcpComponent implements OnInit {
  readonly norms: string[] =
  [
    "Нормы продолжительности строительства городских инженерных сетей и сооружений",
    "Нормы продолжительности строительства объектов коммунального хозяйства"
  ];

  readonly appendixACategories: string[] =
  [
    'Уличные трубопроводы водо-, газоснабжения и канализации, сооружаемые в траншеях с откосами',
    'Уличные трубопроводы водо-, газоснабжения и канализации, сооружаемые в траншеях с креплением стенок',
    'Уличные тепловые сети, сооружаемые в траншеях с откосами',
    'Уличные тепловые сети, сооружаемые в траншеях с креплением стенок'
  ];

  readonly appendixBCategories: string[] =
  [
    'Наружные трубопроводы',
    'Распределительная газовая сеть'
  ];

  readonly componentMaterialsAppendixA2And3: string[] = ['сборных железобетонных лотковых элементов'];

  readonly componentMaterailsAppendixB1: string[] =
  [
    'стальных труб в две нитки',
    'стальных труб в одну нитку',
    'полиэтиленовых труб в одну нитку'
  ];

  readonly defaultComponentMaterials: string[] =
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

  constructor() { }

  ngOnInit(): void {
  }

}
