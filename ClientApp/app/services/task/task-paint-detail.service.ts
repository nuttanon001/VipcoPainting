import { Injectable, ViewContainerRef } from "@angular/core";
import { Http } from "@angular/http";
// rxjs
import { Observable } from "rxjs/Rx";
// models
import { TaskPaintDetail } from "../../models/model.index";
// base-service
import { BaseRestService, BaseCommunicateService } from "../base-service/base.index";

@Injectable()
export class TaskPaintDetailService extends BaseRestService<TaskPaintDetail> {
    constructor(http: Http) { super(http, "api/TaskPaintDetail/"); }
}

@Injectable()
export class TaskPaintDetailServiceCommunicate extends BaseCommunicateService<TaskPaintDetail> {
    constructor() { super(); }
}