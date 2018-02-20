// angular
import { Component, Output, EventEmitter, Input } from "@angular/core";
// models
import { RequirePaintList, AttachFile, RequirePaintMaster } from "../../../models/model.index";
// components
import { BaseViewComponent } from "../../base-component/base-view.component";
// services
import { RequirePaintListService } from "../../../services/require-paint/require-paint-list.service";
import { RequirePaintMasterService } from "../../../services/require-paint/require-paint-master.service";

@Component({
    selector: "require-painting-list-byinitial-view",
    templateUrl: "./require-painting-list-byinitial-view.component.html",
    styleUrls: ["../../../styles/view.style.scss"],
})
/** require-painting-list-byinitial-view component*/
export class RequirePaintingListByinitialViewComponent extends BaseViewComponent<RequirePaintList> {
    /** require-painting-list-byinitial-view ctor */
    constructor(
        private serviceRequireMaster: RequirePaintMasterService,
        private serviceRequireList: RequirePaintListService,
    ) {
        super();
    }
    // Parameter
    attachFiles: Array<AttachFile> = new Array;
    requirePaintMaster: RequirePaintMaster;

    // load more data
    onLoadMoreData(value: RequirePaintList) {

        if (value) {
            if (value.RequirePaintingListId) {
                this.serviceRequireList.getAttachFile(value.RequirePaintingListId)
                    .subscribe(dbAttach => this.attachFiles = dbAttach.slice());
            }

            if (value.RequirePaintingMasterId) {
                this.serviceRequireMaster.getOneKeyNumber(value.RequirePaintingMasterId)
                    .subscribe(dbRequireMaster => this.requirePaintMaster = dbRequireMaster);
            }
        }
    }

    // open attact file
    onOpenNewLink(link: string): void {
        if (link) {
            window.open("paint/" + link, "_blank");
            //this.serviceMaster.getDownloadFilePaper(link)
            //    .subscribe(data => {
            //        let link: any = document.createElement("a");
            //        link.href = window.URL.createObjectURL(data);
            //        // link.download = "file_";
            //        link.click();
            //    });
        }
    }
}