import { Injectable,ViewContainerRef } from "@angular/core";
import { Http } from "@angular/http";
import { MatDialogRef, MatDialog, MatDialogConfig } from "@angular/material";
// rxjs
import { Observable } from "rxjs/Rx";
// models
import { Employee } from "../../models/model.index";
// base-service
import { BaseRestService, BaseCommunicateService } from "../base-service/base.index";

@Injectable()
export class EmployeeService extends BaseRestService<Employee> {
    constructor(
        http: Http,
        private dialog: MatDialog
    ) { super(http, "api/Employee/"); }
}

@Injectable()
export class EmployeeServiceCommunicate extends BaseCommunicateService<Employee> {
    constructor() {
        super();
    }
}