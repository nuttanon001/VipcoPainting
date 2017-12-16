// angular
import { Component, Output, EventEmitter, Input } from "@angular/core";
// models
import { ProjectMaster,ProjectSub } from "../../../models/model.index";
// components
import { BaseViewComponent } from "../../base-component/base-view.component";
// services
import { ProjectSubService } from "../../../services/project/project-sub.service";

@Component({
    selector: "project-view",
    templateUrl: "./project-view.component.html",
    styleUrls: ["../../../styles/view.style.scss"],
})
/** project-view component*/
export class ProjectViewComponent extends BaseViewComponent<ProjectMaster> {
    /** project-view ctor */
    constructor(
        private service: ProjectSubService
    ) { super(); }

    // Parameter
    details: Array<ProjectSub> = new Array;
    columns = [
        { prop: "Code", name: "Code", flexGrow: 1 },
        { prop: "Name", name: "Name", flexGrow: 2 }
    ];

    // load more data
    onLoadMoreData(value: ProjectMaster) {
        this.service.getByMasterId(value.ProjectCodeMasterId)
            .subscribe(dbDetail => {
                this.details = dbDetail.slice();//[...dbDetail];
                // console.log("DataBase is :", this.details);
            }, error => console.error(error));
    }
}