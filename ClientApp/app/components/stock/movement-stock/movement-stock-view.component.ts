// angular
import { Component, Output, EventEmitter, Input } from "@angular/core";
// models
import { ColorMovementStock } from "../../../models/model.index";
// components
import { BaseViewComponent } from "../../base-component/base-view.component";

@Component({
    selector: "movement-stock-view",
    templateUrl: "./movement-stock-view.component.html",
    styleUrls: ["../../../styles/view.style.scss"],
})
/** movement-stock-view component*/
export class MovementStockViewComponent extends BaseViewComponent<ColorMovementStock> {
    /** movement-stock-view ctor */
    constructor() {
        super();
    }
    // load more data
    onLoadMoreData(value: ColorMovementStock) { }
}