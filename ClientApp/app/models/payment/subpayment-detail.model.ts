import { BaseModel } from "../base.model";

export interface SubPaymentDetail extends BaseModel {
    SubPaymentDetailId: number;
    AreaWorkLoad?: number;
    AdditionArea?: number;
    CalcCost?: number;
    AdditionCost?: number;
    PaymentDate?: Date;
    Remark?: string;
    //FK
    //SubPaymentMaster
    SubPaymentMasterId?: number;
    //PaymentDetail
    PaymentDetailId?: number;
    //ViewModel
    CurrentCost?: number;
    PaymentDetailString?: string;
}