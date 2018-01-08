﻿import { Injectable, ViewContainerRef } from "@angular/core";
import { Http } from "@angular/http";
// rxjs
import { Observable } from "rxjs/Rx";
// models
import { PaintTaskDetail } from "../../models/model.index";
// base-service
import { BaseRestService, BaseCommunicateService } from "../base-service/base.index";

@Injectable()
export class PaintTaskDetailService extends BaseRestService<PaintTaskDetail> {
    constructor(http: Http) { super(http, "api/PaintTaskDetail/"); }

    // get one with key number
    getOneKeyNumberWithCustom(key: number): Observable<PaintTaskDetail> {
        return this.http.get(this.actionUrl + "WithCustom/" + key + "/")
            .map(this.extractData).catch(this.handleError);
    }
}

@Injectable()
export class PaintTaskDetailServiceCommunicate extends BaseCommunicateService<PaintTaskDetail> {
    constructor() { super(); }
}