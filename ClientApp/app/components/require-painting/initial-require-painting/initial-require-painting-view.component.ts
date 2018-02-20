// angular
import { Component, Output, EventEmitter, Input, ViewContainerRef } from "@angular/core";
// models
import { RequirePaintMaster, InitialRequirePaint, BlastWorkItem, PaintWorkItem } from "../../../models/model.index";
// components
import { BaseViewComponent } from "../../base-component/base-view.component";
// services
import { InitialRequirePaintService } from "../../../services/require-paint/initial-require-paint.service";
import { DialogsService } from "../../../services/dialog/dialogs.service";
import { TableColumn } from "@swimlane/ngx-datatable";
import { BlastWorkitemService } from "../../../services/require-paint/blast-workitem.service";
import { PaintWorkitemService } from "../../../services/require-paint/paint-workitem.service";

@Component({
    selector: "initial-require-painting-view",
    templateUrl: "./initial-require-painting-view.component.html",
    styleUrls: ["../../../styles/view.style.scss"],
})

/** initial-require-painting-view component*/
export class InitialRequirePaintingViewComponent extends BaseViewComponent<RequirePaintMaster>
{
    /** initial-require-painting-view ctor */
    constructor(
        public service: InitialRequirePaintService,
        private serviceBlastWork: BlastWorkitemService,
        private servicePaintWork: PaintWorkitemService,
        private dialogService: DialogsService,
        private viewContainerRef: ViewContainerRef,
    ) {
        super();
    }

    // parameter
    initialRequirePaint: InitialRequirePaint;
    columnsBlastWork: Array<TableColumn> = [
        { prop: "IntSurfaceTypeString", name: "Internal Surface", width: 150 },
        { prop: "IntStandradTimeString", name: "Internal Std", width: 200 },
        { prop: "ExtSurfaceTypeString", name: "External Surface", width: 150 },
        { prop: "ExtStandradTimeString", name: "External Std", width: 200 },
    ];
    columnsPaintWork: Array<TableColumn> = [
        { prop: "PaintLevelString", name: "Level", width: 150 },
        { prop: "IntColorString", name: "Internal Color", width: 200 },
        { prop: "IntStandradTimeString", name: "Internal Std", width: 200 },
        { prop: "ExtColorString", name: "External Color", width: 200 },
        { prop: "ExtStandradTimeString", name: "External Std", width: 200 },
    ];

    blastWorkItems: Array<BlastWorkItem>;
    paintWorkItems: Array<PaintWorkItem>;

    // load more data
    onLoadMoreData(value: RequirePaintMaster) {
        this.service.getByMasterId(value.RequirePaintingMasterId)
            .subscribe(dbInitials => {
                // Debug here
                // console.log("Data ", JSON.stringify(dbInitials));

                if (dbInitials) {
                    if (dbInitials.length > 0) {
                        this.initialRequirePaint = dbInitials[0];
                        if (this.initialRequirePaint) {
                            this.serviceBlastWork.getByMasterId(this.initialRequirePaint.InitialRequireId, "GetByMaster2/")
                                .subscribe(dbData => {
                                    this.blastWorkItems = dbData.slice();
                                });

                            this.servicePaintWork.getByMasterId(this.initialRequirePaint.InitialRequireId, "GetByMaster2/")
                                .subscribe(dbData => {
                                    this.paintWorkItems = dbData.slice();
                                });
                        }
                    }
                }
            }, error => console.error(error));
    }
}