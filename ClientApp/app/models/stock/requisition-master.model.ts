import { BaseModel } from "../base.model";

export interface RequisitionMaster extends BaseModel {
    RequisitionMasterId: number;
    RequisitionDate?: Date;
    RequisitionBy?: string;
    Quantity?: number;
    Remark?: string;
    //FK
    //ColorItem
    ColorItemId?: number;
    //PaintTaskDetail
    PaintTaskDetailId?: number;
    //ColorMovementStock
    ColorMovementStockId?: number;
    //ViewModel
    ColorNameString?: string;
    RequisitionByString?: string;
    // Index
    [key: string]: string | number | Date | undefined;
}