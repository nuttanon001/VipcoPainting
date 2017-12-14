// angular
import { Component, Output, EventEmitter, Input } from "@angular/core";
// models
import { RequirePaintMaster, RequirePaintList,RequirePaintSub } from "../../../models/model.index";
// components
import { BaseViewComponent } from "../../base-component/base-view.component";
// services
import { RequirePaintListService } from "../../../services/require-paint/require-paint-list.service";
import { RequirePaintSubService } from "../../../services/require-paint/require-paint-sub.service";

@Component({
    selector: "require-painting-view",
    templateUrl: "./require-painting-view.component.html",
    styleUrls: ["../../../styles/view.style.scss"],
})
// require-painting-view component*/
export class RequirePaintingViewComponent extends BaseViewComponent<RequirePaintMaster>
{
    requireLists: Array<RequirePaintList> = new Array;
    /** require-painting-view ctor */
    constructor(
        private service: RequirePaintListService,
        private serviceSub: RequirePaintSubService,
    ) {
        super();
    }

    // load more data
    onLoadMoreData(value: RequirePaintMaster) {
        this.service.getByMasterId(value.RequirePaintingMasterId)
            .subscribe(dbLists => {
                this.requireLists = dbLists.slice();//[...dbDetail];
                // console.log("DataBase is :", this.details);
            }, error => console.error(error));
    }
}