import { Component, OnInit, OnDestroy, ViewContainerRef, ViewEncapsulation } from "@angular/core";
import { FormBuilder, FormGroup, FormControl, Validators } from "@angular/forms";
import { Router, ActivatedRoute, ParamMap } from "@angular/router";
// rxjs
import { Observable } from "rxjs/Rx";
import { Subscription } from "rxjs/Subscription";
// model
import {
    TaskMaster, OptionTaskMasterSchedule,
    ProjectMaster, ProjectSub, Employee, TaskBlastDetail, TaskPaintDetail
} from "../../../models/model.index";
// service
import { TaskMasterService,TaskMasterServiceCommunicate } from "../../../services/task/task-master.service";
import { DialogsService } from "../../../services/dialog/dialogs.service";
import { AuthService } from "../../../services/auth/auth.service";
// 3rd patry
import { Column, SelectItem, LazyLoadEvent } from "primeng/primeng";
// timezone
import * as moment from "moment-timezone";

@Component({
    selector: "task-schedule",
    templateUrl: "./task-schedule.component.html",
    providers: [TaskMasterServiceCommunicate],
    styleUrls: [
        "../../../styles/schedule.style.scss",
        "../../../styles/master.style.scss"
    ],
})
/** task-schedule component*/
export class TaskScheduleComponent implements OnInit, OnDestroy {
    /** task-schedule ctor */
    constructor(
        private service: TaskMasterService,
        private serviceCom: TaskMasterServiceCommunicate,
        private serviceDialogs: DialogsService,
        private serviceAuth: AuthService,
        private viewContainerRef: ViewContainerRef,
        private fb: FormBuilder,
        private router: Router,
        private route: ActivatedRoute)
    { }

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
    taskMasterEdit: TaskMaster|undefined;
    canSave: boolean = false;
    // angular hook
    ngOnInit(): void {
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

            if (key) {
                this.mode = key;

                let schedule: OptionTaskMasterSchedule = {
                    Mode: this.mode
                };

                if (this.serviceAuth.getAuth) {
                    if (this.mode === 1) {
                        schedule.Creator = this.serviceAuth.getAuth.EmpCode;
                        schedule.CreatorName = this.serviceAuth.getAuth.NameThai;
                    }
                }

                this.buildForm(schedule);
            }
        }, error => console.error(error));

        this.subscription1 = this.serviceCom.ToParent$.subscribe(
            (TypeValue: [TaskMaster, boolean]) => {
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
        this.onValueChanged();
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
        if (this.mode) {
            if (this.mode > 1) {
                // debug here
                console.log("For Paint");
                this.service.getTaskMasterSchedule(schedule)
                    .subscribe(dbDataSchedule => {
                        if (dbDataSchedule) {
                            this.onSetupDataTable(dbDataSchedule);
                        }
                    }, error => {
                        this.columns = new Array;
                        this.taskMasters = new Array;
                        this.reloadData();
                    });

                return;
            }
        }

        console.log("For All");
        this.service.getTaskMasterScheduleV2(schedule)
            .subscribe(dbDataSchedule => {
                if (dbDataSchedule) {
                    this.onSetupDataTable(dbDataSchedule);
                }
            }, error => {
                this.columns = new Array;
                this.taskMasters = new Array;
                this.reloadData();
            });
    }

    // on Setup datatable
    onSetupDataTable(dbDataSchedule: any) {
        this.totalRecords = dbDataSchedule.TotalRow;

        this.columns = new Array;
        this.columnsUpper = new Array;

        let ProMasterWidth: string = "170px";
        let WorkItemWidth: string = "350px";
        let ProgressWidth: string = "100px";

        // column Row1
        this.columnsUpper.push({ header: "ProjectMaster", rowspan: 2, style: { "width": ProMasterWidth, } });
        this.columnsUpper.push({ header: "WorkItem | MarkNo | UnitNo", rowspan: 2, style: { "width": WorkItemWidth, } });
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
        this.columns.push({ header: "ProjectMaster", field: "ProjectMaster", style: { "width": ProMasterWidth, } });

        // debug here
        // console.log("Mode is:", this.mode);

        if (this.mode) {
            if (this.mode > 1) {
                // debug here
                // console.log("Mode is 2:", this.mode);
                this.columns.push({
                    header: "WorkItem | MarkNo | UnitNo", field: "WorkItem",
                    style: { "width": WorkItemWidth, }, isLink: true
                });
            } else {
                // debug here
                // console.log("Mode is 3:", this.mode);
                this.columns.push({ header: "WorkItem | MarkNo | UnitNo", field: "WorkItem", style: { "width": WorkItemWidth, } });
            }
        } else {
            // debug here
            // console.log("Mode is 4:", this.mode);
            this.columns.push({ header: "WorkItem | MarkNo | UnitNo", field: "WorkItem", style: { "width": WorkItemWidth, } });
        }
        this.columns.push({ header: "Progress", field: "Progress", style: { "width": ProgressWidth, } });

        // debug here
        // console.log(JSON.stringify(this.columnsLower));

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
                    if (this.reportForm.value) {;
                        this.onGetTaskMasterSchedule(this.reportForm.value);
                    }
                }
            });
    }

    // reset
    resetFilter(): void {
        this.taskMasters = new Array;
        this.buildForm();
        this.onGetTaskMasterSchedule(this.schedule);
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
                        console.log(emp);
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
        if (TaskMasterId) {
            this.service.getOneKeyNumber(TaskMasterId)
                .subscribe(dbData => {
                    this.taskMasterEdit = dbData;
                    setTimeout(() => this.serviceCom.toChildEdit(dbData), 1000);
                });
           
        }
    }

    // on cancel edit
    onCancelEdit(): void {
        this.taskMasterEdit = undefined;
        this.canSave = false;
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

    // on update data
    onUpdateToDataBase(): void {
        if (this.taskMasterEdit) {
            let tempValue: TaskMaster = Object.assign({}, this.taskMasterEdit);

            if (this.serviceAuth.getAuth) {
                tempValue.Modifyer = this.serviceAuth.getAuth.UserName || "";
            }
            // change timezone
            tempValue = this.changeTimezone(tempValue);
            // update data
            this.service.putKeyNumber(tempValue, tempValue.TaskMasterId).subscribe(
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
}