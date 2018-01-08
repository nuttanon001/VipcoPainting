import { BaseModel } from "../base.model";
import { BlastWorkItem, PaintWorkItem } from "../model.index";

export interface PaintTaskDetail extends BaseModel {
    PaintTaskDetailId: number;
    Remark?: string;
    PaintTaskDetailStatus?: number;
    PaintTaskDetailType?: number;
    PaintTaskDetailLayer?: number;
    PlanSDate: Date;
    PlanEDate: Date;
    ActualSDate?: Date;
    ActualEDate?: Date;
    TaskDetailProgress?: number;

    //FK
    //PaintTaskMaster
    PaintTaskMasterId?: number;
    //PaintTeam
    PaintTeamId?: number;
    //BlastRoom
    BlastRoomId?: number;
    //PaintWorkItem
    PaintWorkItemId?: number;
    //BlastWorkItem
    BlastWorkItemId?: number;
    // ViewModel
    PaintTeamString?:string;
    BlastRoomString?: string;
    BlastWorkItem?: BlastWorkItem;
    PaintWorkItem?: PaintWorkItem;
    isValid?: boolean;
    CommonText?: string;
}