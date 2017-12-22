import { Injectable, ViewContainerRef } from "@angular/core";
import { Http } from "@angular/http";
// rxjs
import { Observable } from "rxjs/Rx";
// models
import { TaskMaster } from "../../models/model.index";
// base-service
import { BaseRestService, BaseCommunicateService } from "../base-service/base.index";

@Injectable()
export class TaskMasterService extends BaseRestService<TaskMaster> {
    constructor(http: Http) { super(http, "api/TaskMaster/"); }
}

@Injectable()
export class TaskMasterServiceCommunicate extends BaseCommunicateService<TaskMaster> {
    constructor() { super(); }
}