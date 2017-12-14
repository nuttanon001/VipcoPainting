import { Injectable, ViewContainerRef } from "@angular/core";
import { Http } from "@angular/http";
// rxjs
import { Observable } from "rxjs/Rx";
// models
import { RequirePaintSub } from "../../models/model.index";
// base-service
import { BaseRestService, BaseCommunicateService } from "../base-service/base.index";

@Injectable()
export class RequirePaintSubService extends BaseRestService<RequirePaintSub> {
    constructor(http: Http) { super(http, "api/RequirePaintingSub/"); }
}

@Injectable()
export class RequirePaintSubServiceCommunicate extends BaseCommunicateService<RequirePaintSub> {
    constructor() { super(); }
}