import { Injectable, ViewContainerRef } from "@angular/core";
import { Http } from "@angular/http";
// rxjs
import { Observable } from "rxjs/Rx";
// models
import { RequirePaintList } from "../../models/model.index";
// base-service
import { BaseRestService, BaseCommunicateService } from "../base-service/base.index";

@Injectable()
export class RequirePaintListService extends BaseRestService<RequirePaintList> {
    constructor(http: Http) { super(http, "api/RequirePaintingList/"); }
}

@Injectable()
export class RequirePaintListServiceCommunicate extends BaseCommunicateService<RequirePaintList> {
    constructor() { super(); }
}