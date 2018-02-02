import { Component, OnInit, Inject, ViewChild } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material";
// models
import { RequirePaintMaster } from "../../models/model.index";
// services
import { DialogsService } from "../../services/dialog/dialogs.service";
import { RequirePaintMasterService } from "../../services/require-paint/require-paint-master.service";

@Component({
    selector: "require-painting-view-dialog",
    templateUrl: "./require-painting-view-dialog.component.html",
    styleUrls: ["../../styles/master.style.scss"],
})
/** require-painting-view-dialog component*/
export class RequirePaintingViewDialogComponent implements OnInit {
    /** require-painting-view-dialog ctor */
    constructor(
        private service: RequirePaintMasterService,
        @Inject(MAT_DIALOG_DATA) public requireMasterId: number,
        private dialogRef: MatDialogRef<number>) {}

    // Parameter
    displayValue: RequirePaintMaster;
    canClose: boolean;

    /** Called by Angular after cutting-plan-dialog component initialized */
    ngOnInit(): void {
        this.canClose = false;
        if (this.requireMasterId) {
            this.service.getOneKeyNumber(this.requireMasterId)
                .subscribe(dbData => {
                    this.displayValue = dbData;
                },error => this.onCancelClick());
        } else {
            this.onCancelClick();
        }
    }

    // Selected Value
    onSelectedValue(value?: number): void {
        if (value) {
            this.dialogRef.close(value);
        }
    }

    // No Click
    onCancelClick(mode: number = 0): void {
        if (mode === 0) {
            this.dialogRef.close();
        } else {
            this.dialogRef.close(-99);
        }
    }
}