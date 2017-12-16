// angular
import { Component, Inject, ViewChild, OnDestroy } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material";
// models
import { StandradTime, Scroll } from "../../models/model.index";
// service
import { StandradTimeService } from "../../services/standrad-time/standrad-time.service";
import { DataTableServiceCommunicate } from "../../services/data-table/data-table.service";
// rxjs
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";
// base-component
import { BaseDialogComponent } from "../base-component/base-dialog.component";

@Component({
    selector: "standrad-time-dialog",
    templateUrl: "./standrad-time-dialog.component.html",
    styleUrls: ["../../styles/master.style.scss"],
    providers: [
        StandradTimeService,
        DataTableServiceCommunicate
    ]
})
// standrad-time-dialog component*/
export class StandradTimeDialogComponent
    extends BaseDialogComponent<StandradTime, StandradTimeService> implements OnDestroy {
    /** standrad-time-dialog ctor */
    constructor(
        public service: StandradTimeService,
        public serviceDataTable: DataTableServiceCommunicate<StandradTime>,
        public dialogRef: MatDialogRef<StandradTimeDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public mode: number
    ) {
        super(
            service,
            serviceDataTable,
            dialogRef
        );

        this.columns = [
            { prop: "Code", name: "Code", flexGrow: 1 },
            { prop: "Description", name: "Description", flexGrow: 1 },
            { prop: "RateWithUnit", name: "StandardTime", flexGrow: 1 },
            { prop: "PercentLoss", name: "Loss", flexGrow: 1 },
        ];
    }

    // on init
    onInit(): void {
        this.fastSelectd = true;
    }

    // on get data with lazy load
    loadDataScroll(scroll: Scroll): void {
        if (this.mode) {
            scroll.Where = this.mode.toString();
        }
        this.service.getAllWithScroll(scroll)
            .subscribe(scrollData => {
                if (scrollData) {
                    this.serviceDataTable.toChild(scrollData);
                }
            }, error => console.error(error));
    }

    // on destory
    ngOnDestroy(): void {
        if (this.subscription) {
            this.subscription.unsubscribe();
        }
    }
}