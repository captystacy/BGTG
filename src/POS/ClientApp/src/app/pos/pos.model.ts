export interface IProject {
  objectCipher: string;
  projectTemplate: number;
  projectEngineer: number;
  chiefProjectEngineer: number;
  householdTownIncluded: boolean;
}

export interface ITableOfContents {
  objectCipher: string;
  projectTemplate: number;
  chiefProjectEngineer: number;
}

export interface ITitlePage {
  objectCipher: string;
  objectName: string;
  chiefProjectEngineer: number;
}
