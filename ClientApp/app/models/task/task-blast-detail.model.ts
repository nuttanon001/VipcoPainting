import { BaseModel } from "../base.model";
import { BlastWorkItem } from "../model.index";

export interface TaskBlastDetail extends BaseModel {
    TaskBlastDetailId: number;
    Remark?: string;
    //FK
    //TaskMaster
    TaskMasterId?: number;
    //BlastRoom
    BlastRoomId?: number;
    //BlastWorkItem
    BlastWorkItemId?: number;
    //ViewModel
    BlastRoomString?: string;
    BlastWorkItem?: BlastWorkItem;
}