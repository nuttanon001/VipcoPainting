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
        { prop: "Description", name: "Description", flexGrow: 1 },
        { prop: "MarkNo", name: "MarkNo", flexGrow: 1 },
        { prop: "DrawingNo", name: "DrawingNo", flexGrow: 1 },
        { prop: "UnitNo", name: "UnitNo", flexGrow: 1 },
        { prop: "Quantity", name: "Q'ty", flexGrow: 1 },
        { prop: "Weight", name: "Weight", flexGrow: 1 },
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