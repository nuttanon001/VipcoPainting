import { BaseModel } from "../base.model";
import { PaintWorkItem } from "../model.index";

export interface TaskPaintDetail extends BaseModel {
    TaskPaintDetailId: number;
    Remark?: string;
    //FK
    //TaskMaster
    TaskMasterId?: number;
    //PaintTeam
    PaintTeamId?: number;
    //PaintWorkItem
    PaintWorkItemId?: number;
    // ViewModel
    PaintTeamString?: string;
    PaintWorkItem?: PaintWorkItem;
}