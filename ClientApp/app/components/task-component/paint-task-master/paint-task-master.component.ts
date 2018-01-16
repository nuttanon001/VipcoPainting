import { Component, ViewContainerRef } from "@angular/core";
import { Router, ActivatedRoute, ParamMap } from "@angular/router";
// components
import { BaseMasterComponent } from "../../base-component/base-master.component";
// models
import {
    PaintTaskMaster, PaintTaskDetail,
    Scroll, ScrollData
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
import { PaintTaskMasterService,PaintTaskMasterServiceCommunicate } from "../../../services/paint-task/paint-task-master.service";

@Component({
    selector: "paint-task-master",
    templateUrl: "./paint-task-master.component.html",
    styleUrls: ["../../../styles/master.style.scss"],
    providers: [DataTableServiceCommunicate]
})
/** paint-task-master component*/
export class PaintTaskMasterComponent extends BaseMasterComponent<PaintTaskMaster, PaintTaskMasterService> {
    /** paint-task-master ctor */
    constructor(
        service: PaintTaskMasterService,
        serviceCom: PaintTaskMasterServiceCommunicate,
        serviceComDataTable: DataTableServiceCommunicate<PaintTaskMaster>,
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
        { prop: "TaskPaintNo", name: "TaskNo", flexGrow: 1 },
        { prop: "ProjectCodeSubString", name: "Job", flexGrow: 1, },
        { prop: "AssignByString", name: "AssingBy", flexGrow: 1 },

    ];
    // report
    loadReportPaint: boolean;
    PaintTaskDetailId?: number;
    ReportType?: string;

    // on inti override
    ngOnInit(): void {
        // override class
        super.ngOnInit();

        this.route.paramMap.subscribe((param: ParamMap) => {
            let key: number = Number(param.get("condition") || 0);

            if (key) {
                let newTaskMaster: PaintTaskMaster = {
                    PaintTaskMasterId: 0,
                    PaintTaskStatus: 1,
                    RequirePaintingListId: key
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
            .subscribe((scrollData: ScrollData<PaintTaskMaster>) => {
                // console.log(scrollData);

                if (scrollData) {
                    this.dataTableServiceCom.toChild(scrollData);
                }
            }, error => console.error(error));
    }

    // on change time zone befor update to webapi
    changeTimezone(value: PaintTaskMaster): PaintTaskMaster {
        let zone: string = "Asia/Bangkok";
        if (value !== null) {
            if (value.CreateDate !== null) {
                value.CreateDate = moment.tz(value.CreateDate, zone).toDate();
            }
            if (value.ModifyDate !== null) {
                value.ModifyDate = moment.tz(value.ModifyDate, zone).toDate();
            }
            if (value.AssignDate !== null) {
                value.AssignDate = moment.tz(value.AssignDate, zone).toDate();
            }

            // TaskBlastDetail
            if (value.PaintTaskDetails) {
                value.PaintTaskDetails.forEach((item, index) => {
                    if (item.CreateDate !== null && item.CreateDate !== undefined) {
                        item.CreateDate = moment.tz(item.CreateDate, zone).toDate();
                    }
                    if (item.ModifyDate !== null && item.ModifyDate !== undefined) {
                        item.ModifyDate = moment.tz(item.ModifyDate, zone).toDate();
                    }
                    if (item.ActualEDate !== null && item.ActualEDate !== undefined) {
                        item.ActualEDate = moment.tz(item.ActualEDate, zone).toDate();
                    }
                    if (item.ActualSDate !== null && item.ActualSDate !== undefined) {
                        item.ActualSDate = moment.tz(item.ActualSDate, zone).toDate();
                    }
                    if (item.PlanEDate !== null && item.PlanEDate !== undefined) {
                        item.PlanEDate = moment.tz(item.PlanEDate, zone).toDate();
                    }
                    if (item.PlanSDate !== null && item.PlanSDate !== undefined) {
                        item.PlanSDate = moment.tz(item.PlanSDate, zone).toDate();
                    }

                    if (value.PaintTaskDetails) {
                        let newData: PaintTaskDetail = {
                            PaintTaskDetailId: item.PaintTaskDetailId,
                            Remark: item.Remark,
                            PaintTaskDetailStatus: item.PaintTaskDetailStatus,
                            PaintTaskDetailType: item.PaintTaskDetailType,
                            PaintTaskDetailLayer: item.PaintTaskDetailLayer,
                            PlanSDate: item.PlanSDate,
                            PlanEDate: item.PlanEDate,
                            ActualSDate: item.ActualSDate,
                            ActualEDate: item.ActualEDate,
                            TaskDetailProgress: item.TaskDetailProgress,
                            //BaseModel
                            Creator: item.Creator,
                            CreateDate: item.CreateDate,
                            Modifyer: item.Modifyer,
                            ModifyDate: item.ModifyDate,
                            //FK
                            PaintTaskMasterId: item.PaintTaskMasterId,
                            //PaintTeam
                            PaintTeamId: item.PaintTeamId,
                            //BlastRoom
                            BlastRoomId: item.BlastRoomId,
                            //PaintWorkItem
                            PaintWorkItemId: item.PaintWorkItemId,
                            //BlastWorkItem
                            BlastWorkItemId: item.BlastWorkItemId,
                            //PaymentDetail
                            PaymentDetailId:item.PaymentDetailId,
                        };
                        value.PaintTaskDetails[index] = newData;
                    }
                });
            }
        }
        return value;
    }

    // on insert data
    onInsertToDataBase(value: PaintTaskMaster): void {
        if (this.serverAuth.getAuth) {
            value.Creator = this.serverAuth.getAuth.UserName || "";
        }
        // change timezone
        // console.log("Befor Value is:", JSON.stringify(value));
        value = this.changeTimezone(value);
        // console.log("Value is:", JSON.stringify(value));

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
    onUpdateToDataBase(value: PaintTaskMaster): void {
        if (this.serverAuth.getAuth) {
            value.Modifyer = this.serverAuth.getAuth.UserName || "";
        }
        // change timezone
        value = this.changeTimezone(value);
        // update data
        this.service.putKeyNumber(value, value.PaintTaskMasterId).subscribe(
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
    onDetailView(value: PaintTaskMaster): void {
        if (this.ShowEdit) {
            return;
        }
        if (value) {
            this.service.getOneKeyNumber(value.PaintTaskMasterId)
                .subscribe(dbData => {
                    this.displayValue = dbData;
                }, error => this.displayValue = undefined);
        } else {
            this.displayValue = undefined;
        }
    }

    // on show report
    onShowReportPaint(PaintTaskDetailId?: number, type?: string): void {
        if (PaintTaskDetailId && type) {
            this.PaintTaskDetailId = PaintTaskDetailId;
            this.loadReportPaint = !this.loadReportPaint;
            this.ReportType = type;
        }
    }

    // on back from report
    onBack(): void {
        this.loadReportPaint = !this.loadReportPaint;
        this.ReportType = "";
        setTimeout(() => {
            if (this.displayValue) {
                this.serviceCom.toChildEdit(this.displayValue);
            }
        }, 500);
    }
}