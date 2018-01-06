import { BaseModel } from "../base.model";

export interface ColorMovementStock extends BaseModel {
    ColortMovementStockId: number;
    MovementStockDate?: Date;
    Quantity?: number;
    // FK
    ColorItemId?: number;
    MovementStockStatusId?: number;
    // ViewModel
    StatusNameString?: string;
    ColorNameString?: string;
}