import { Injectable, ViewContainerRef } from "@angular/core";
import { Http } from "@angular/http";
// rxjs
import { Observable } from "rxjs/Rx";
// models
import { SubPaymentMaster, SubPaymentDetail } from "../../models/model.index";
// base-service
import { BaseRestService, BaseCommunicateService } from "../base-service/base.index";

@Injectable()
export class SubpaymentMasterService extends BaseRestService<SubPaymentMaster> {
    constructor(http: Http) { super(http, "api/SubPaymentMaster/"); }

    //CalclateSubPaymentMaster
    postCalclateSubPaymentMaster(nObject: SubPaymentMaster): Observable<Array<SubPaymentDetail>> {
        return this.http.post(this.actionUrl + "CalclateSubPaymentMaster/", JSON.stringify(nObject), this.getRequestOption())
                    .map(this.extractData).catch(this.handleError);
    }
}

@Injectable()
export class SubpaymentMasterServiceCommunicate extends BaseCommunicateService<SubPaymentMaster> {
    constructor() { super(); }
}