import { Injectable, ViewContainerRef } from "@angular/core";
import { Http } from "@angular/http";
// rxjs
import { Observable } from "rxjs/Rx";
// models
import { PaintTeam } from "../../models/model.index";
// base-service
import { BaseRestService, BaseCommunicateService } from "../base-service/base.index";

@Injectable()
export class PaintTeamService extends BaseRestService<PaintTeam> {
    constructor(http: Http) { super(http, "api/PaintTeam/"); }
}

@Injectable()
export class PaintTeamServiceCommunicate extends BaseCommunicateService<PaintTeam> {
    constructor() { super(); }
}