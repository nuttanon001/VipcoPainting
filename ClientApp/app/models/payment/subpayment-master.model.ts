import { BaseModel } from "../base.model";
import { SubPaymentDetail } from "./subpayment-detail.model";

export interface SubPaymentMaster extends BaseModel {
    SubPaymentMasterId: number;
    SubPaymentNo?:string;
    SubPaymentDate ?: Date;
    StartDate?: Date;
    EndDate?: Date;
    SubPaymentMasterStatus ?: number;
    Remark?:string;
    EmpApproved1?:string;
    EmpApproved2?:string;
    //FK
    //SubPaymentMaster
    PrecedingSubPaymentId?: number;
    //ProjectMaster
    ProjectCodeMasterId ?: number;
    //PaintTeam
    PaintTeamId ?: number;
    //SubPaymentDetal
    SubPaymentDetails ?: Array<SubPaymentDetail>;
    //ViewModel
    EmpApproved1String?:string;
    EmpApproved2String?:string;
    ProjectCodeMasterString?:string;
    PaintTeamString?:string;
    SubPaymentMasterStatusString?:string;
}