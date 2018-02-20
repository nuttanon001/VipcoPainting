// Angular Core
import { Component, Inject } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";
// Components
import { RequirePaintingViewDialogComponent } from "./dialog.index";
// Models
import { RequirePaintList } from "../../models/model.index";
// Services
import { RequirePaintMasterService } from "../../services/require-paint/require-paint-master.service";
import { RequirePaintListService } from "../../services/require-paint/require-paint-list.service";

@Component({
    selector: "require-painting-bydetail-view",
    templateUrl: "./require-painting-view-dialog.component.html",
    styleUrls: ["../../styles/master.style.scss"],
})
/** require-painting-bydetail-view component*/
export class RequirePaintingBydetailViewComponent extends RequirePaintingViewDialogComponent {
    /** require-painting-bydetail-view ctor */
    constructor(
        service: RequirePaintMasterService,
        private serviceDetail: RequirePaintListService,
        @Inject(MAT_DIALOG_DATA) public requirePaintList: RequirePaintList,
        dialogRef: MatDialogRef<number>
    ) {
        super(
            service,
            requirePaintList.RequirePaintingMasterId || 0,
            dialogRef);

        this.displayMode2 = true;
        this.RequireListId = requirePaintList.RequirePaintingListId;
        this.isReceive = requirePaintList.IsReceive || false;
    }
    // Parameter
    displayMode2: boolean;
    RequireListId: number;
    isReceive: boolean;
    // On Angular Hook Init
    ngOnInit(): void {

        super.ngOnInit();
    }
}