import { Injectable, ViewContainerRef } from "@angular/core";
import { Http } from "@angular/http";
// rxjs
import { Observable } from "rxjs/Rx";
// models
import { PaymentDetail } from "../../models/model.index";
// base-service
import { BaseRestService, BaseCommunicateService } from "../base-service/base.index";

@Injectable()
export class PaymentDetailService extends BaseRestService<PaymentDetail> {
    constructor(http: Http) { super(http, "api/PaymentDetail/"); }
}

@Injectable()
export class PaymentDetailServiceCommunicate extends BaseCommunicateService<PaymentDetail> {
    constructor() { super(); }
}