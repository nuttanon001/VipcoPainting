import { BlastWorkItem } from "./blast-workitem.model";
import { PaintWorkItem } from "./paint-workitem.model";
import { BaseModel } from "../base.model";

export interface RequirePaintList extends BaseModel {
    RequirePaintingListId: number;
    RequirePaintingListStatus?: number;
    Description?: string;
    PartNo?: string;
    MarkNo?: string;
    DrawingNo?: string;
    UnitNo?: number;
    Quantity?: number;
    FieldWeld?: boolean;
    Insulation?: boolean;
    ITP?: boolean;
    SizeL?: number;
    SizeW?: number;
    SizeH?: number;
    Weight?: number;
    PlanStart?: Date;
    PlanEnd?: Date;
    // FK
    RequirePaintingMasterId?: number;
    BlastWorkItems?: Array<BlastWorkItem>;
    PaintWorkItems?: Array<PaintWorkItem>;
}