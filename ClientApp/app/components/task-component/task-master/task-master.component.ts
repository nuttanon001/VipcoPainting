import { Component, ViewContainerRef } from "@angular/core";
import { Router, ActivatedRoute, ParamMap } from "@angular/router";
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
        private router: Router,
        private route: ActivatedRoute,
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
        { prop: "ProjectCodeSubString", name: "Job", flexGrow: 1, },
        { prop: "AssignByString", name: "AssingBy", flexGrow: 1 },

    ];

    // on inti override
    ngOnInit(): void {
        // override class
        super.ngOnInit();

        this.route.paramMap.subscribe((param: ParamMap) => {
            let key: number = Number(param.get("condition") || 0);

            if (key) {
                let newTaskMaster: TaskMaster = {
                    TaskMasterId: 0,
                    RequirePaintingListId : key
                };
                setTimeout(() => {
                    this.onDetailEdit(newTaskMaster);
                }, 500);
            }
        }, error => console.error(error));
    }

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

            // TaskBlastDetail
            if (value.TaskBlastDetails) {
                value.TaskBlastDetails.forEach((item, index) => {
                    if (item.CreateDate !== null) {
                        item.CreateDate = moment.tz(item.CreateDate, zone).toDate();
                    }
                    if (item.ModifyDate !== null) {
                        item.ModifyDate = moment.tz(item.ModifyDate, zone).toDate();
                    }
                    // can't update FromBody with same data from webapi try new object and send back update
                    if (value.TaskBlastDetails) {
                        let newData: TaskBlastDetail = {
                            TaskBlastDetailId: item.TaskBlastDetailId,
                            Remark: item.Remark,
                            Creator: item.Creator,
                            CreateDate: item.CreateDate,
                            Modifyer: item.Modifyer,
                            ModifyDate: item.ModifyDate,
                            //FK
                            TaskMasterId: item.TaskMasterId,
                            BlastRoomId: item.BlastRoomId,
                            BlastWorkItemId: item.BlastWorkItemId,
                        };
                        value.TaskBlastDetails[index] = newData;
                    }
                });
            }

            // TaskPaintDetail
            if (value.TaskPaintDetails) {
                value.TaskPaintDetails.forEach((item, index) => {
                    if (item.CreateDate !== null) {
                        item.CreateDate = moment.tz(item.CreateDate, zone).toDate();
                    }
                    if (item.ModifyDate !== null) {
                        item.ModifyDate = moment.tz(item.ModifyDate, zone).toDate();
                    }
                    // can't update FromBody with same data from webapi try new object and send back update
                    if (value.TaskPaintDetails) {
                        let newData: TaskPaintDetail = {
                            TaskPaintDetailId: item.TaskPaintDetailId,
                            Remark: item.Remark,
                            Creator: item.Creator,
                            CreateDate: item.CreateDate,
                            Modifyer: item.Modifyer,
                            ModifyDate: item.ModifyDate,
                            //FK
                            TaskMasterId: item.TaskMasterId,
                            PaintTeamId: item.PaintTeamId,
                            PaintWorkItemId: item.PaintWorkItemId,
                        };
                        value.TaskPaintDetails[index] = newData;
                    }
                });
            }
        }
        return value;
    }

    // on insert data
    onInsertToDataBase(value: TaskMaster): void {
        let tempValue: TaskMaster = Object.assign({}, value);

        if (this.serverAuth.getAuth) {
            tempValue.Creator = this.serverAuth.getAuth.UserName || "";
        }
        // change timezone
        tempValue = this.changeTimezone(tempValue);
        // insert data
        this.service.post(tempValue).subscribe(
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
        let tempValue: TaskMaster = Object.assign({}, value);

        if (this.serverAuth.getAuth) {
            tempValue.Modifyer = this.serverAuth.getAuth.UserName || "";
        }
        // change timezone
        tempValue = this.changeTimezone(tempValue);
        // update data
        this.service.putKeyNumber(tempValue, tempValue.TaskMasterId).subscribe(
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