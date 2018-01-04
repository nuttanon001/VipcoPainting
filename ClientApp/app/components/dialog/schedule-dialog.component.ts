// angular
import { Component, Inject, ViewChild, OnDestroy, OnInit } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material";
// models
import { OptionTaskMasterSchedule } from "../../models/model.index";
// service
import { PaintTaskMasterService } from "../../services/paint-task/paint-task-master.service";
// rxjs
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";
import { LazyLoadEvent } from "primeng/primeng";

@Component({
    selector: "schedule-dialog",
    templateUrl: "./schedule-dialog.component.html",
    styleUrls: [
        "../../styles/master.style.scss",
        "../../styles/schedule.style.scss",
    ],
    providers: [PaintTaskMasterService]
})
/** schedule-dialog component*/
export class ScheduleDialogComponent implements OnInit {
    /** schedule-dialog ctor */
    constructor(
        private service: PaintTaskMasterService,
        public dialogRef: MatDialogRef<ScheduleDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public MasterId: number
    ) {}

    //@param
    columnsUpper: Array<any>;
    columnsLower: Array<any>;
    columns: Array<any>;
    taskPaintDetails: Array<any>;
    option: OptionTaskMasterSchedule;
    totalRecords: number;

    /** Called by Angular after project-dialog component initialized */
    ngOnInit(): void {
        this.taskPaintDetails = new Array;

        if (this.MasterId) {
            this.dialogRef.afterClosed();
        }

        this.option = {
            TaskMasterId: this.MasterId
        };

        this.onGetTaskMasterSchedule();
    }

    // get task master schedule data
    onGetTaskMasterSchedule(): void {
        this.service.getOnlyMasterSchedule(this.option)
            .subscribe(dbDataSchedule => {
                this.totalRecords = dbDataSchedule.TotalRow;

                this.columns = new Array;
                this.columnsUpper = new Array;

                let ProMasterWidth: string = "170px";
                let WorkItemWidth: string = "350px";
                let ProgressWidth: string = "100px";

                // column Row1
                this.columnsUpper.push({ header: "TaskType", rowspan: 2, style: { "width": ProMasterWidth, } });
                this.columnsUpper.push({ header: "Blast/Paint Detail", rowspan: 2, style: { "width": WorkItemWidth, } });
                this.columnsUpper.push({ header: "Progress", rowspan: 2, style: { "width": ProgressWidth, } });

                for (let month of dbDataSchedule.ColumnsTop) {
                    this.columnsUpper.push({
                        header: month.Name,
                        colspan: month.Value,
                        style: { "width": (month.Value * 35).toString() + "px", }
                    });
                }
                // column Row 2
                this.columnsLower = new Array;

                for (let name of dbDataSchedule.ColumnsLow) {
                    this.columnsLower.push({
                        header: name,
                        // style: { "width": "25px" }
                    });
                }

                // column Main
                this.columns = new Array;
                this.columns.push({ header: "TaskType", field: "TaskType", style: { "width": ProMasterWidth, } });
                this.columns.push({ header: "Blast/Paint Detail", field: "WorkItem", style: { "width": WorkItemWidth, } });
                this.columns.push({ header: "Progress", field: "Progress", style: { "width": ProgressWidth, } });
                // column Sub
                let i: number = 0;
                for (let name of dbDataSchedule.ColumnsAll) {
                    if (name.indexOf("Col") >= -1) {
                        this.columns.push({
                            header: this.columnsLower[i], field: name, style: { "width": "35px" }, isCol: true,
                        });
                        i++;
                    }
                }

                this.taskPaintDetails = dbDataSchedule.DataTable.slice();
            }, error => {
                this.columns = new Array;
                this.taskPaintDetails = new Array;
            });
    }

    // load Data Lazy
    loadDataLazy(event: LazyLoadEvent): void {
        // in a real application, make a remote request to load data using state metadata from event
        // event.first = First row offset
        // event.rows = Number of rows per page
        // event.sortField = Field name to sort with
        // event.sortOrder = Sort order as number, 1 for asc and -1 for dec
        // filters: FilterMetadata object having field as key and filter value, filter matchMode as value

        // imitate db connection over a network
        this.option.Skip = event.first;
        this.option.Take = (event.rows || 5);
    }

    // no Click
    onCancelClick(): void {
        this.dialogRef.close();
    }
}