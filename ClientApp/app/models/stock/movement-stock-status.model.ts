import { BaseModel } from "../base.model";

export interface MovementStockStatus extends BaseModel {
    MovementStockStatusId: number;
    StatusName?: string;
    StatusMovement?: number;
    TypeStatusMovement?: number;
}