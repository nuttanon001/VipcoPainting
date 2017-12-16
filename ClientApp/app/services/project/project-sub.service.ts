import { Injectable, ViewContainerRef } from "@angular/core";
import { Http } from "@angular/http";
// rxjs
import { Observable } from "rxjs/Rx";
// models
import { ProjectSub } from "../../models/model.index";
// base-service
import { BaseRestService, BaseCommunicateService } from "../base-service/base.index";

@Injectable()
export class ProjectSubService extends BaseRestService<ProjectSub> {
    constructor(http: Http) { super(http, "api/ProjectCodeSub/"); }

    // auto complate
    getAutoComplate(): Observable<Array<string>> {
        let url: string = `${this.actionUrl}GetAutoComplate/`;
        return this.http.get(url).map(this.extractData).catch(this.handleError);
    }
}

@Injectable()
export class ProjectSubServiceCommunicate extends BaseCommunicateService<ProjectSub> {
    constructor() { super(); }
}