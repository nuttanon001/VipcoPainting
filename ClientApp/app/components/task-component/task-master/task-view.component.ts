// angular
import { Component, Output, EventEmitter, Input } from "@angular/core";
// models
import { TaskMaster, RequirePaintList, RequirePaintMaster } from "../../../models/model.index";
// components
import { BaseViewComponent } from "../../base-component/base-view.component";
import { TaskBlastDetailService } from "../../../services/task/task-blast-detail.service";
import { TaskPaintDetailService } from "../../../services/task/task-paint-detail.service";
import { RequirePaintListService } from "../../../services/require-paint/require-paint-list.service";
import { RequirePaintMasterService } from "../../../services/require-paint/require-paint-master.service";
@Component({
    selector: "task-view",
    templateUrl: "./task-view.component.html",
    styleUrls: ["../../../styles/view.style.scss"],
})
/** task-view component*/
export class TaskViewComponent extends BaseViewComponent<TaskMaster>{
    /** task-view ctor */
    constructor(
        private serviceTaskBlast: TaskBlastDetailService,
        private serviceTaskPaint: TaskPaintDetailService,
        private serviceRequirePaintList: RequirePaintListService,
        private serviceRequirePaintMaster: RequirePaintMasterService,
    ) {
        super();
    }
    //Parameter
    requirePaintList: RequirePaintList;
    requirePaintMaster: RequirePaintMaster;
    // Show On/Off
    get ShowBlast(): boolean {
        if (this.displayValue) {
            if (this.displayValue.TaskBlastDetails) {
                if (this.displayValue.TaskBlastDetails.length > 0) {
                    return true;
                }
            }
        }
        return false;
    }
    get ShowPaint(): boolean {
        if (this.displayValue) {
            if (this.displayValue.TaskPaintDetails) {
                if (this.displayValue.TaskPaintDetails.length > 0) {
                    return true;
                }
            }
        }
        return false;
    }

    // load more data
    onLoadMoreData(value: TaskMaster) {
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

        this.serviceTaskBlast.getByMasterId(value.TaskMasterId)
            .subscribe(dbTaskBlast => {
                this.displayValue.TaskBlastDetails = dbTaskBlast.slice();
            });
        // Get TaskPaintDetails
        this.serviceTaskPaint.getByMasterId(value.TaskMasterId)
            .subscribe(dbTaskPaint => {
                this.displayValue.TaskPaintDetails = dbTaskPaint.slice();
            });
    }
}