import { Component, ViewContainerRef } from "@angular/core";
// components
import { BaseMasterComponent } from "../../base-component/base-master.component";
// models
import {
    TaskMaster, TaskBlastDetail,
    TaskPaintDetail, Scroll, ScrollData
} from "../../../models/model.index";
// timezone
import * as moment from "moment-timezone";
// 3rd Party
import { TableColumn } from "@swimlane/ngx-datatable";
import { DateOnlyPipe } from "../../../pipes/date-only.pipe";
// services
import { AuthService } from "../../../services/auth/auth.service";
import { DialogsService } from "../../../services/dialog/dialogs.service";
import { DataTableServiceCommunicate } from "../../../services/data-table/data-table.service";
import { TaskMasterService, TaskMasterServiceCommunicate } from "../../../services/task/task-master.service";


@Component({
    selector: "task-master",
    templateUrl: "./task-master.component.html",
    styleUrls: ["../../../styles/master.style.scss"],
    providers: [DataTableServiceCommunicate]
})
/** task-master component*/
export class TaskMasterComponent extends BaseMasterComponent<TaskMaster, TaskMasterService> {
    /** task-master ctor */
    constructor(
        service: TaskMasterService,
        serviceCom: TaskMasterServiceCommunicate,
        serviceComDataTable: DataTableServiceCommunicate<TaskMaster>,
        dialogsService: DialogsService,
        viewContainerRef: ViewContainerRef,
        private serverAuth: AuthService,
    ) {
        super(
            service,
            serviceCom,
            serviceComDataTable,
            dialogsService,
            viewContainerRef
        );
    }

    //Parameter
    datePipe: DateOnlyPipe = new DateOnlyPipe("it");
    columns: Array<TableColumn> = [
        { prop: "TaskNo", name: "TaskNo", flexGrow: 1 },
        { prop: "ProjectCodeSubString", name: "Job", flexGrow: 2, },
        { prop: "AssignByString", name: "AssingBy", flexGrow: 1 },

    ];

    // on get data with lazy load
    loadPagedData(scroll: Scroll): void {
        if (this.scroll) {
            if (this.scroll.Filter && scroll.Reload) {
                scroll.Filter = this.scroll.Filter;
            }
        }

        this.scroll = scroll;
        this.service.getAllWithScroll(scroll)
            .subscribe((scrollData: ScrollData<TaskMaster>) => {
                // console.log(scrollData);

                if (scrollData) {
                    this.dataTableServiceCom.toChild(scrollData);
                }
            }, error => console.error(error));
    }

    // on change time zone befor update to webapi
    changeTimezone(value: TaskMaster): TaskMaster {
        let zone: string = "Asia/Bangkok";
        if (value !== null) {
            if (value.CreateDate !== null) {
                value.CreateDate = moment.tz(value.CreateDate, zone).toDate();
            }
            if (value.ModifyDate !== null) {
                value.ModifyDate = moment.tz(value.ModifyDate, zone).toDate();
            }
            if (value.ActualEDate !== null) {
                value.ActualEDate = moment.tz(value.ActualEDate, zone).toDate();
            }
            if (value.ActualSDate !== null) {
                value.ActualSDate = moment.tz(value.ActualSDate, zone).toDate();
            }
            if (value.AssignDate !== null) {
                value.AssignDate = moment.tz(value.AssignDate, zone).toDate();
            }
        }
        return value;
    }

    // on insert data
    onInsertToDataBase(value: TaskMaster): void {
        if (this.serverAuth.getAuth) {
            value.Creator = this.serverAuth.getAuth.UserName || "";
        }
        // change timezone
        value = this.changeTimezone(value);
        // insert data
        this.service.post(value).subscribe(
            (complete: any) => {
                this.displayValue = complete;
                this.onSaveComplete();
            },
            (error: any) => {
                console.error(error);
                this.editValue.Creator = undefined;
                this.canSave = true;
                this.dialogsService.error("Failed !",
                    "Save failed with the following error: Invalid Identifier code !!!", this.viewContainerRef);
            }
        );
    }

    // on update data
    onUpdateToDataBase(value: TaskMaster): void {
        if (this.serverAuth.getAuth) {
            value.Modifyer = this.serverAuth.getAuth.UserName || "";
        }
        // change timezone
        value = this.changeTimezone(value);
        // TaskPaintDetail
        if (value.TaskPaintDetails) {
            value.TaskPaintDetails.forEach((taskPaint, index) => {
                // can't update FromBody with same data from webapi try new object and send back update
                if (value.TaskPaintDetails) {
                    let newData: TaskPaintDetail = {
                        TaskPaintDetailId: taskPaint.TaskPaintDetailId,
                        Remark: taskPaint.Remark,
                        Creator: taskPaint.Creator,
                        CreateDate: taskPaint.CreateDate,
                        Modifyer: taskPaint.Modifyer,
                        ModifyDate: taskPaint.ModifyDate,
                        //FK
                        TaskMasterId: taskPaint.TaskMasterId,
                        PaintTeamId: taskPaint.PaintTeamId,
                        PaintWorkItemId: taskPaint.PaintWorkItemId,
                    };
                    value.TaskPaintDetails[index] = newData;
                }
            });
        }
        // TaskBlastDetail
        if (value.TaskBlastDetails) {
            value.TaskBlastDetails.forEach((taskBlast, index) => {
                // can't update FromBody with same data from webapi try new object and send back update
                if (value.TaskBlastDetails) {
                    let newData: TaskBlastDetail = {
                        TaskBlastDetailId: taskBlast.TaskBlastDetailId,
                        Remark: taskBlast.Remark,
                        Creator: taskBlast.Creator,
                        CreateDate: taskBlast.CreateDate,
                        Modifyer: taskBlast.Modifyer,
                        ModifyDate: taskBlast.ModifyDate,
                        //FK
                        TaskMasterId: taskBlast.TaskMasterId,
                        BlastRoomId: taskBlast.BlastRoomId,
                        BlastWorkItemId: taskBlast.BlastWorkItemId,
                    };
                    value.TaskBlastDetails[index] = newData;
                }
            });
        }

        // update data
        this.service.putKeyNumber(value, value.TaskMasterId).subscribe(
            (complete: any) => {
                this.displayValue = complete;
                this.onSaveComplete();
            },
            (error: any) => {
                console.error(error);
                this.canSave = true;
                this.dialogsService.error("Failed !",
                    "Save failed with the following error: Invalid Identifier code !!!", this.viewContainerRef);
            }
        );
    }

    // on detail view
    onDetailView(value: TaskMaster): void {
        if (this.ShowEdit) {
            return;
        }
        if (value) {
            this.service.getOneKeyNumber(value.TaskMasterId)
                .subscribe(dbData => {
                    this.displayValue = dbData;
                }, error => this.displayValue = undefined);
        } else {
            this.displayValue = undefined;
        }
    }
}