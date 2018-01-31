// angular
import { Component, OnInit, OnDestroy, Inject, ViewChild } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material";
// models
import { RequisitionMaster,PaintTaskDetail, Color } from "../../models/model.index";
// service
import { RequisitionMasterService } from "../../services/stock/requisition-master.service";
import { PaintWorkitemService } from "../../services/require-paint/paint-workitem.service";
// rxjs
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";
// base-component
import { TableColumn } from "@swimlane/ngx-datatable";
// 3rd-party
import { DateOnlyPipe } from "../../pipes/date-only.pipe";
// timezone
import * as moment from "moment-timezone";
import { AuthService } from "../../services/auth/auth.service";
@Component({
    selector: "requisition-list-dialog",
    templateUrl: "./requisition-list-dialog.component.html",
    styleUrls: [
        "../../styles/master.style.scss",
        "../../styles/edit.style.scss"
    ],
    providers: [
        RequisitionMasterService,
        PaintWorkitemService
    ]
})
/** requisition-list-dialog component*/
export class RequisitionListDialogComponent implements OnInit, OnDestroy {
    /** requisition-list-dialog ctor */
    constructor(
        private serviceRequisition: RequisitionMasterService,
        private servicePaintWorkItem: PaintWorkitemService,
        private serviceAuth: AuthService,
        public dialogRef: MatDialogRef<RequisitionListDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public paintTaskDetail: PaintTaskDetail
    ) { }

    // Parameter
    datePipe: DateOnlyPipe = new DateOnlyPipe("it");
    requisitionMasters: Array<RequisitionMaster>;
    removeRequisitionMasters: Array<RequisitionMaster>;
    colorItem: Color;

    /** Called by Angular after project-dialog component initialized */
    ngOnInit(): void {
        if (!this.requisitionMasters) {
            this.requisitionMasters = new Array;
        }
        if (!this.removeRequisitionMasters) {
            this.removeRequisitionMasters = new Array;
        }
        // Load-Data
        this.loadData();
    }

    // angular hook
    ngOnDestroy(): void { }

    // on get data with lazy load
    loadData(): void {
        if (this.paintTaskDetail) {
            this.serviceRequisition.getByMasterId(this.paintTaskDetail.PaintTaskDetailId)
                .subscribe(dbRequisiton => {
                    if (dbRequisiton) {
                        dbRequisiton.forEach((item, index) => {
                            if (item.RequisitionDate) {
                                item.RequisitionDate = item.RequisitionDate != null ?
                                    new Date(item.RequisitionDate) : new Date();
                            }
                            dbRequisiton[index] = item;
                        });
                    }
                    this.requisitionMasters = dbRequisiton.slice();
                });

            if (this.paintTaskDetail.PaintWorkItem) {
                this.colorItem = {
                    ColorItemId: (this.paintTaskDetail.PaintTaskDetailLayer === 1 ?
                        this.paintTaskDetail.PaintWorkItem.IntColorItemId || 0: this.paintTaskDetail.PaintWorkItem.ExtColorItemId || 0),
                    ColorName: (this.paintTaskDetail.PaintTaskDetailLayer === 1 ?
                        this.paintTaskDetail.PaintWorkItem.IntColorString || '' : this.paintTaskDetail.PaintWorkItem.ExtColorString || ''),
                }
            }
        }
    }

    // no cancel click
    onCancelClick(): void {
        this.dialogRef.close();
    }

    // on save
    onUpdateClick(): void {
        if (this.requisitionMasters) {
            this.requisitionMasters.forEach(item => {
                if (item.RequisitionMasterId < 1) {
                    this.onInsertToDataBase(Object.assign({}, item));
                } else {
                    this.onUpdateToDataBase(Object.assign({}, item));
                }
            });
        }
        if (this.removeRequisitionMasters) {
            this.removeRequisitionMasters.forEach(item => {
                this.serviceRequisition.deleteKeyNumber(item.RequisitionMasterId)
                    .subscribe(complate => console.log(JSON.stringify(complate)));
            });
        }
        this.dialogRef.close(true);
    }

    // on change time zone befor update to webapi
    changeTimezone(value: RequisitionMaster): RequisitionMaster {
        let zone: string = "Asia/Bangkok";
        if (value !== null) {
            if (value.RequisitionDate !== null) {
                value.RequisitionDate = moment.tz(value.RequisitionDate, zone).toDate();
            }
            if (value.CreateDate !== null) {
                value.CreateDate = moment.tz(value.CreateDate, zone).toDate();
            }
            if (value.ModifyDate !== null) {
                value.ModifyDate = moment.tz(value.ModifyDate, zone).toDate();
            }
        }
        return value;
    }

    // on insert data
    onInsertToDataBase(value: RequisitionMaster): void {
        if (this.serviceAuth.getAuth) {
            value.Creator = this.serviceAuth.getAuth.UserName || "";
        }
        // change timezone
        value = this.changeTimezone(value);
        // insert data
        this.serviceRequisition.post(value).subscribe(
            (complete: RequisitionMaster) => {
                // console.log("onInsertToDataBase");
            }
        );
    }

    // on update data
    onUpdateToDataBase(value: RequisitionMaster): void {
        if (this.serviceAuth.getAuth) {
            value.Modifyer = this.serviceAuth.getAuth.UserName || "";
        }
        // change timezone
        value = this.changeTimezone(value);
        // update data
        this.serviceRequisition.putKeyNumber(value, value.RequisitionMasterId).subscribe(
            (complete: any) => {
                // console.log("onUpdateToDataBase");
            });
    }

    // on add new requisition
    onAddNewRequisition(): void {
        let requisition: RequisitionMaster = {
            RequisitionMasterId: 0,
            RequisitionDate: new Date,
            Quantity: 1,
            ColorItemId: this.colorItem.ColorItemId,
            PaintTaskDetailId: this.paintTaskDetail.PaintTaskDetailId,
        };

        if (this.serviceAuth.getAuth) {
            requisition.RequisitionBy = this.serviceAuth.getAuth.EmpCode;
        }

        // debug here
        // console.log(JSON.stringify(requisition));

        this.requisitionMasters.push(requisition);
        this.requisitionMasters = [...this.requisitionMasters];
    }

    // on remove requisition
    onRemoveRequisition(requisition: RequisitionMaster): void {
        // remove Detail
        if (requisition && this.requisitionMasters) {
            // find id
            let index: number = this.requisitionMasters.indexOf(requisition);
            if (index > -1) {
                if (requisition.RequisitionMasterId < 1) {
                    this.requisitionMasters.splice(index, 1);
                } else {
                    const requisitionMaster: RequisitionMaster | undefined = this.requisitionMasters
                        .find((value, index) => value.RequisitionMasterId === requisition.RequisitionMasterId);
                    if (requisitionMaster) {
                        this.removeRequisitionMasters.push(Object.assign({}, requisitionMaster));
                    }
                    this.requisitionMasters.splice(index, 1);
                }
                // update array
                this.requisitionMasters = this.requisitionMasters.slice();
            }
        }
    }

    // update value on data-table
    updateValue(event: any, cell: string, rowIndex: number): void {
        if (this.requisitionMasters) {
            // console.log("Get By index!", this.editValue.OverTimeDetails[rowIndex][cell]);
            // befor use index must add [key: string]: string | number | Date | undefined; in interface
            this.requisitionMasters[rowIndex][cell] = event.target.value;
            this.requisitionMasters = [...this.requisitionMasters];
        }
        // console.log("UPDATED!", this.employees[rowIndex][cell]);
    }

    // update value on data-table
    updateValueForPrimeNgComponent(event: any, cell: string, rowIndex: number): void {
        if (this.requisitionMasters) {
            // console.log("Get By index!", this.editValue.OverTimeDetails[rowIndex][cell]);
            // befor use index must add [key: string]: string | number | Date | undefined; in interface
            this.requisitionMasters[rowIndex][cell] = event;
            this.requisitionMasters = [...this.requisitionMasters];
        }
        // console.log("UPDATED!", this.employees[rowIndex][cell]);
    }
}