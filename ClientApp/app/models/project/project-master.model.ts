import { ProjectSub } from "./project-sub.model";

export interface ProjectMaster {
    ProjectCodeMasterId: number;
    CreateDate?: Date;
    Creator?: string;
    EndDate?: Date;
    ModifyDate?: Date;
    Modifyer?: string;
    ProjectCode?: string;
    ProjectName?: string;
    StartDate?: Date;
    // ViewModel
    ProjectSubs?: Array<ProjectSub>;
}