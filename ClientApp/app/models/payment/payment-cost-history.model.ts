import { BaseModel } from "../base.model";

export interface PaymentCostHistory extends BaseModel {
    PaymentCostHistoryId: number;
    StartDate: Date;
    EndDate?: Date;
    PaymentCost: number;
    //FK
    //PaymentDetail
    PaymentDetailId: number;
}