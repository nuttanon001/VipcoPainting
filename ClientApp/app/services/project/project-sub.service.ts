import { Injectable, ViewContainerRef } from "@angular/core";
import { Http } from "@angular/http";
// rxjs
import { Observable } from "rxjs/Rx";
// models
import { ProjectSub } from "../../models/model.index";
// base-service
import { BaseRestService, BaseCommunicateService } from "../base-service/base.index";

@Injectable()
export class ProjectSubService extends BaseRestService<ProjectSub> {
    constructor(http: Http) { super(http, "api/ProjectCodeSub/"); }
}

@Injectable()
export class ProjectSubServiceCommunicate extends BaseCommunicateService<ProjectSub> {
    constructor() { super(); }
}