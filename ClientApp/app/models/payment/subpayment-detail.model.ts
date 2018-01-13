import { BaseModel } from "../base.model";

export interface SubPaymentDetail extends BaseModel {
    SubPaymentDetailId: number;
    AreaWorkLoad?: number;
    CalcCost?: number;
    PaymentDate?: Date;
    Remark?: string;
    //FK
    //SubPaymentMaster
    SubPaymentMasterId?: number;
    //PaintTaskDetail
    PaintTaskDetailId?: number;
    //PaymentCostHistory
    PaymentCostHistoryId?: number;
    //ViewModel
    CurrentCost?: number;
    PaymentDetailString?: string;
}