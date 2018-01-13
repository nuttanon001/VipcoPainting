import { BaseModel } from "../base.model";

export interface PaymentDetail extends BaseModel {
    PaymentDetailId: number;
    Description?: string;
    LastCost: number;
    PaymentType: number;
    //ViewModel
    PaymentTypeString?: string;
}