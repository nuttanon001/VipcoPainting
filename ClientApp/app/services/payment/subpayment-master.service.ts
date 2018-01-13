import { Injectable, ViewContainerRef } from "@angular/core";
import { Http } from "@angular/http";
// rxjs
import { Observable } from "rxjs/Rx";
// models
import { SubPaymentMaster } from "../../models/model.index";
// base-service
import { BaseRestService, BaseCommunicateService } from "../base-service/base.index";

@Injectable()
export class SubpaymentMasterService extends BaseRestService<SubPaymentMaster> {
    constructor(http: Http) { super(http, "api/SubPaymentMaster/"); }
}

@Injectable()
export class SubpaymentMasterServiceCommunicate extends BaseCommunicateService<SubPaymentMaster> {
    constructor() { super(); }
}