import { Injectable, ViewContainerRef } from "@angular/core";
import { Http } from "@angular/http";
// rxjs
import { Observable } from "rxjs/Rx";
// models
import { InitialRequirePaint } from "../../models/model.index";
// base-service
import { BaseRestService, BaseCommunicateService } from "../base-service/base.index";

@Injectable()
export class InitialRequirePaintService extends BaseRestService<InitialRequirePaint> {
    constructor(http: Http) { super(http, "api/InitialRequirePainting/"); }
}

@Injectable()
export class InitialRequirePaintServiceCommunicate extends BaseCommunicateService<InitialRequirePaint> {
    constructor() { super(); }
}