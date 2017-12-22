import { Component, OnInit, OnDestroy, ViewContainerRef, ViewEncapsulation } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import {
    trigger, state, style,
    animate, transition
} from "@angular/animations";
// rxjs
import { Observable } from "rxjs/Rx";
import { Subscription } from "rxjs/Subscription";
// model
import { RequirePaintMaster } from "../../../models/model.index";
// service
import { RequirePaintMasterService } from "../../../services/require-paint/require-paint-master.service";
import { DialogsService } from "../../../services/dialog/dialogs.service";

@Component({
    selector: "require-painting-schedule",
    templateUrl: "./require-painting-schedule.component.html",
    styleUrls: ["../../../styles/schedule.style.scss"],
    animations: [
        trigger("flyInOut", [
            state("in", style({ transform: "translateX(0)" })),
            transition("void => *", [
                style({ transform: "translateX(-100%)" }),
                animate(250)
            ]),
            transition("* => void", [
                animate("0.2s 0.1s ease-out", style({ opacity: 0, transform: "translateX(100%)" }))
            ])
        ])
    ]
})
// require-painting-schedule component*/
export class RequirePaintingScheduleComponent implements OnInit, OnDestroy {
    /** require-painting-schedule ctor */
    constructor(
        private service: RequirePaintMasterService,
        private serviceDialogs: DialogsService,
        private viewContainerRef: ViewContainerRef,
        private router: Router
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
        this.onGetData();
    }

    // destroy
    ngOnDestroy(): void {
        if (this.subscription) {
            // prevent memory leak when component destroyed
            this.subscription.unsubscribe();
        }
    }

    // get request data
    onGetData(): void {
        this.service.getRequirePaintingMasterHasWait()
            .subscribe(dbRequirePainting => {
                this.columns = new Array<any>();

                for (let name of dbRequirePainting.Columns) {
                    if (name.indexOf("Employee") >= 0) {
                        this.columns.push({
                            field: name, header: name,
                            style: { "width": "150px", "text-align": "center" }, styleclass: "time-col"
                        });
                    } else if (name.indexOf("JobNumber") >= 0) {
                        this.columns.push({
                            field: name, header: name,
                            style: { "width": "150px", "text-align": "center" }, styleclass: "type-col"
                        });
                    } else {
                        this.columns.push({
                            field: name, header: name,
                            style: { "width": "270px" }, styleclass: "singleLine", isButton: true
                        });
                    }
                }

                // debug here
                this.requirePaintings = dbRequirePainting.DataTable;
                this.reloadData();
            }, error => {
                this.columns = new Array<any>();
                this.requirePaintings = new Array<any>();
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
                    this.onGetData();
                }
            });
    }

    // selected request
    onSelectData(data: any): void {
        let splitArray: Array<string> = data.split("#");
        if (splitArray.length > 0) {
            this.service.postGetMultipleKey(splitArray).subscribe(dbData => {
                //this.serviceDialogs.dialogSelectedJobCardDetailForWait(this.viewContainerRef, data)
                //    .subscribe(requirePaintMaster => {
                //        if (requirePaintMaster) {
                //            this.router.navigate(["task-blast&paint/task-blast&paint-detail/", requirePaintMaster.JobCardDetailId]);
                //        }
                //    });
                // wait for dev
            }, error => this.serviceDialogs.error("Error Message", "Can't found key !!!", this.viewContainerRef));
        } else {
            this.serviceDialogs.error("Error Message", "Can't found key !!!", this.viewContainerRef);
        }
    }
}