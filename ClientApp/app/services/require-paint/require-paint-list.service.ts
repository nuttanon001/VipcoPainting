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

    // post with list
    postLists(nObject: Array<RequirePaintList>): Observable<any> {
        return this.http.post(this.actionUrl +"Lists/", JSON.stringify(nObject), this.getRequestOption())
            .map(this.extractData).catch(this.handleError);
    }

    // put with list
    putLists(uObject: Array<RequirePaintList>): Observable<any> {
        return this.http.put(this.actionUrl + "Lists/", JSON.stringify(uObject), this.getRequestOption())
            .map(this.extractData).catch(this.handleError);
    }
}

@Injectable()
export class RequirePaintListServiceCommunicate extends BaseCommunicateService<RequirePaintList> {
    constructor() { super(); }
}