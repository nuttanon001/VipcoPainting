﻿import { Injectable, ViewContainerRef } from "@angular/core";
import { Http } from "@angular/http";
// rxjs
import { Observable } from "rxjs/Rx";
// models
import { RequirePaintList, RequirePaintSchedule, AttachFile } from "../../models/model.index";
// base-service
import { BaseRestService, BaseCommunicateService } from "../base-service/base.index";

@Injectable()
export class RequirePaintListService extends BaseRestService<RequirePaintList> {
    constructor(http: Http) { super(http, "api/RequirePaintingList/"); }

    // post with initial require
    postWithInitialRequire(newRequirePaintList: RequirePaintList,subAction:string = "PostWithInitialRequire/"): Observable<RequirePaintList> {
        return this.http.post(this.actionUrl + subAction, JSON.stringify(newRequirePaintList), this.getRequestOption())
            .map(this.extractData).catch(this.handleError);
    }

    // post with RequirePaintList
    postV2(nObject: RequirePaintList, SubAction: string = "PostV2/"): Observable<RequirePaintList> {
        return this.http.post(this.actionUrl + SubAction, JSON.stringify(nObject), this.getRequestOption())
            .map(this.extractData).catch(this.handleError);
    }

    // post with list
    postLists2(nObject: Array<RequirePaintList>): Observable<any> {
        return this.http.post(this.actionUrl +"Lists2/", JSON.stringify(nObject), this.getRequestOption())
            .map(this.extractData).catch(this.handleError);
    }

    // SetReceiveWorkItem
    setReceiveWorkItem(key: number, create: string, subAction: string = "SetReceiveWorkItem/"): Observable<any> {
        let url: string = `${this.actionUrl}${subAction}${key}/${create}/`;
        return this.http.get(url)
            .map(this.extractData).catch(this.handleError);
    }

    // put with key number v2
    putKeyNumberV2(uObject: RequirePaintList, key: number, SubAction: string = "PutV2/"): Observable<RequirePaintList> {
        //console.log(uObject);
        return this.http.put(this.actionUrl + SubAction + key + "/", JSON.stringify(uObject), this.getRequestOption())
            .map(this.extractData).catch(this.handleError);
    }

    // put with key number v3
    putKeyNumberV3(uObject: RequirePaintList, key: number, SubAction: string = "PutV3/"): Promise<RequirePaintList> {
        //console.log(uObject);
        return this.http.put(this.actionUrl + SubAction + key + "/", JSON.stringify(uObject), this.getRequestOption())
            .toPromise()
            .then(this.extractData).catch(this.handleError);
    }


    // GetV2
    // get one with key number
    getOneKeyNumberV2(key: number): Observable<RequirePaintList> {
        return this.http.get(this.actionUrl + "GetV2/" + key + "/")
            .map(this.extractData).catch(this.handleError);
    }


    // ===================== Require Paint Master Schedule ===========================\\
    // get Require Paint Schedule
    getRequirePaintSchedule(option: RequirePaintSchedule): Observable<any> {
        let url: string = `${this.actionUrl}RequirePaintSchedule/`;
        return this.http.post(url, JSON.stringify(option), this.getRequestOption())
            .map(this.extractData).catch(this.handleError);
    }

    // put with list
    putLists(uObject: Array<RequirePaintList>): Observable<any> {
        return this.http.put(this.actionUrl + "Lists/", JSON.stringify(uObject), this.getRequestOption())
            .map(this.extractData).catch(this.handleError);
    }

    // ===================== Upload File ===============================\\
    // get file
    getAttachFile(JobCardMasterId: number): Observable<Array<AttachFile>> {
        let url: string = `${this.actionUrl}GetAttach/${JobCardMasterId}/`;
        return this.http.get(url)
            .map(this.extractData).catch(this.handleError);
    }


    // upload file
    postAttactFile(RequirePaintListId: number, files: FileList,CreateBy:string): Observable<any> {
        let input: any = new FormData();

        for (let i: number = 0; i < files.length; i++) {
            if (files[i].size <= 5242880) {
                input.append("files", files[i]);
            }
        }

        // console.log("Files : ", input);

        let url: string = `${this.actionUrl}PostAttach/${RequirePaintListId}/${CreateBy}`;
        return this.http.post(url, input).map(this.extractData).catch(this.handleError);
    }

    // delete file
    deleteAttactFile(AttachId: number): Observable<any> {
        let url: string = this.actionUrl + "DeleteAttach/" + AttachId;
        return this.http.delete(url).catch(this.handleError);
    }

    // ===================== End Upload File ===========================\\
}

@Injectable()
export class RequirePaintListServiceCommunicate extends BaseCommunicateService<RequirePaintList> {
    constructor() { super(); }
}