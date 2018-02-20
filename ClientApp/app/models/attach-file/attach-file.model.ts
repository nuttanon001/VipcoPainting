import { BaseModel } from "../base.model";

export interface AttachFile extends BaseModel {
    AttachFileId: number;
    FileName?: string;
    FileAddress?: string;
}