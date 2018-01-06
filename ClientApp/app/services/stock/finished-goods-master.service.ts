import { Injectable, ViewContainerRef } from "@angular/core";
import { Http } from "@angular/http";
// rxjs
import { Observable } from "rxjs/Rx";
// models
import { FinishedGoodsMaster } from "../../models/model.index";
// base-service
import { BaseRestService, BaseCommunicateService } from "../base-service/base.index";

@Injectable()
export class FinishedGoodsMasterService extends BaseRestService<FinishedGoodsMaster> {
    constructor(http: Http) { super(http, "api/FinishedGoodsMaster/"); }
}

@Injectable()
export class FinishedGoodsMasterServiceCommunicate extends BaseCommunicateService<FinishedGoodsMaster> {
    constructor() { super(); }
}