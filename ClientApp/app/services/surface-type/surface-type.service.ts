import { Injectable, ViewContainerRef } from "@angular/core";
import { Http } from "@angular/http";
// rxjs
import { Observable } from "rxjs/Rx";
// models
import { SurfaceType } from "../../models/model.index";
// base-service
import { BaseRestService, BaseCommunicateService } from "../base-service/base.index";

@Injectable()
export class SurfaceTypeService extends BaseRestService<SurfaceType> {
    constructor(http: Http) { super(http, "api/SurfaceType/"); }
}

@Injectable()
export class SurfaceTypeServiceCommunicate extends BaseCommunicateService<SurfaceType> {
    constructor() { super(); }
}