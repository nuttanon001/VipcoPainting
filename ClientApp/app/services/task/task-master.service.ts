import { Injectable, ViewContainerRef } from "@angular/core";
import { Http } from "@angular/http";
// rxjs
import { Observable } from "rxjs/Rx";
// models
import { TaskMaster, OptionTaskMasterSchedule } from "../../models/model.index";
// base-service
import { BaseRestService, BaseCommunicateService } from "../base-service/base.index";

@Injectable()
export class TaskMasterService extends BaseRestService<TaskMaster> {
    constructor(http: Http) { super(http, "api/TaskMaster/"); }
    // ===================== TaskMaster Schedule ===========================\\
    // get TaskMachine WaitAndProcess
    getTaskMasterSchedule(option: OptionTaskMasterSchedule): Observable<any> {
        let url: string = `${this.actionUrl}TaskMasterSchedule/`;
        return this.http.post(url, JSON.stringify(option), this.getRequestOption())
            .map(this.extractData).catch(this.handleError);
    }

    // get TaskMachine WaitAndProcess V2
    getTaskMasterScheduleV2(option: OptionTaskMasterSchedule): Observable<any> {
        let url: string = `${this.actionUrl}TaskMasterScheduleV2/`;
        return this.http.post(url, JSON.stringify(option), this.getRequestOption())
            .map(this.extractData).catch(this.handleError);
    }
}

@Injectable()
export class TaskMasterServiceCommunicate extends BaseCommunicateService<TaskMaster> {
    constructor() { super(); }
}