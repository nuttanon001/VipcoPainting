import { BaseModel } from "../base.model";

export interface BlastRoom extends BaseModel {
    BlastRoomId: number;
    BlastRoomName?: string;
    BlastRoomNumber?: number;
    Remark?: string;
    //FK
    PaintTeamId?: number;
    //ViewModel
    TeamBlastString?: string;
}