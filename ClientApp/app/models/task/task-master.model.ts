import { BaseModel } from "../base.model";
import { TaskBlastDetail } from "./task-blast-detail.model";
import { TaskPaintDetail } from "./task-paint-detail.model";

export interface TaskMaster extends BaseModel {
    TaskMasterId: number;
    TaskNo?: string;
    AssignDate?: Date;
    AssignBy?: string;
    ActualSDate?: Date;
    ActualEDate?: Date;
    TaskProgress?: number;
    TaskStatus?: number;
    //FK
    // RequirePaintingList
    RequirePaintingListId?: number;
    // TaskBlastDetail
    TaskBlastDetails?: Array<TaskBlastDetail>;
    // TaslPaintDetail
    TaskPaintDetails?: Array<TaskPaintDetail>;
    //ViewModel
    AssignByString?:string;
    ProjectCodeSubString?:string;
}