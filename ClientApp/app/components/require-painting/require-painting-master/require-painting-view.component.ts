// angular
import { Component, Output, EventEmitter, Input, ViewContainerRef} from "@angular/core";
// models
import { RequirePaintMaster, RequirePaintList } from "../../../models/model.index";
// components
import { BaseViewComponent } from "../../base-component/base-view.component";
// services
import { RequirePaintListService } from "../../../services/require-paint/require-paint-list.service";
import { DialogsService } from "../../../services/dialog/dialogs.service";
import { TableColumn } from "@swimlane/ngx-datatable";

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
        private service: RequirePaintListService,
        private dialogService: DialogsService,
        private viewContainerRef: ViewContainerRef,
    ) {
        super();
    }

    //parameter
    requireLists: Array<RequirePaintList> = new Array;
    @Input("height") height: string = "calc(100vh - 184px)";

    columns: Array<TableColumn> = [
        { prop: "Description", name: "Description", width: 250 },
        { prop: "MarkNo", name: "MarkNo", width: 150 },
        { prop: "DrawingNo", name: "DrawingNo", width: 150 },
        { prop: "UnitNo", name: "UnitNo", width: 75 },
        { prop: "Quantity", name: "Q'ty", width: 75 },
        { prop: "Weight", name: "Weight", width: 75 },
    ];

    // load more data
    onLoadMoreData(value: RequirePaintMaster) {
        this.service.getByMasterId(value.RequirePaintingMasterId)
            .subscribe(dbLists => {
                this.requireLists = dbLists.slice();//[...dbDetail];
                // console.log("DataBase is :", this.details);
            }, error => console.error(error));
    }

    // on Require-Workitem
    onSelectedRequestWorkItem(value?: RequirePaintList) {
        if (value) {
            this.dialogService.dialogRequestPaintList(this.viewContainerRef, value.RequirePaintingListId);
        }
    }
}