export interface StandradTime {
    StandradTimeId: number;
    Code?: string;
    Description?: string;
    Rate?: number;
    RateUnit?: string;
    PercentLoss?: number;
    TypeStandardTime?: number;
    AreaCodition?: number;
    Codition?: number;
    LinkStandardTimeId?: number;
    Creator?: string;
    CreateDate?: Date;
    Modifyer?: string;
    ModifyDate?: Date;
    // ViewModel
    RateWithUnit?: string;
    PercentLossString?: string;
    TypeStandardTimeString?: string;
    AreaWithUnitNameString?: string;
    ConditionString?: string;
    LinkStandardTimeString?: string;
}