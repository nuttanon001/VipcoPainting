// angular
import { Component, OnInit, OnDestroy, Inject, ViewChild } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material";
// models
import { RequisitionMaster,PaintTaskDetail } from "../../models/model.index";
// service
import { RequisitionMasterService } from "../../services/stock/requisition-master.service";
// rxjs
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";
// base-component
import { TableColumn } from "@swimlane/ngx-datatable";
// 3rd-party
import { DateOnlyPipe } from "../../pipes/date-only.pipe";

@Component({
    selector: "requisition-list-dialog",
    templateUrl: "./requisition-list-dialog.component.html",
    styleUrls: ["../../styles/master.style.scss"],
    providers: [
        RequisitionMasterService,
    ]
})
/** requisition-list-dialog component*/
export class RequisitionListDialogComponent implements OnInit, OnDestroy {
    /** requisition-list-dialog ctor */
    constructor(
        private serviceRequisition: RequisitionMasterService,
        public dialogRef: MatDialogRef<RequisitionListDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public paintTaskDetail: PaintTaskDetail
    ) { }

    // Parameter
    datePipe: DateOnlyPipe = new DateOnlyPipe("it");
    requisitionMasters: Array<RequisitionMaster>;

    /** Called by Angular after project-dialog component initialized */
    ngOnInit(): void {
        if (!this.requisitionMasters) {
            this.requisitionMasters = new Array;
        }
        // Load-Data
        this.loadData(this.paintTaskDetail);
    }

    // angular hook
    ngOnDestroy(): void { }

    // on get data with lazy load
    loadData(paintTaskDetail: PaintTaskDetail): void {
        if (paintTaskDetail) {
            this.serviceRequisition.getByMasterId(paintTaskDetail.PaintTaskDetailId)
                .subscribe(dbRequisiton => {
                    this.requisitionMasters = dbRequisiton.slice();
                });
        }
    }

    // no cancel click
    onCancelClick(): void {
        this.dialogRef.close();
    }

    // on save
    onUpdateClick(): void {

    }

    // update value on data-table
    updateValue(event: any, cell: string, rowIndex: number): void {
        // console.log("inline editing rowIndex", rowIndex);
        // console.log(rowIndex + "-" + cell);
        // console.log("value:", event.target.value);

        if (this.requisitionMasters) {
            // console.log("Get By index!", this.editValue.OverTimeDetails[rowIndex][cell]);
            // befor use index must add [key: string]: string | number | Date | undefined; in interface
            this.requisitionMasters[rowIndex][cell] = event.target.value;
            this.requisitionMasters = [...this.requisitionMasters];
        }
        // console.log("UPDATED!", this.employees[rowIndex][cell]);
    }
}