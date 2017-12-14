export interface RequirePaintMaster {
    RequirePaintingMasterId: number;
    ReceiveDate?: Date;
    RequireDate?: Date;
    FinishDate?: Date;
    PaintingSchedule?: string;
    RequireNo?: string;
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