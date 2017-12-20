import { Injectable, ViewContainerRef } from "@angular/core";
import { Http } from "@angular/http";
// rxjs
import { Observable } from "rxjs/Rx";
// models
import { PaintWorkItem } from "../../models/model.index";
// base-service
import { BaseRestService } from "../base-service/base.index";

@Injectable()
export class PaintWorkitemService extends BaseRestService<PaintWorkItem> {
    constructor(http: Http) { super(http, "api/PaintWorkItem/"); }
}