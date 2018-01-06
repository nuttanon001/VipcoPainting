// angular
import { Component, Output, EventEmitter, Input } from "@angular/core";
// models
import { MovementStockStatus } from "../../../models/model.index";
// components
import { BaseViewComponent } from "../../base-component/base-view.component";

@Component({
    selector: "movement-status-view",
    templateUrl: "./movement-status-view.component.html",
    styleUrls: ["../../../styles/view.style.scss"],
})
/** movement-status-view component*/
export class MovementStatusViewComponent extends BaseViewComponent<MovementStockStatus> {
    /** movement-status-view ctor */
    constructor() {
        super();
    }
    // load more data
    onLoadMoreData(value: MovementStockStatus) { }
}