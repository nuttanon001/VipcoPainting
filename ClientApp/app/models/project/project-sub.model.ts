export interface ProjectSub {
    ProjectCodeSubId: number;
    Code?: string;
    Name?: string;
    ProjectSubStatus?: number;
    Creator?: string;
    CreateDate?: Date;
    Modifyer?: string;
    ModifyDate?: Date;
    // FK
    ProjectSubParentId?: number;
    ProjectCodeMasterId?: number;
    // ViewModel
    ProjectMasterString?: string;
}