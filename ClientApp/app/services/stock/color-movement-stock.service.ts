import { Injectable, ViewContainerRef } from "@angular/core";
import { Http } from "@angular/http";
// rxjs
import { Observable } from "rxjs/Rx";
// models
import { ColorMovementStock } from "../../models/model.index";
// base-service
import { BaseRestService, BaseCommunicateService } from "../base-service/base.index";

@Injectable()
export class ColorMovementStockService extends BaseRestService<ColorMovementStock> {
    constructor(http: Http) { super(http, "api/ColorMovementStock/"); }
}

@Injectable()
export class ColorMovementStockServiceCommunicate extends BaseCommunicateService<ColorMovementStock> {
    constructor() { super(); }
}