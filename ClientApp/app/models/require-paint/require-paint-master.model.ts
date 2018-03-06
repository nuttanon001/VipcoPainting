import { RequirePaintList } from "./require-paint-list.model";
import { BaseModel } from "../base.model";
import { InitialRequirePaint } from "../model.index";

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
    // ATTACH
    AttachFile?: FileList;
    RemoveAttach?: Array<number>;
}

export interface RequirePaintMasterHasList {
    RequirePaintMaster?: RequirePaintMaster;
    RequirePaintLists?: Array<RequirePaintList>;
}

export interface RequirePaintMasterHasInitial {
    RequirePaintMaster?: RequirePaintMaster;
    InitialRequirePaint?: InitialRequirePaint;
}