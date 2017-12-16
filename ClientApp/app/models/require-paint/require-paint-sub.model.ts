export interface RequirePaintSub {
    RequirePaintingSubId: number;
    PaintingArea?: number;
    PaintingType?: number;
    Area?: number;
    DFTMin?: number;
    DFTMax?: number;
    Creator?: string;
    CreateDate?: Date;
    Modifyer?: string;
    ModifyDate?: Date;
    CalcColorUsage?: number;
    CalcStdUsage?: number;
    //FK
    // ColorItem
    ColorItemId?: number;
    // SurfaceType
    SurfaceTypeId?: number;
    // StandradTime
    StandradTimeId?: number;
    // RequirePaintingList
    RequirePaintingListId?: number;
}