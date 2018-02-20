import { Injectable, ViewContainerRef } from "@angular/core";
import { Http } from "@angular/http";
// rxjs
import { Observable } from "rxjs/Rx";
// models
import { RequirePaintMaster, RequirePaintMasterHasList, RequirePaintSchedule } from "../../models/model.index";
// base-service
import { BaseRestService, BaseCommunicateService } from "../base-service/base.index";
import { RequirePaintMasterHasInitial } from "../../models/require-paint/require-paint-master.model";

@Injectable()
export class RequirePaintMasterService extends BaseRestService<RequirePaintMaster> {
    constructor(http: Http) { super(http, "api/RequirePaintingMaster/"); }

    // get waiting require painting master
    getRequirePaintingMasterHasWait(): Observable<any> {
        let url: string = `${this.actionUrl}RequirePaintingMasterHasWait/`;
        return this.http.get(url).map(this.extractData).catch(this.handleError);
    }

    // GetMultipleKey
    // get require paint master with MultipleKey
    postGetMultipleKey(MutipleKey: Array<string>): Observable<Array<RequirePaintMaster>> {
        let url: string = `${this.actionUrl}GetMultipleKey/`;
        return this.http.post(url, MutipleKey).map(this.extractData).catch(this.handleError);
    }
    // CloseRequirePaintingMaster
    // try to close require paint
    getTryToCloseRequirePaintingMaster(RequirePainingMasterId: number): Observable<any> {
        let url: string = `${this.actionUrl}CloseRequirePaintingMaster/${RequirePainingMasterId}/`;
        return this.http.get(url).map(this.extractData).catch(this.handleError);
    }
    // ===================== Require Paint Master Schedule ===========================\\
    // get RequirePaint Schedule
    getRequirePaintSchedule(option: RequirePaintSchedule): Observable<any> {
        let url: string = `${this.actionUrl}RequirePaintSchedule/`;
        return this.http.post(url, JSON.stringify(option), this.getRequestOption())
            .map(this.extractData).catch(this.handleError);
    }

}

@Injectable()
export class RequirePaintMasterServiceCommunicate extends BaseCommunicateService<RequirePaintMasterHasList> {
    constructor() { super(); }
}

@Injectable()
export class RequirePaintMasterHasInitialServiceCommunicate extends BaseCommunicateService<RequirePaintMasterHasInitial> {
    constructor() { super(); }
}