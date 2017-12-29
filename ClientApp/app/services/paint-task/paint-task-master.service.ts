import { Injectable, ViewContainerRef } from "@angular/core";
import { Http } from "@angular/http";
// rxjs
import { Observable } from "rxjs/Rx";
// models
import { PaintTaskMaster, OptionTaskMasterSchedule } from "../../models/model.index";
// base-service
import { BaseRestService, BaseCommunicateService } from "../base-service/base.index";

@Injectable()
export class PaintTaskMasterService extends BaseRestService<PaintTaskMaster> {
    constructor(http: Http) { super(http, "api/PaintTaskMaster/"); }
    // ===================== TaskMaster Schedule ===========================\\
    // get TaskMachine Schedule
    getTaskMasterSchedule(option: OptionTaskMasterSchedule): Observable<any> {
        let url: string = `${this.actionUrl}TaskMasterSchedule/`;
        return this.http.post(url, JSON.stringify(option), this.getRequestOption())
            .map(this.extractData).catch(this.handleError);
    }
}

@Injectable()
export class PaintTaskMasterServiceCommunicate extends BaseCommunicateService<PaintTaskMaster> {
    constructor() { super(); }
}