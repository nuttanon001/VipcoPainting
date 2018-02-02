// angular
import { Component, Output, EventEmitter, Input, ViewContainerRef } from "@angular/core";
// models
import { RequirePaintMaster, InitialRequirePaint } from "../../../models/model.index";
// components
import { BaseViewComponent } from "../../base-component/base-view.component";
// services
import { InitialRequirePaintService } from "../../../services/require-paint/initial-require-paint.service";
import { DialogsService } from "../../../services/dialog/dialogs.service";
import { TableColumn } from "@swimlane/ngx-datatable";

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
        private dialogService: DialogsService,
        private viewContainerRef: ViewContainerRef,
    ) {
        super();
    }

    // parameter
    initialRequirePaint: InitialRequirePaint;

    // load more data
    onLoadMoreData(value: RequirePaintMaster) {
        this.service.getByMasterId(value.RequirePaintingMasterId)
            .subscribe(dbInitials => {
                if (dbInitials) {
                    if (dbInitials.length > 0) {
                        this.initialRequirePaint = dbInitials[0];
                    }
                }
            }, error => console.error(error));
    }
}