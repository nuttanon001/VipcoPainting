import { BaseModel } from "../base.model";
import { PaintTaskDetail } from "./paint-task-detail.model";

export interface PaintTaskMaster extends BaseModel {
    PaintTaskMasterId: number;
    TaskPaintNo?: string;
    AssignDate?: Date;
    AssignBy?: string;
    MainProgress?: number;
    PaintTaskStatus: number;
    //FK
    // RequirePaintingList
    RequirePaintingListId?: number;
    // PaintTaskDetail
    PaintTaskDetails?: Array<PaintTaskDetail>;
    //ViewModel
    AssignByString?: string;
    ProjectCodeSubString?: string;
}