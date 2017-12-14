import { Injectable, ViewContainerRef } from "@angular/core";
import { Http } from "@angular/http";
// rxjs
import { Observable } from "rxjs/Rx";
// models
import { Color } from "../../models/model.index";
// base-service
import { BaseRestService, BaseCommunicateService } from "../base-service/base.index";

@Injectable()
export class ColorService extends BaseRestService<Color> {
    constructor(http: Http) { super(http, "api/ColorItem/"); }
}

@Injectable()
export class ColorServiceCommunicate extends BaseCommunicateService<Color> {
    constructor() { super(); }
}