export interface PaintWorkItem {
    PaintWorkItemId: number;
    PaintLevel?: number;
    IntArea?: number;
    IntDFTMin?: number;
    IntDFTMax?: number;
    IntCalcColorUsage?: number;
    IntCalcStdUsage?: number;

    ExtArea?: number;
    ExtDFTMin?: number;
    ExtDFTMax?: number;
    ExtCalcColorUsage?: number;
    ExtCalcStdUsage?: number;
    Creator?: string;
    CreateDate?: Date;
    Modifyer?: string;
    ModifyDate?: Date;
    //FK
    // ColorItemInt
    IntColorItemId?: number;
    // ColorItemExt
    ExtColorItemId?: number;
    // StandradTimeInt
    StandradTimeIntId?: number;
    // StandradTimeExt
    StandradTimeExtId?: number;
    // RequirePaintingList
    RequirePaintingListId?: number;
    // ViewModel
    IntColorString?: string;
    ExtColorString?: string;
    IntStandradTimeString?: string;
    ExtStandradTimeString?: string;
    PaintLevelString?: string;
    Use?: boolean;
}