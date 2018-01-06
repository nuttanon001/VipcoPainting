import { Injectable, ViewContainerRef } from "@angular/core";
import { Http } from "@angular/http";
// rxjs
import { Observable } from "rxjs/Rx";
// models
import { RequisitionMaster } from "../../models/model.index";
// base-service
import { BaseRestService, BaseCommunicateService } from "../base-service/base.index";

@Injectable()
export class RequisitionMasterService extends BaseRestService<RequisitionMaster> {
    constructor(http: Http) { super(http, "api/RequisitionMaster/"); }
}

@Injectable()
export class RequisitionMasterServiceCommunicate extends BaseCommunicateService<RequisitionMaster> {
    constructor() { super(); }
}