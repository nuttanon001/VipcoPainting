import { Injectable, ViewContainerRef } from "@angular/core";
import { Http } from "@angular/http";
// rxjs
import { Observable } from "rxjs/Rx";
// models
import { BlastWorkItem } from "../../models/model.index";
// base-service
import { BaseRestService } from "../base-service/base.index";

@Injectable()
export class BlastWorkitemService extends BaseRestService<BlastWorkItem> {
    constructor(http: Http) { super(http, "api/BlastWorkItem/"); }
}