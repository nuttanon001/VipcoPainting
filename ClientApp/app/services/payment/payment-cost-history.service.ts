import { Injectable, ViewContainerRef } from "@angular/core";
import { Http } from "@angular/http";
// rxjs
import { Observable } from "rxjs/Rx";
// models
import { PaymentCostHistory } from "../../models/model.index";
// base-service
import { BaseRestService, BaseCommunicateService } from "../base-service/base.index";

@Injectable()
export class PaymentCostHistoryService extends BaseRestService<PaymentCostHistory> {
    constructor(http: Http) { super(http, "api/PaymentCostHistory/"); }
}

@Injectable()
export class PaymentCostHistoryServiceCommunicate extends BaseCommunicateService<PaymentCostHistory> {
    constructor() { super(); }
}