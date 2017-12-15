// angular
import { Component, Output, EventEmitter, Input } from "@angular/core";
// models
import { StandradTime } from "../../../models/model.index";
// components
import { BaseViewComponent } from "../../base-component/base-view.component";

@Component({
    selector: "standardtime-view",
    templateUrl: "./standardtime-view.component.html",
    styleUrls: ["../../../styles/view.style.scss"],
})
/** standardtime-view component*/
export class StandardtimeViewComponent extends BaseViewComponent<StandradTime> {
    /** standardtime-view ctor */
    constructor() {
        super();
    }

    // load more data
    onLoadMoreData(value: StandradTime) { }
}

