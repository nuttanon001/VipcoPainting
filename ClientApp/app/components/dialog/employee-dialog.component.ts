// angular
import { Component, Inject, ViewChild, OnDestroy } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material";
// models
import { Employee, Scroll } from "../../models/model.index";
// service
import { EmployeeService } from "../../services/employee/employee.service";
import { DataTableServiceCommunicate } from "../../services/data-table/data-table.service";
// rxjs
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";
// base-component
import { BaseDialogComponent } from "../base-component/base-dialog.component";
@Component({
    selector: "employee-dialog",
    templateUrl: "./employee-dialog.component.html",
    styleUrls: ["../../styles/master.style.scss"],
    providers: [
        EmployeeService,
        DataTableServiceCommunicate
    ]
})
/** employee-dialog component*/
export class EmployeeDialogComponent 
    extends BaseDialogComponent<Employee, EmployeeService> implements OnDestroy {
    /** employee-dialog ctor */
    constructor(
        public service: EmployeeService,
        public serviceDataTable: DataTableServiceCommunicate<Employee>,
        public dialogRef: MatDialogRef<EmployeeDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public mode: number
    ) {
        super(
            service,
            serviceDataTable,
            dialogRef
        );

        this.columns = [
            { prop: "EmpCode", name: "Code", flexGrow: 1 },
            { prop: "NameThai", name: "Name", flexGrow: 1 },
            { prop: "GroupName", name: "Group", flexGrow: 1 },
        ];
    }

    // on init
    onInit(): void {
        this.fastSelectd = true;
    }

    // on get data with lazy load
    loadDataScroll(scroll: Scroll): void {
        if (this.mode) {
            scroll.Where = this.mode.toString();
        }
        this.service.getAllWithScroll(scroll)
            .subscribe(scrollData => {
                if (scrollData) {
                    this.serviceDataTable.toChild(scrollData);
                }
            }, error => console.error(error));
    }

    // on destory
    ngOnDestroy(): void {
        if (this.subscription) {
            this.subscription.unsubscribe();
        }
    }
}