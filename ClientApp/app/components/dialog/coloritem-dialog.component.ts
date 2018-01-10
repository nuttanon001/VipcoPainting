// angular
import { Component, Inject, ViewChild, OnDestroy } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material";
// models
import { Color, Scroll } from "../../models/model.index";
// service
import { ColorService,ColorServiceCommunicate } from "../../services/color/color.service";
import { DataTableServiceCommunicate } from "../../services/data-table/data-table.service";
// rxjs
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";
// base-component
import { BaseDialogComponent } from "../base-component/base-dialog.component";

@Component({
    selector: "coloritem-dialog",
    templateUrl: "./coloritem-dialog.component.html",
    styleUrls: ["../../styles/master.style.scss", "../../styles/edit.style.scss"],
    providers: [
        ColorService,
        ColorServiceCommunicate,
        DataTableServiceCommunicate
    ]
})
// color-item-dialog component */
export class ColorItemDialogComponent
    extends BaseDialogComponent<Color, ColorService> implements OnDestroy {
    /** color-item-dialog ctor */
    constructor(
        public service: ColorService,
        public serviceCom: ColorServiceCommunicate,
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
            { prop: "OnhandVolumn",name : "Onhand",flexGrow: 1},
        ];
    }
    // parameter
    canSave: boolean;
    subscription1: Subscription;
    newColorItem: Color | undefined;

    // on init
    onInit(): void {
        this.fastSelectd = true;
        this.canSave = false;
        // subscription from color-edit-extend.component
        this.subscription1 = this.serviceCom.ToParent$.subscribe(
            (TypeValue: [Color, boolean]) => {
                this.newColorItem = TypeValue[0];
                this.canSave = TypeValue[1];
            });
    }

    // on get data with lazy load
    loadDataScroll(scroll: Scroll): void {
        if (this.mode) {
            scroll.Where = this.mode.toString();
        }
        this.service.getAllWithScroll(scroll,"GetScrollOnhand/")
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

        if (this.subscription1) {
            this.subscription1.unsubscribe();
        }
    }

    // on new color item click
    onNewColorItem(): void {
        this.newColorItem = {
            ColorItemId: 0,
        };
        setTimeout(() => {
            if (this.newColorItem) {
                this.serviceCom.toChildEdit(undefined);
            }
        }, 500);
    }

    // on new color button click
    onComplateOrCancel(type?: boolean): void {
        if (type !== undefined && this.newColorItem) {
            if (type === true) {
                // debug here
                // console.log("Color New",this.newColorItem);

                this.service.post(this.newColorItem)
                    .subscribe(newComplate => {
                        this.selected = newComplate;
                        this.onSelectedClick();
                    });
            } else {
                this.newColorItem = undefined;
                this.canSave = false;
            }
        }
    }
}