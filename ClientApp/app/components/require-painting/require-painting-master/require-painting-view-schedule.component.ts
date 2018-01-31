// angular
import { Component, Output, EventEmitter, Input, ViewContainerRef } from "@angular/core";
import { RequirePaintList, RequirePaintMaster } from "../../../models/model.index";
import { RequirePaintingViewComponent } from "./require-painting-view.component";
import { DialogsService } from "../../../services/dialog/dialogs.service";
import { RequirePaintListService } from "../../../services/require-paint/require-paint-list.service";

@Component({
    selector: "require-painting-view-schedule",
    templateUrl: "./require-painting-view.component.html",
    styleUrls: ["../../../styles/view.style.scss"],
})
/** require-painting-view-schedule component*/
export class RequirePaintingViewScheduleComponent extends RequirePaintingViewComponent {
    /** require-painting-view-schedule ctor */
    constructor(
        service: RequirePaintListService,
        dialogService: DialogsService,
        viewContainerRef: ViewContainerRef,
    ) {
        super(service,
            dialogService,
            viewContainerRef);
    }

    // Parameter
    @Output("selected") selected = new EventEmitter<number>();

    // load more data OverRide
    onLoadMoreData(value: RequirePaintMaster) {
        this.service.getByMasterId(value.RequirePaintingMasterId)
            .subscribe(dbLists => {
                this.requireLists = new Array;
                dbLists.forEach(item => {
                    if (item.RequirePaintingListStatus === 1) {
                        this.requireLists.push(item);
                    }
                });
                // this.requireLists = dbLists.slice(); //[...dbDetail];
                // console.log("DataBase is :", this.details);
            }, error => console.error(error));
    }

    // on Require-Workitem override
    onSelectedRequestWorkItem(value?: RequirePaintList) {
        if (value) {
            this.selected.emit(value.RequirePaintingListId);
        }
    }
}