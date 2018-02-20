import { Injectable, ViewContainerRef } from "@angular/core";
import { Http } from "@angular/http";
// rxjs
import { Observable } from "rxjs/Rx";
// models
import { InitialRequirePaint, RequirePaintSchedule } from "../../models/model.index";
// base-service
import { BaseRestService, BaseCommunicateService } from "../base-service/base.index";

@Injectable()
export class InitialRequirePaintService extends BaseRestService<InitialRequirePaint> {
    constructor(http: Http) { super(http, "api/InitialRequirePainting/"); }

    // ===================== Initial Require Paint Schedule ===========================\\
    // get Initial Require Paint Schedule
    getInitialRequirePaintSchedule(option: RequirePaintSchedule): Observable<any> {
        let url: string = `${this.actionUrl}InitialRequirePaintSchedule/`;
        return this.http.post(url, JSON.stringify(option), this.getRequestOption())
            .map(this.extractData).catch(this.handleError);
    }
}

@Injectable()
export class InitialRequirePaintServiceCommunicate extends BaseCommunicateService<InitialRequirePaint> {
    constructor() { super(); }
}