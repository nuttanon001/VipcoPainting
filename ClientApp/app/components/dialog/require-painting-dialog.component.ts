// angular
import { Component, OnInit, OnDestroy, Inject, ViewChild } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material";
// models
import { BlastWorkItem, PaintWorkItem } from "../../models/model.index";
// service
import { BlastWorkitemService } from "../../services/require-paint/blast-workitem.service";
import { PaintWorkitemService } from "../../services/require-paint/paint-workitem.service";
// rxjs
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";
// base-component
import { BaseDialogComponent } from "../base-component/base-dialog.component";
import { TableColumn } from "@swimlane/ngx-datatable";

@Component({
    selector: "require-painting-dialog",
    templateUrl: "./require-painting-dialog.component.html",
    styleUrls: ["../../styles/master.style.scss"],
    providers: [
        BlastWorkitemService,
        PaintWorkitemService,
    ]
})
// require-painting-dialog component*/
export class RequirePaintingDialogComponent implements OnInit, OnDestroy {
    /** require-painting-dialog ctor */
    constructor(
        private serviceBlast: BlastWorkitemService,
        private servicePaint: PaintWorkitemService,
        public dialogRef: MatDialogRef<RequirePaintingDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public requirePaintListId: number
    ) { }

    // Parameter
    columnsBlastWork: Array<TableColumn> = [
        { prop: "IntArea", name: "Int Area", width: 70 },
        { prop: "IntSurfaceTypeString", name: "Int Surface", width: 120 },
        { prop: "IntStandradTimeString", name: "Internal Std", width: 200 },
        { prop: "ExtArea", name: "Ext Area", width: 70 },
        { prop: "ExtSurfaceTypeString", name: "Ext Surface", width: 120 },
        { prop: "ExtStandradTimeString", name: "External Std", width: 200 },
    ];

    columnsPaintWork: Array<TableColumn> = [
        { prop: "PaintLevelString", name: "Level", width: 120 },
        { prop: "IntAreaString", name: "Int Area", width: 150 },
        { prop: "IntColorString", name: "Int Color", width: 120 },
        { prop: "IntStandradTimeString", name: "Int Std", width: 120 },

        { prop: "ExtAreaString", name: "Ext Area", width: 150 },
        { prop: "ExtColorString", name: "Ext Color", width: 120 },
        { prop: "ExtStandradTimeString", name: "Ext Std", width: 120 },
    ];

    blastWorkItems: Array<BlastWorkItem>;
    paintWorkItems: Array<PaintWorkItem>;

    /** Called by Angular after project-dialog component initialized */
    ngOnInit(): void {
        if (!this.blastWorkItems) {
            this.blastWorkItems = new Array;
        }

        if (!this.paintWorkItems) {
            this.paintWorkItems = new Array;
        }
        // Load-Data
        this.loadData(this.requirePaintListId);
    }

    // angular hook
    ngOnDestroy(): void {}

    // on get data with lazy load
    loadData(requirePaintListId: number): void {
        this.serviceBlast.getByMasterId(requirePaintListId)
            .subscribe(dbBlast => {
                this.blastWorkItems = dbBlast.slice();
            });

        this.servicePaint.getByMasterId(requirePaintListId)
            .subscribe(dbPaint => {
                this.paintWorkItems = dbPaint.slice();
            });
    }

    // no Click
    onCancelClick(): void {
        this.dialogRef.close();
    }
}