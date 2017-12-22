import { Injectable, ViewContainerRef } from "@angular/core";
import { Http } from "@angular/http";
// rxjs
import { Observable } from "rxjs/Rx";
// models
import { BlastRoom } from "../../models/model.index";
// base-service
import { BaseRestService, BaseCommunicateService } from "../base-service/base.index";
@Injectable()
export class BlastRoomService extends BaseRestService<BlastRoom> {
    constructor(http: Http) { super(http, "api/BlastRoom/"); }
}

@Injectable()
export class BlastRoomServiceCommunicate extends BaseCommunicateService<BlastRoom> {
    constructor() { super(); }
}