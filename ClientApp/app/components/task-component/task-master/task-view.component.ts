// angular
import { Component, Output, EventEmitter, Input } from "@angular/core";
// models
import { TaskMaster } from "../../../models/model.index";
// components
import { BaseViewComponent } from "../../base-component/base-view.component";
@Component({
    selector: "task-view",
    templateUrl: "./task-view.component.html",
    styleUrls: ["../../../styles/view.style.scss"],
})
/** task-view component*/
export class TaskViewComponent extends BaseViewComponent<TaskMaster>{
    /** task-view ctor */
    constructor() {
        super();
    }
    // load more data
    onLoadMoreData(value: TaskMaster) { }
}