export interface RequirePaintSub {
    RequirePaintingSubId: number;
    PaintingArea?: number;
    PaintingType?: number;
    Area?: number;
    DFTMin?: number;
    DFTMax?: number;
    Creator?: string;
    CreateDate?: number;
    Modifyer?: string;
    ModifyDate?: number;
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