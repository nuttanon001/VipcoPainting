export interface StandradTime {
    StandradTimeId: number;
    Code?: string;
    Description?: string;
    Rate?: number;
    RateUnit?: string;
    PercentLoss?: number;
    TypeStandardTime?: number;

    Creator?: string;
    CreateDate?: Date;
    Modifyer?: string;
    ModifyDate?: Date;
    // ViewModel
    RateWithUnit?: string;
    PercentLossString?: string;
    TypeStandardTimeString?: string;
}