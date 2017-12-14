// angular
import { Component, Inject, ViewChild, OnDestroy } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material";
// models
import { SurfaceType, Scroll } from "../../models/model.index";
// service
import { SurfaceTypeService } from "../../services/surface-type/surface-type.service";
import { DataTableServiceCommunicate } from "../../services/data-table/data-table.service";
// rxjs
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";
// base-component
import { BaseDialogComponent } from "../base-component/base-dialog.component";

@Component({
    selector: "surface-type-dialog",
    templateUrl: "./surface-type-dialog.component.html",
    styleUrls: ["../../styles/master.style.scss"],
    providers: [
        SurfaceTypeService,
        DataTableServiceCommunicate
    ]
})
/** surface-type-dialog component*/
export class SurfaceTypeDialogComponent 
    extends BaseDialogComponent<SurfaceType, SurfaceTypeService> implements OnDestroy {

    /** surface-type-dialog ctor */
    constructor(
        public service: SurfaceTypeService,
        public serviceDataTable: DataTableServiceCommunicate<SurfaceType>,
        public dialogRef: MatDialogRef<SurfaceTypeDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public mode: number
    ) {
        super(
            service,
            serviceDataTable,
            dialogRef
        );

        this.columns = [
            { prop: "SurfaceCode", name: "Code", flexGrow: 1 },
            { prop: "SurfaceName", name: "Description", flexGrow: 1 },
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