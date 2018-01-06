// angular
import { Component, Output, EventEmitter, Input } from "@angular/core";
// models
import { RequisitionMaster } from "../../../models/model.index";
// components
import { BaseViewComponent } from "../../base-component/base-view.component";

@Component({
    selector: "requisition-view",
    templateUrl: "./requisition-view.component.html",
    styleUrls: ["../../../styles/view.style.scss"],
})
/** requisition-view component*/
export class RequisitionViewComponent extends BaseViewComponent<RequisitionMaster> {
    /** requisition-view ctor */
    constructor() {
        super();
    }
    // load more data
    onLoadMoreData(value: RequisitionMaster) { }
}