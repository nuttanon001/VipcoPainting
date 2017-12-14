// angular
import { Component, Inject, ViewChild, OnDestroy } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material";
// models
import { Color, Scroll } from "../../models/model.index";
// service
import { ColorService } from "../../services/color/color.service";
import { DataTableServiceCommunicate } from "../../services/data-table/data-table.service";
// rxjs
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";
// base-component
import { BaseDialogComponent } from "../base-component/base-dialog.component";

@Component({
    selector: "coloritem-dialog",
    templateUrl: "./coloritem-dialog.component.html",
    styleUrls: ["../../styles/master.style.scss"],
    providers: [
        ColorService,
        DataTableServiceCommunicate
    ]
})
// color-item-dialog component */
export class ColorItemDialogComponent
    extends BaseDialogComponent<Color, ColorService> implements OnDestroy {
    /** color-item-dialog ctor */
    constructor(
        public service: ColorService,
        public serviceDataTable: DataTableServiceCommunicate<Color>,
        public dialogRef: MatDialogRef<ColorItemDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public mode: number
    ) {
        super(
            service,
            serviceDataTable,
            dialogRef
        );

        this.columns = [
            { prop: "ColorCode", name: "Code", flexGrow: 1 },
            { prop: "ColorName", name: "Name", flexGrow: 1 },
            { prop: "VolumeSolids", name: "VolumeSolids", flexGrow: 1 },
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