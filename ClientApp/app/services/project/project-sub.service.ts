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

    // CanRemoveProjectSub
    // get CanRemove ProjectSub
    CanRemoveProjectSub(ProjectSubId: number, subAction: string = "CanRemoveProjectSub/"): Observable<any> {
        let url: string = `${this.actionUrl}${subAction}${ProjectSubId}/`;
        // console.log("Url", url);

        return this.http.get(url).map(this.extractData).catch(this.handleError);
    }
}

@Injectable()
export class ProjectSubServiceCommunicate extends BaseCommunicateService<ProjectSub> {
    constructor() { super(); }
}