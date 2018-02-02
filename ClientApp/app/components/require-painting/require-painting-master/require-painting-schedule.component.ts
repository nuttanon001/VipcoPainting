import { Component, OnInit, OnDestroy, ViewContainerRef, ViewEncapsulation } from "@angular/core";
import { FormBuilder, FormGroup, FormControl, Validators } from "@angular/forms";
import { Router, ActivatedRoute } from "@angular/router";
// rxjs
import { Observable } from "rxjs/Rx";
import { Subscription } from "rxjs/Subscription";
// model
import { RequirePaintSchedule,RequirePaintMaster } from "../../../models/model.index";
// 3rd patry
import { Column, SelectItem, LazyLoadEvent } from "primeng/primeng";
// service
import { RequirePaintMasterService } from "../../../services/require-paint/require-paint-master.service";
import { DialogsService } from "../../../services/dialog/dialogs.service";
import { AuthService } from "../../../services/auth/auth.service";


@Component({
    selector: "require-painting-schedule",
    templateUrl: "./require-painting-schedule.component.html",
    styleUrls: ["../../../styles/schedule.style.scss"],
})
// require-painting-schedule component*/
export class RequirePaintingScheduleComponent implements OnInit, OnDestroy {
    /** require-painting-schedule ctor */
    constructor(
        private service: RequirePaintMasterService,
        private serviceDialogs: DialogsService,
        private viewContainerRef: ViewContainerRef,
        private fb: FormBuilder,
        private router: Router,
        private route: ActivatedRoute,
    ) { }

    // Parameter
    // model
    columns: Array<any>;
    requirePaintings: Array<any>;
    requireMaster: RequirePaintMaster;
    scrollHeight: string;
    subscription: Subscription;
    // time
    message: number = 0;
    count: number = 0;
    time: number = 300;
    totalRecords: number;
    // value
    status: number | undefined;
    ProjectString: string;
    schedule: RequirePaintSchedule;
    // form
    reportForm: FormGroup;

    // called by Angular after jobcard-waiting component initialized
    ngOnInit(): void {
        if (window.innerWidth >= 1600) {
            this.scrollHeight = 75 + "vh";
        } else if (window.innerWidth > 1360 && window.innerWidth < 1600) {
            this.scrollHeight = 68 + "vh";
        } else {
            this.scrollHeight = 65 + "vh";
        }

        this.requirePaintings = new Array;
        this.buildForm();
    }

    // destroy
    ngOnDestroy(): void {
        if (this.subscription) {
            // prevent memory leak when component destroyed
            this.subscription.unsubscribe();
        }
    }

    // build form
    buildForm(): void {
        this.schedule = {
            Status: this.status || 1,
        };

        this.reportForm = this.fb.group({
            Filter: [this.schedule.Filter],
            ProjectId: [this.schedule.ProjectId],
            ProjectString: [this.ProjectString],
            SDate: [this.schedule.SDate],
            EDate: [this.schedule.EDate],
            Status: [this.schedule.Status],
            Skip: [this.schedule.Skip],
            Take: [this.schedule.Take],
        });

        this.reportForm.valueChanges.subscribe((data: any) => this.onValueChanged(data));
        // this.onValueChanged();
    }

    // on value change
    onValueChanged(data?: any): void {
        if (!this.reportForm) { return; }
        this.schedule = this.reportForm.value;
        this.onGetData(this.schedule);
    }

    // get request data
    onGetData(schedule: RequirePaintSchedule): void {
        this.service.getRequirePaintSchedule(schedule)
            .subscribe(dbDataSchedule => {
                // console.log("Api Send is", dbDataSchedule);

                this.totalRecords = dbDataSchedule.TotalRow;
                this.columns = new Array;

                let ColJobNumberWidth: string = "150px";
                let ColDateWidth: string = "250px";
                // column Main
                this.columns = new Array;
                this.columns.push({
                    header: "JobNumber", field: "JobNumber",
                    style: { "width": ColJobNumberWidth }, styleclass: "time-col",
                });

                let i: number = 0;
                for (let name of dbDataSchedule.Columns) {
                    this.columns.push({
                        header: name, field: name, style: { "width": ColDateWidth }, isCol: true,
                    });
                }

                this.requirePaintings = dbDataSchedule.DataTable.slice();
                // console.log("OverTime is:", this.overtimeMasters);
                this.reloadData();
            }, error => {
                this.columns = new Array;
                this.requirePaintings = new Array;
                this.reloadData();
            });
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
                        this.onGetData(this.reportForm.value);
                    }
                }
            });
    }
  
    // open dialog
    openDialog(type?: string): void {
        if (type) {
            if (type === "Project") {
                this.serviceDialogs.dialogSelectProject(this.viewContainerRef)
                    .subscribe(project => {
                        if (project) {
                            this.reportForm.patchValue({
                                ProjectId: project.ProjectCodeSubId,
                                ProjectString: `${project.ProjectMasterString}/${project.Code}`,
                            });
                        }
                    });
            }
        }
    }

    // reset
    resetFilter(): void {
        this.requirePaintings = new Array;
        this.buildForm();
        this.onGetData(this.schedule);
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
            Take: (event.rows || 10),
        });
    }

    // on selected data
    onSelectRequirePaintMaster(RequirePaintMasterId?: number): void {
        if (RequirePaintMasterId) {
            this.serviceDialogs.dialogRequestPaintView(this.viewContainerRef, RequirePaintMasterId)
                .subscribe(RequirePaintListId => {
                    if (RequirePaintListId === -99) {
                        this.service.getTryToCloseRequirePaintingMaster(RequirePaintMasterId)
                            .subscribe(result => {
                                if (result.Complate) {
                                    this.serviceDialogs.context("Complate Message",
                                        "This Request-Painti was close.", this.viewContainerRef);
                                }
                            }, error => {
                                this.serviceDialogs.error("Error Message",
                                    "This Request-Painti was Incompleted. some work item has waiting status !!!",
                                    this.viewContainerRef);
                            });
                    }
                    else if (RequirePaintListId) {
                        // Debug here
                        // console.log("RequirePaintListId is:", RequirePaintListId);
                        this.router.navigate(["paint-task/paint-task-master/", RequirePaintListId]);
                    }
                });
        }
    }
}