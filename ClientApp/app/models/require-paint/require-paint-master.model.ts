import { RequirePaintList } from "./require-paint-list.model";

export interface RequirePaintMaster {
    RequirePaintingMasterId: number;
    ReceiveDate?: Date;
    RequireDate?: Date;
    FinishDate?: Date;
    PaintingSchedule?: string;
    RequireNo?: string;
    RequirePaintingStatus?: number;
    Creator?: string;
    CreateDate?: Date;
    Modifyer?: string;
    ModifyDate?: Date;

    //FK
    RequireEmp?: string;
    RequireString?: string;
    ReceiveEmp?: string;
    ReceiveString?: string;

    ProjectCodeSubId?: number;
    ProjectCodeSubString?: string;
}

export interface RequirePaintMasterHasList {
    RequirePaintMaster?: RequirePaintMaster;
    RequirePaintLists?: Array<RequirePaintList>;
}