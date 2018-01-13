import { Injectable, ViewContainerRef } from "@angular/core";
import { Http } from "@angular/http";
// rxjs
import { Observable } from "rxjs/Rx";
// models
import { SubPaymentDetail } from "../../models/model.index";
// base-service
import { BaseRestService, BaseCommunicateService } from "../base-service/base.index";

@Injectable()
export class SubpaymentDetailService extends BaseRestService<SubPaymentDetail> {
    constructor(http: Http) { super(http, "api/SubPaymentDetail/"); }
}

@Injectable()
export class SubpaymentDetailServiceCommunicate extends BaseCommunicateService<SubPaymentDetail> {
    constructor() { super(); }
}