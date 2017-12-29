// angular
import { Component, Output, EventEmitter, Input } from "@angular/core";
// models
import { PaintTaskMaster, RequirePaintList, RequirePaintMaster } from "../../../models/model.index";
// components
import { BaseViewComponent } from "../../base-component/base-view.component";
import { PaintTaskDetailService } from "../../../services/paint-task/paint-task-detail.service";
import { RequirePaintListService } from "../../../services/require-paint/require-paint-list.service";
import { RequirePaintMasterService } from "../../../services/require-paint/require-paint-master.service";

@Component({
    selector: "paint-task-view",
    templateUrl: "./paint-task-view.component.html",
    styleUrls: ["../../../styles/view.style.scss"],
})
/** paint-task-view component*/
export class PaintTaskViewComponent extends BaseViewComponent<PaintTaskMaster>{
    /** paint-task-view ctor */
    constructor(
        private servicePaintTaskDetail: PaintTaskDetailService,
        private serviceRequirePaintList: RequirePaintListService,
        private serviceRequirePaintMaster: RequirePaintMasterService,
    ) {
        super();
    }
    //Parameter
    requirePaintList: RequirePaintList;
    requirePaintMaster: RequirePaintMaster;
    newValue: PaintTaskMaster | undefined;
    // load more data
    onLoadMoreData(value: PaintTaskMaster) {
        this.newValue = undefined;
        setTimeout(() => {
            this.newValue = value;
        }, 50);

        if (value.RequirePaintingListId) {
            this.serviceRequirePaintList.getOneKeyNumber(value.RequirePaintingListId)
                .subscribe(dbRequirePaintList => {
                    this.requirePaintList = dbRequirePaintList;
                    // Get RequirePaintingMaster
                    if (this.requirePaintList.RequirePaintingMasterId) {
                        this.serviceRequirePaintMaster.getOneKeyNumber(this.requirePaintList.RequirePaintingMasterId)
                            .subscribe(dbRequirePaintingMaster => {
                                this.requirePaintMaster = dbRequirePaintingMaster;
                            });
                    }
                });
        }

        this.servicePaintTaskDetail.getByMasterId(value.PaintTaskMasterId)
            .subscribe(dbPaintTaskDetails => {
                this.displayValue.PaintTaskDetails = dbPaintTaskDetails.slice();
            });
    }
}