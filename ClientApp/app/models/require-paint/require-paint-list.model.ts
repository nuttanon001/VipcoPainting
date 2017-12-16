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
    PlanStart?: Date;
    PlanEnd?: Date;
    Creator?: string;
    CreateDate?: Date;
    Modifyer?: string;
    ModifyDate?: Date;
    // FK
    RequirePaintingMasterId?: number;
}