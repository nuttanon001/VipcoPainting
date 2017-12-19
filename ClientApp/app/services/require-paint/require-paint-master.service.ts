import { Injectable, ViewContainerRef } from "@angular/core";
import { Http } from "@angular/http";
// rxjs
import { Observable } from "rxjs/Rx";
// models
import { RequirePaintMaster, RequirePaintMasterHasList } from "../../models/model.index";
// base-service
import { BaseRestService, BaseCommunicateService } from "../base-service/base.index";

@Injectable()
export class RequirePaintMasterService extends BaseRestService<RequirePaintMaster> {
    constructor(http: Http) { super(http, "api/RequirePaintingMaster/"); }
}

@Injectable()
export class RequirePaintMasterServiceCommunicate extends BaseCommunicateService<RequirePaintMasterHasList> {
    constructor() { super(); }
}