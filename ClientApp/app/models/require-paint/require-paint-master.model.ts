import { RequirePaintList } from "./require-paint-list.model";
import { BaseModel } from "../base.model";

export interface RequirePaintMaster extends BaseModel {
    RequirePaintingMasterId: number;
    ReceiveDate?: Date;
    RequireDate?: Date;
    FinishDate?: Date;
    PaintingSchedule?: string;
    RequireNo?: string;
    RequirePaintingStatus?: number;

    //FK
    RequireEmp?: string;
    ReceiveEmp?: string;
    ProjectCodeSubId?: number;
    // ViewModel
    RequireString?: string;
    ReceiveString?: string;
    ProjectCodeSubString?: string;
}

export interface RequirePaintMasterHasList {
    RequirePaintMaster?: RequirePaintMaster;
    RequirePaintLists?: Array<RequirePaintList>;
}