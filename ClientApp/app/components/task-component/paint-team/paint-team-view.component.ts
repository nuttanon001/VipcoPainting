// angular
import { Component, Output, EventEmitter, Input } from "@angular/core";
// models
import { PaintTeam } from "../../../models/model.index";
// components
import { BaseViewComponent } from "../../base-component/base-view.component";

@Component({
    selector: 'paint-team-view',
    templateUrl: './paint-team-view.component.html',
    styleUrls: ["../../../styles/view.style.scss"],
})
/** paint-team-view component*/
export class PaintTeamViewComponent extends BaseViewComponent<PaintTeam> {
    /** paint-team-view ctor */
    constructor() {
        super();
    }
    // load more data
    onLoadMoreData(value: PaintTeam) { }
}