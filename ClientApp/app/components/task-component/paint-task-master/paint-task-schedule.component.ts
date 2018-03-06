import { Component, OnInit, OnDestroy, ViewContainerRef, ViewEncapsulation } from "@angular/core";
import { FormBuilder, FormGroup, FormControl, Validators } from "@angular/forms";
import { Router, ActivatedRoute, ParamMap } from "@angular/router";
// rxjs
import { Observable } from "rxjs/Rx";
import { Subscription } from "rxjs/Subscription";
// model
import {
    PaintTaskMaster, OptionTaskMasterSchedule,
    ProjectMaster, ProjectSub, Employee, PaintTaskDetail,
} from "../../../models/model.index";
// service
import { DialogsService } from "../../../services/dialog/dialogs.service";
import { AuthService } from "../../../services/auth/auth.service";
import { PaintTaskMasterService, PaintTaskMasterServiceCommunicate } from "../../../services/paint-task/paint-task-master.service";
// 3rd patry
import { Column, SelectItem, LazyLoadEvent } from "primeng/primeng";
// timezone
import * as moment from "moment-timezone";
@Component({
    selector: "paint-task-schedule",
    templateUrl: "./paint-task-schedule.component.html",
    providers: [PaintTaskMasterServiceCommunicate],
    styleUrls: [
        "../../../styles/schedule.style.scss",
        "../../../styles/master.style.scss"
    ],
})
/** paint-task-schedule component*/
export class PaintTaskScheduleComponent implements OnInit, OnDestroy {
    /** paint-task-schedule ctor */
    constructor(
        private service: PaintTaskMasterService,
        private serviceCom: PaintTaskMasterServiceCommunicate,
        private serviceDialogs: DialogsService,
        private serviceAuth: AuthService,
        private viewContainerRef: ViewContainerRef,
        private fb: FormBuilder,
        private router: Router,
        private route: ActivatedRoute) { }

    // Parameter
    // form
    reportForm: FormGroup;
    // model
    columnsUpper: Array<any>;
    columnsLower: Array<any>;
    columns: Array<any>;
    taskMasters: Array<any>;
    //
    scrollHeight: string;
    subscription: Subscription;
    subscription1: Subscription;
    // time
    message: number = 0;
    count: number = 0;
    time: number = 300;
    totalRecords: number;
    // mode
    mode: number | undefined;
    schedule: OptionTaskMasterSchedule;
    taskMasterId: number | undefined;
    taskMasterEdit: PaintTaskMaster | undefined;
    canSave: boolean = false;
    // report
    loadReportPaint: boolean;
    PaintTaskDetailId?: number;
    ReportType?: string;
    // angular hook
    ngOnInit(): void {
        this.loadReportPaint = false;
        this.ReportType = "";

        if (window.innerWidth >= 1600) {
            this.scrollHeight = 75 + "vh";
        } else if (window.innerWidth > 1360 && window.innerWidth < 1600) {
            this.scrollHeight = 68 + "vh";
        } else {
            this.scrollHeight = 65 + "vh";
        }

        this.taskMasters = new Array;

        this.route.paramMap.subscribe((param: ParamMap) => {
            let key: number = Number(param.get("condition") || 0);

            // debug here
            // console.log("Mode is", key);

            if (key) {
                this.mode = key;

                let schedule: OptionTaskMasterSchedule = {
                    Mode: this.mode
                };

                if (this.serviceAuth.getAuth) {
                    if (this.mode === 1) {
                        schedule.Creator = this.serviceAuth.getAuth.UserName;
                        schedule.CreatorName = this.serviceAuth.getAuth.NameThai;
                    }
                }

                this.buildForm(schedule);

                if (this.reportForm) {
                    this.onValueChanged();
                }
            }
        }, error => console.error(error));

        this.subscription1 = this.serviceCom.ToParent$.subscribe(
            (TypeValue: [PaintTaskMaster, boolean]) => {
                this.taskMasterEdit = TypeValue[0];
                this.canSave = TypeValue[1];
            });
    }

    // destroy
    ngOnDestroy(): void {
        if (this.subscription) {
            // prevent memory leak when component destroyed
            this.subscription.unsubscribe();
        }

        if (this.subscription1) {
            this.subscription1.unsubscribe();
        }
    }

    // build form
    buildForm(schedule?: OptionTaskMasterSchedule): void {
        if (!schedule) {
            schedule = {
                Mode: this.mode || 2,
            };
        }
        this.schedule = schedule;

        this.reportForm = this.fb.group({
            Filter: [this.schedule.Filter],
            ProjectMasterId: [this.schedule.ProjectMasterId],
            ProjectSubId: [this.schedule.ProjectSubId],
            ProjectSubString: [this.schedule.ProjectSubString],
            Mode: [this.schedule.Mode],
            Skip: [this.schedule.Skip],
            Take: [this.schedule.Take],
            TaskMasterId: [this.schedule.TaskMasterId],
            Creator: [this.schedule.Creator],
            // template
            CreatorName: [this.schedule.CreatorName],
        });

        this.reportForm.valueChanges.subscribe((data: any) => this.onValueChanged(data));
        // this.onValueChanged();
    }

    // on value change
    onValueChanged(data?: any): void {
        if (!this.reportForm) { return; }

        this.schedule = this.reportForm.value;
        this.onGetTaskMasterSchedule(this.schedule);
    }

    // get task master schedule data
    onGetTaskMasterSchedule(schedule: OptionTaskMasterSchedule): void {
        if (this.taskMasterId) {
            schedule.TaskMasterId = this.taskMasterId;
        }

        // debug here
        // console.log("Get Data");

        if (this.mode) {
            if (this.mode > 1) {
                // debug here
                // console.log("For Paint");

                this.service.getTaskMasterSchedule(schedule)
                    .subscribe(dbDataSchedule => {
                        this.onSetupDataTable(dbDataSchedule);
                    }, error => {
                        this.columns = new Array;
                        this.taskMasters = new Array;
                        this.reloadData();
                    });
                return;
            }
        }
        // debug here
        // console.log("For All");

        this.service.getTaskMasterScheduleV2(schedule)
            .subscribe(dbDataSchedule => {
                this.onSetupDataTable(dbDataSchedule);
            }, error => {
                this.columns = new Array;
                this.taskMasters = new Array;
                this.reloadData();
            });
        
    }

    // on setup datatable
    onSetupDataTable(dbDataSchedule: any): void {
        this.totalRecords = dbDataSchedule.TotalRow;

        this.columns = new Array;
        this.columnsUpper = new Array;

        let ProMasterWidth: string = "170px";
        let WorkItemWidth: string = "350px";
        let ProgressWidth: string = "100px";

        // column Row1
        this.columnsUpper.push({ header: "JobNo", rowspan: 2, style: { "width": ProMasterWidth, } });
        this.columnsUpper.push({ header: "WorkItem | MarkNo | UnitNo", rowspan: 2, style: { "width": WorkItemWidth, } });
        this.columnsUpper.push({ header: "Progress", rowspan: 2, style: { "width": ProgressWidth, } });

        for (let month of dbDataSchedule.ColumnsTop) {
            this.columnsUpper.push({
                header: month.Name,
                colspan: month.Value,
                style: { "width": (month.Value * 35).toString() + "px", }
            });
        }
        // column Row2
        this.columnsLower = new Array;

        for (let name of dbDataSchedule.ColumnsLow) {
            this.columnsLower.push({
                header: name,
                // style: { "width": "25px" }
            });
        }

        // column Main
        this.columns = new Array;
        this.columns.push({ header: "JobNo", field: "ProjectMaster", style: { "width": ProMasterWidth, } });

        this.columns.push({
            header: "WorkItem | MarkNo | UnitNo", field: "WorkItem",
            style: { "width": WorkItemWidth, }, isLink: true
        });

        this.columns.push({ header: "Progress", field: "Progress", style: { "width": ProgressWidth, } });

        let i: number = 0;
        for (let name of dbDataSchedule.ColumnsAll) {
            if (name.indexOf("Col") >= -1) {
                this.columns.push({
                    header: this.columnsLower[i], field: name, style: { "width": "35px" }, isCol: true,
                });
                i++;
            }
        }

        this.taskMasters = dbDataSchedule.DataTable.slice();

        this.reloadData();
    }

    // reload data
    reloadData(): void {
        if (this.subscription) {
            this.subscription.unsubscribe();
        }
        this.subscription = Observable.interval(1000)
            .take(this.time).map((x) => x + 1)
            .subscribe((x) => {
                this.message = this.time - x;
                this.count = (x / this.time) * 100;
                if (x === this.time) {
                    if (this.reportForm.value) {
                        ;
                        this.onGetTaskMasterSchedule(this.reportForm.value);
                    }
                }
            });
    }

    // reset
    resetFilter(): void {
        this.taskMasters = new Array;
        this.buildForm();

        this.reportForm.patchValue({
            Skip: 0,
            Take: 5,
        });

        this.onGetTaskMasterSchedule(this.reportForm.value);
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

        this.reportForm.patchValue({
            Skip: event.first,
            // mark Take: ((event.first || 0) + (event.rows || 4)),
            Take: (event.rows || 5),
        });
    }

    // on select dialog
    onShowDialog(type?: string): void {
        if (type) {
            if (type === "Employee") {
                this.serviceDialogs.dialogSelectEmployee(this.viewContainerRef)
                    .subscribe(emp => {
                        // console.log(emp);
                        if (emp) {
                            this.reportForm.patchValue({
                                Creator: emp.EmpCode,
                                CreatorName: `คุณ${emp.NameThai}`,
                            });
                        }
                    });
            } else if (type === "Project") {
                this.serviceDialogs.dialogSelectProject(this.viewContainerRef)
                    .subscribe(project => {
                        if (project) {
                            this.reportForm.patchValue({
                                ProjectSubId: project.ProjectCodeSubId,
                                ProjectSubString: `${project.ProjectMasterString}/${project.Code}`,
                            });
                        }
                    });
            }
        }
    }

    // on update progress
    onSelectTaskMasterId(TaskMasterId?: number): void {
        if (TaskMasterId && this.mode) {
            if (this.mode > 1) {
                if (TaskMasterId) {
                    this.service.getOneKeyNumber(TaskMasterId)
                        .subscribe(dbData => {
                            this.taskMasterEdit = dbData;
                            setTimeout(() => this.serviceCom.toChildEdit(dbData), 1000);
                        });
                }              
            } else {
                if (TaskMasterId) {
                    let option: OptionTaskMasterSchedule = {
                        TaskMasterId: TaskMasterId
                    }
                    this.serviceDialogs.dialogTaskPaintMasterScheduleView(this.viewContainerRef, option);
                } else {
                    this.serviceDialogs.error("Warning Message", "This workitem not plan yet.",
                        this.viewContainerRef);
                }   
            }
        } else {
            this.serviceDialogs.error("Warning Message", "This workitem not plan yet.",
                this.viewContainerRef);
        }   
    }

    // on cancel edit
    onCancelEdit(): void {
        this.taskMasterEdit = undefined;
        this.canSave = false;
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
                            PaymentDetailId: item.PaymentDetailId,
                        };
                        value.PaintTaskDetails[index] = newData;
                    }
                });
            }
        }
        return value;
    }

    // on update data
    onUpdateToDataBase(): void {
        if (this.taskMasterEdit) {
            let tempValue: PaintTaskMaster = Object.assign({}, this.taskMasterEdit);

            if (this.serviceAuth.getAuth) {
                tempValue.Modifyer = this.serviceAuth.getAuth.UserName || "";
            }
            // change timezone
            tempValue = this.changeTimezone(tempValue);
            // update data
            this.service.putKeyNumber(tempValue, tempValue.PaintTaskMasterId).subscribe(
                (complete: any) => {
                    this.serviceDialogs
                        .context("System message", "Save completed.", this.viewContainerRef)
                        .subscribe(result => {
                            this.onCancelEdit();
                            this.onGetTaskMasterSchedule(this.reportForm.value);
                        });
                },
                (error: any) => {
                    this.canSave = true;
                    this.serviceDialogs.error("Failed !",
                        "Save failed with the following error: Invalid Identifier code !!!", this.viewContainerRef);
                }
            );
        }
    }

    // on show report
    onShowReportPaint(PaintTaskDetailId?: number,type?:string): void {
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
            if (this.taskMasterEdit) {
                this.serviceCom.toChildEdit(this.taskMasterEdit);
            }}, 500);
    }
}