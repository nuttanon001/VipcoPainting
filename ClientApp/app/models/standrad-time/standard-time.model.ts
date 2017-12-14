export interface StandradTime {
    StandradTimeId: number;
    Code?: string;
    Description?: string;
    Rate?: number;
    RateUnit?: string;
    PercentLoss?: number;

    Creator?: string;
    CreateDate?: Date;
    Modifyer?: string;
    ModifyDate?: Date;
}