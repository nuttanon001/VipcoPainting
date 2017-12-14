import { Injectable, ViewContainerRef } from "@angular/core";
import { Http } from "@angular/http";
// rxjs
import { Observable } from "rxjs/Rx";
// models
import { ProjectMaster } from "../../models/model.index";
// base-service
import { BaseRestService, BaseCommunicateService } from "../base-service/base.index";

@Injectable()
export class ProjectMasterService extends BaseRestService<ProjectMaster> {
    constructor(http: Http) { super(http, "api/ProjectCodeMaster/"); }
}

@Injectable()
export class ProjectMasterServiceCommunicate extends BaseCommunicateService<ProjectMaster> {
    constructor() { super(); }
}