export interface IProject {
  objectCipher: string;
  projectTemplate: number;
  projectEngineer: number;
  normalInspectionEngineer: number;
  chiefEngineer: number;
  chiefProjectEngineer: number;
}

export interface ITableOfContents {
  objectCipher: string;
  projectTemplate: number;
  normalInspectionEngineer: number;
  chiefProjectEngineer: number;
}

export interface ITitlePage {
  objectCipher: string;
  objectName: string;
  chiefProjectEngineer: number;
}
