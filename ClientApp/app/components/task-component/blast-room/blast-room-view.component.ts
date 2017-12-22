// angular
import { Component, Output, EventEmitter, Input } from "@angular/core";
// models
import { BlastRoom } from "../../../models/model.index";
// components
import { BaseViewComponent } from "../../base-component/base-view.component";

@Component({
    selector: 'blast-room-view',
    templateUrl: './blast-room-view.component.html',
    styleUrls: ["../../../styles/view.style.scss"],
})

/** blast-room-view component*/
export class BlastRoomViewComponent extends BaseViewComponent<BlastRoom> {
    /** blast-room-view ctor */
    constructor() {
        super();
    }
    // load more data
    onLoadMoreData(value: BlastRoom) { }
}