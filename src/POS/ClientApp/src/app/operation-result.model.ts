export interface IOperationResult {
  ok: boolean;
  result: any;
  metadata: IMetadata;
}

export interface IMetadata {
  message: string;
}
