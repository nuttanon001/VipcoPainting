// angular
import { Component, Output, EventEmitter, Input } from "@angular/core";
// models
import { SurfaceType } from "../../../models/model.index";
// components
import { BaseViewComponent } from "../../base-component/base-view.component";

@Component({
    selector: "surface-view",
    templateUrl: "./surface-view.component.html",
    styleUrls: ["../../../styles/view.style.scss"],
})
/** surface-view component*/
export class SurfaceViewComponent extends BaseViewComponent<SurfaceType> {
    /** surface-view ctor */
    constructor() {
        super();
    }

    // load more data
    onLoadMoreData(value: SurfaceType) { }
}