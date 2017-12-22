import { Injectable, ViewContainerRef } from "@angular/core";
import { Http } from "@angular/http";
// rxjs
import { Observable } from "rxjs/Rx";
// models
import { TaskBlastDetail } from "../../models/model.index";
// base-service
import { BaseRestService, BaseCommunicateService } from "../base-service/base.index";
@Injectable()
export class TaskBlastDetailService extends BaseRestService<TaskBlastDetail> {
    constructor(http: Http) { super(http, "api/TaskBlastDetail/"); }
}

@Injectable()
export class TaskBlastDetailServiceCommunicate extends BaseCommunicateService<TaskBlastDetail> {
    constructor() { super(); }
}