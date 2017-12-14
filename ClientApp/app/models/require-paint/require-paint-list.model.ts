import { RequirePaintSub } from "./require-paint-sub.model";

export interface RequirePaintList {
    RequirePaintingListId: number;
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
    Creator?: string;
    CreateDate?: Date;
    Modifyer?: string;
    ModifyDate?: Date;
    // FK
    RequirePaintingMasterId?: number;
    RequirePaintingSubs?: Array<RequirePaintSub>;
}