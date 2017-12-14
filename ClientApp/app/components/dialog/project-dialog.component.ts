import { Component, OnInit, OnDestroy, Inject, ViewChild } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material";
// models
import {
    ProjectMaster, ProjectSub,
    Scroll
} from "../../models/model.index";
// service
import { DataTableServiceCommunicate } from "../../services/data-table/data-table.service";
import { ProjectMasterService } from "../../services/project/project-master.service";
import { ProjectSubService } from "../../services/project/project-sub.service";
// rxjs
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";
// 3rd party
import { DatatableComponent, TableColumn } from "@swimlane/ngx-datatable";
// pipes
import { DateOnlyPipe } from "../../pipes/date-only.pipe";


@Component({
    selector: "project-dialog",
    templateUrl: "./project-dialog.component.html",
    styleUrls: ["../../styles/master.style.scss"],
    providers: [
        ProjectMasterService,
        ProjectSubService,
        DataTableServiceCommunicate
    ]
})
// project-dialog component*/
export class ProjectDialogComponent
    implements OnInit, OnDestroy {
    details: Array<ProjectSub>;
    templates: Array<ProjectSub>;
    // detail
    selectedDetails: ProjectSub | undefined;
    datePipe: DateOnlyPipe = new DateOnlyPipe("it");
    // subscription
    subscription: Subscription;
    @ViewChild(DatatableComponent) table: DatatableComponent;
    // column
    columns:Array<TableColumn> = [
        { prop: "ProjectCode", name: "Code", flexGrow: 1 },
        { prop: "ProjectName", name: "Description", flexGrow: 1 },
        { prop: "StartDate", name: "Date", pipe: this.datePipe, flexGrow: 1 },
    ];
    columnsDetail: Array<TableColumn> = [
        { prop: "Code", name: "Code", flexGrow: 1 },
        { prop: "Name", name: "Description", flexGrow: 3 },
    ];

    get CanSelected(): boolean {
        return this.selectedDetails !== undefined;
    }

    /** project-dialog ctor */
    constructor(
        private serviceMaster: ProjectMasterService,
        private serviceDetail: ProjectSubService,
        private serviceDataTable: DataTableServiceCommunicate<ProjectMaster>,
        public dialogRef: MatDialogRef<ProjectDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public mode: number
    ) { }

    /** Called by Angular after project-dialog component initialized */
    ngOnInit(): void {
        if (!this.details) {
            this.details = new Array;
        }

        this.subscription = this.serviceDataTable.ToParent$
            .subscribe((scroll: Scroll) => this.loadData(scroll));
    }

    // angular hook
    ngOnDestroy(): void {
        if (this.subscription) {
            this.subscription.unsubscribe();
        }
    }

    // on get data with lazy load
    loadData(scroll: Scroll): void {
        this.serviceMaster.getAllWithScroll(scroll)
            .subscribe(scrollData => {
                if (scrollData) {
                    this.serviceDataTable.toChild(scrollData);
                }
            }, error => console.error(error));
    }

    // selected Project Master
    onSelectedMaster(master?: ProjectMaster): void {
        if (master) {
            if (this.mode === 1) {
                this.dialogRef.close(master);
            } else {
                this.serviceDetail.getByMasterId(master.ProjectCodeMasterId)
                    .subscribe(dbDetail => {
                        this.details = dbDetail.slice();
                        this.templates = [...dbDetail];
                        this.selectedDetails = undefined;
                    });
            }
        }
    }

    // selected Project Detail
    onSelectedDetail(selected?: any): void {
        if (selected) {
            this.selectedDetails = selected.selected[0];
            this.onSelectedClick();
        }
    }

    // on Filter
    onFilter(search: string):void {
        // filter our data
        const temp:Array<ProjectSub> = this.templates.slice().filter((item, index) => {
            let searchStr:string = ((item.Name || "") + (item.Code || "")).toLowerCase();
            return searchStr.indexOf(search.toLowerCase()) !== -1;
        });

        // update the rows
        this.details = temp;
        // whenever the filter changes, always go back to the first page
        this.table.offset = 0;
    }

    // no Click
    onCancelClick(): void {
        this.dialogRef.close();
    }

    // update Click
    onSelectedClick(): void {
        this.dialogRef.close(this.selectedDetails);
    }
}