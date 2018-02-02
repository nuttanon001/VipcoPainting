export interface BlastWorkItem {
    BlastWorkItemId: number;
    IntArea?: number;
    IntCalcStdUsage?: number;
    ExtArea?: number;
    ExtCalcStdUsage?: number;
    Creator?: string;
    CreateDate?: Date;
    Modifyer?: string;
    ModifyDate?: Date;
    //FK
    // StandradTimeInt
    StandradTimeIntId?: number;
    // StandradTimeInt
    StandradTimeExtId?: number;
    // SurfaceTypeInt
    SurfaceTypeIntId?: number;
    // SurfaceTypeExt
    SurfaceTypeExtId?: number;
    // RequirePaintingList
    RequirePaintingListId?: number;
    // InitialRequireId
    InitialRequireId?: number;
    // ViewModel
    IntStandradTimeString?: string;
    ExtStandradTimeString?: string;
    IntSurfaceTypeString?: string;
    ExtSurfaceTypeString?: string;
    IsValid?: boolean;
}