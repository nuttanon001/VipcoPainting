// angular
import { Component, Output, EventEmitter, Input, ViewContainerRef} from "@angular/core";
// models
import { RequirePaintMaster, RequirePaintList, AttachFile } from "../../../models/model.index";
// components
import { BaseViewComponent } from "../../base-component/base-view.component";
// services
import { RequirePaintListService } from "../../../services/require-paint/require-paint-list.service";
import { DialogsService } from "../../../services/dialog/dialogs.service";
import { TableColumn } from "@swimlane/ngx-datatable";
import { RequirePaintMasterService } from "../../../services/require-paint/require-paint-master.service";

@Component({
    selector: "require-painting-view",
    templateUrl: "./require-painting-view.component.html",
    styleUrls: ["../../../styles/view.style.scss"],
})
// require-painting-view component*/
export class RequirePaintingViewComponent extends BaseViewComponent<RequirePaintMaster>
{
    /** require-painting-view ctor */
    constructor(
        public service: RequirePaintListService,
        private serviceMaster: RequirePaintMasterService,
        private dialogService: DialogsService,
        private viewContainerRef: ViewContainerRef,
    ) {
        super();
    }

    //parameter
    requireLists: Array<RequirePaintList> = new Array;
    attachFiles: Array<AttachFile> = new Array;
    @Input("height") height: string = "60vh";

    columns: Array<TableColumn> = [
        { prop: "Description", name: "Description", width: 250 },
        { prop: "MarkNo", name: "MarkNo", width: 150 },
        { prop: "DrawingNo", name: "DrawingNo", width: 150 },
        { prop: "UnitNo", name: "UnitNo", width: 75 },
        { prop: "Quantity", name: "Q'ty", width: 75 },
        { prop: "Weight", name: "Weight", width: 75 },
        //{ prop: "RequirePaintingListStatus", name: "Status", width:75 }
    ];

    // load more data
    onLoadMoreData(value: RequirePaintMaster) {
        this.attachFiles = new Array;
        this.requireLists = new Array;

        if (value) {
            if (value.RequirePaintingMasterId) {
                this.service.getByMasterId(value.RequirePaintingMasterId)
                    .subscribe(dbLists => {
                        this.requireLists = dbLists.slice();//[...dbDetail];
                    }, error => console.error(error));

                this.serviceMaster.getAttachFile(value.RequirePaintingMasterId)
                    .subscribe(dbAttach => this.attachFiles = dbAttach.slice());
            }
        }
    }

    // on Require-Workitem
    onSelectedRequestWorkItem(value?: RequirePaintList) {
        if (value) {
            this.dialogService.dialogRequestPaintList(this.viewContainerRef, value.RequirePaintingListId);
        }
    }
}