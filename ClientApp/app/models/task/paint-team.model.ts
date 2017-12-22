import { BaseModel } from "../base.model";

export interface PaintTeam extends BaseModel {
    PaintTeamId: number;
    TeamName?: string;
    Ramark?: string;
}