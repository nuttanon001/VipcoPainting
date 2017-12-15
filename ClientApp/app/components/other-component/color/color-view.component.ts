// angular
import { Component, Output, EventEmitter, Input } from "@angular/core";
// models
import { Color } from "../../../models/model.index";
// components
import { BaseViewComponent } from "../../base-component/base-view.component";

@Component({
    selector: "color-view",
    templateUrl: "./color-view.component.html",
    styleUrls: ["../../../styles/view.style.scss"],
})
/** color-view component*/
export class ColorViewComponent extends BaseViewComponent<Color> {
    /** color-view ctor */
    constructor() {
        super();
    }

    // load more data
    onLoadMoreData(value: Color) {}
}