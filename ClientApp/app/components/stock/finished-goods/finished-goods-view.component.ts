// angular
import { Component, Output, EventEmitter, Input } from "@angular/core";
// models
import { FinishedGoodsMaster } from "../../../models/model.index";
// components
import { BaseViewComponent } from "../../base-component/base-view.component";

@Component({
    selector: "finished-goods-view",
    templateUrl: "./finished-goods-view.component.html",
    styleUrls: ["../../../styles/view.style.scss"],
})
/** finished-goods-view component*/
export class FinishedGoodsViewComponent extends BaseViewComponent<FinishedGoodsMaster> {
    /** finished-goods-view ctor */
    constructor() {
        super();
    }
    // load more data
    onLoadMoreData(value: FinishedGoodsMaster) { }
}