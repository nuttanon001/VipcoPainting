import { BaseModel } from "../base.model";

export interface FinishedGoodsMaster extends BaseModel {
    FinishedGoodsMasterId: number;
    FinishedGoodsDate?: Date;
    ExpiredDate?: Date;
    ReceiveBy?: string;
    Quantity?: number;
    LotNumber?: string;
    Remark?: string;
    //FK
    //ColorItem
    ColorItemId?: number;
    //ColorMovementStock
    ColorMovementStockId?: number;
    //ProjectCodeMaster
    ProjectCodeMasterId?: number;
    //ViewModel
    ColorNameString?: string;
    ReceiveByString?: string;
    ProjectMasterString?: string;
}