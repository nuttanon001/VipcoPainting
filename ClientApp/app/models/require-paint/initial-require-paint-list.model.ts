import { BaseModel } from "../base.model";
import { BlastWorkItem,PaintWorkItem } from "../model.index";

export interface InitialRequirePaint extends BaseModel {
    InitialRequireId: number;
    PlanStart?: Date;
    PlanEnd?: Date;
    DrawingNo?: string;
    UnitNo?: number;
    // FK
    RequirePaintingMasterId: number;
    // BlastWorkItem
    BlastWorkItems?: Array<BlastWorkItem>
    // PaintWorkItem
    PaintWorkItems?: Array<PaintWorkItem>
}