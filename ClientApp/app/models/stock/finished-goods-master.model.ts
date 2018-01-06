import { BaseModel } from "../base.model";

export interface FinishedGoodsMaster extends BaseModel {
    FinishedGoodsMasterId: number;
    FinishedGoodsDate?: Date;
    ReceiveBy?: string;
    Quantity?: number;
    Remark?: string;
    //FK
    //ColorItem
    ColorItemId?: number;
    //ColorMovementStock
    ColorMovementStockId?: number;
    //ViewModel
    ColorNameString?: string;
    ReceiveByString?: string;
}