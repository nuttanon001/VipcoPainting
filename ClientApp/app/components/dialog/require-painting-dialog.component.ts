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
        { prop: "IntArea", name: "IntArea", width: 70 },
        { prop: "IntSurfaceTypeString", name: "IntSurface", width: 120 },
        { prop: "IntStandradTimeString", name: "IntStd", width: 120 },
        { prop: "ExtArea", name: "ExtArea", width: 70 },
        { prop: "ExtSurfaceTypeString", name: "ExtSurface", width: 120 },
        { prop: "ExtStandradTimeString", name: "ExtStd", width: 120 },
    ];

    columnsPaintWork: Array<TableColumn> = [
        { prop: "PaintLevelString", name: "Level", width: 120 },
        { prop: "IntAreaString", name: "IntArea", width: 150 },
        { prop: "IntColorString", name: "IntColor", width: 120 },
        { prop: "IntStandradTimeString", name: "IntStd", width: 120 },

        { prop: "ExtAreaString", name: "ExtArea", width: 150 },
        { prop: "ExtColorString", name: "ExtColor", width: 120 },
        { prop: "ExtStandradTimeString", name: "ExtStd", width: 120 },
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