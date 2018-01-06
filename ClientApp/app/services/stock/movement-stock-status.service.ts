import { Injectable, ViewContainerRef } from "@angular/core";
import { Http } from "@angular/http";
// rxjs
import { Observable } from "rxjs/Rx";
// models
import { MovementStockStatus } from "../../models/model.index";
// base-service
import { BaseRestService, BaseCommunicateService } from "../base-service/base.index";

@Injectable()
export class MovementStockStatusService extends BaseRestService<MovementStockStatus> {
    constructor(http: Http) { super(http, "api/MovementStockStatus/"); }
}

@Injectable()
export class MovementStockStatusServiceCommunicate extends BaseCommunicateService<MovementStockStatus> {
    constructor() { super(); }
}