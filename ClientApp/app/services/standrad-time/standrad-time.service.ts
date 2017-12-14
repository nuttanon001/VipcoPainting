import { Injectable, ViewContainerRef } from "@angular/core";
import { Http } from "@angular/http";
// rxjs
import { Observable } from "rxjs/Rx";
// models
import { StandradTime } from "../../models/model.index";
// base-service
import { BaseRestService, BaseCommunicateService } from "../base-service/base.index";

@Injectable()
export class StandradTimeService extends BaseRestService<StandradTime> {
    constructor(http: Http) { super(http, "api/StandradTime/"); }
}

@Injectable()
export class StandradTimeServiceCommunicate extends BaseCommunicateService<StandradTime> {
    constructor() { super(); }
}