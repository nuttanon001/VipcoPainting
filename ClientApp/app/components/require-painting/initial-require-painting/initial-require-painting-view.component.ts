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
    // Parameter
    columnsBlastWork: Array<TableColumn> = [
        { prop: "IntArea", name: "IntArea", width: 70 },
        { prop: "IntSurfaceTypeString", name: "IntSurface", width: 120 },
        { prop: "IntStandradTimeString", name: "IntStd", width: 120 },
        { prop: "ExtArea", name: "ExtArea", width: 70 },
        { prop: "ExtSurfaceTypeString", name: "ExtSurface", width: 120 },
        { prop: "ExtStandradTimeString", name: "ExtStd", width: 120 },
    ];
    columnsPaintWork: Array<TableColumn> = [
        { prop: "PaintLevelString", name: "Level", width: 120 },
        { prop: "IntAreaString", name: "IntArea", width: 150 },
        { prop: "IntColorString", name: "IntColor", width: 120 },
        { prop: "IntStandradTimeString", name: "IntStd", width: 120 },
        { prop: "ExtAreaString", name: "ExtArea", width: 150 },
        { prop: "ExtColorString", name: "ExtColor", width: 120 },
        { prop: "ExtStandradTimeString", name: "ExtStd", width: 120 },
    ];

    blastWorkItems: Array<BlastWorkItem>;
    paintWorkItems: Array<PaintWorkItem>;

    // load more data
    onLoadMoreData(value: RequirePaintMaster) {
        this.service.getByMasterId(value.RequirePaintingMasterId)
            .subscribe(dbInitials => {
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