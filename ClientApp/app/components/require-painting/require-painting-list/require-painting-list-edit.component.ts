// angular
import { Component, ViewContainerRef,OnInit,Input,Output, EventEmitter } from "@angular/core";
import { FormBuilder, FormControl, Validators, FormGroup } from "@angular/forms";
// models
import { RequirePaintMaster, RequirePaintList, PaintWorkItem } from "../../../models/model.index";
// components
import { BaseEditComponent } from "../../base-component/base-edit.component";
// services
import { DialogsService } from "../../../services/dialog/dialogs.service";
import { RequirePaintMasterService, RequirePaintMasterServiceCommunicate } from "../../../services/require-paint/require-paint-master.service";
import { RequirePaintListService } from "../../../services/require-paint/require-paint-list.service";

@Component({
    selector: "require-painting-list-edit",
    templateUrl: "./require-painting-list-edit.component.html",
    styleUrls: ["../../../styles/edit.style.scss"],
})
/** require-painting-list-edit component*/
export class RequirePaintingListEditComponent implements OnInit {

    /** require-painting-list-edit ctor */
    constructor(
        private service: RequirePaintListService,
        private fb: FormBuilder,
    ) { }

    // Parameter
    // PrimerCoat
    primerCoat: boolean;
    paintPrimerCoat: PaintWorkItem;
    // MidCoat
    midCoat: boolean;
    paintMidCoat: PaintWorkItem;
    // IntermediateCoat
    intermediateCoat: boolean;
    paintIntermediateCoat: PaintWorkItem;
    // TopCoat
    topCoat: boolean;
    paintTopCoat: PaintWorkItem;
    // Form
    LinkArea: boolean;
    listItemForm: FormGroup;
    _listItem: RequirePaintList;

    @Output("ComplateOrCancel") ComplateOrCancel = new EventEmitter<RequirePaintList>();
    @Input("listItem")
    get listItem(): RequirePaintList {
        return this._listItem;
    }
    set listItem(value: RequirePaintList) {
        this._listItem = value;
        if (this._listItem) {
            this.buildForm();
        }
    }

    // on Init
    ngOnInit(): void {
        this.LinkArea = true;
        // Auto Complate
    }

    // build form
    buildForm(): void {
        console.log("buildForm");
        this.listItemForm = this.fb.group({
            RequirePaintingListId: [this.listItem.RequirePaintingListId],
            Description: [this.listItem.Description,
                [
                    Validators.required,
                    Validators.maxLength(250)
                ]
            ],
            PartNo: [this.listItem.PartNo,
                [
                    Validators.maxLength(150)
                ]
            ],
            MarkNo: [this.listItem.MarkNo,
                [
                    Validators.maxLength(150)
                ]
            ],
            DrawingNo: [this.listItem.DrawingNo,
                [
                    Validators.maxLength(150)
                ]
            ],
            UnitNo: [this.listItem.UnitNo],
            Quantity: [this.listItem.Quantity,
                [
                    Validators.required,
                    Validators.min(1)
                ]
            ],
            FieldWeld: [this.listItem.FieldWeld],
            Insulation: [this.listItem.Insulation],
            ITP: [this.listItem.ITP],
            SizeL: [this.listItem.SizeL,
                [
                    Validators.required,
                    Validators.min(1)
                ]
            ],
            SizeW: [this.listItem.SizeW,
                [
                    Validators.required,
                    Validators.min(1)
                ]
            ],
            SizeH: [this.listItem.SizeH,
                [
                    Validators.required,
                    Validators.min(1)
                ]
            ],
            Weight: [this.listItem.Weight,
                [
                    Validators.required,
                    Validators.min(1)
                ]
            ],
            Creator: [this.listItem.Creator],
            CreateDate: [this.listItem.CreateDate],
            Modifyer: [this.listItem.Modifyer],
            ModifyDate: [this.listItem.ModifyDate],
            // FK
            RequirePaintingMasterId: [this.listItem.RequirePaintingMasterId],
        });
    }

    // on New/Update
    onNewOrUpdateClick(): void {
        if (this.listItemForm) {
            if (this.listItemForm.valid) {
                this.listItem = this.listItemForm.value;
                this.ComplateOrCancel.emit(this.listItem);
            }
        }
    }

    // on Cancel
    onCancelClick(): void {
        this.ComplateOrCancel.emit(undefined);
    }

    // on Change
    checkBoxChage(isChang?: any, levelPaint?: string): void {
        if (isChang !== undefined && levelPaint) {
            if (levelPaint.indexOf("PrimerCoat") !== -1) {
                if (!this.paintPrimerCoat) {
                    this.paintPrimerCoat = {
                        PaintWorkItemId: 0,
                        PaintLevel: 1,
                        PaintLevelString: "PrimerCoat"
                    };
                }
                this.primerCoat = isChang;

            } else if (levelPaint.indexOf("MidCoat") !== -1) {
                if (!this.paintMidCoat) {
                    this.paintMidCoat = {
                        PaintWorkItemId: 0,
                        PaintLevel: 2,
                        PaintLevelString: "MidCoat"
                    };
                }
                this.midCoat = isChang;

            } else if (levelPaint.indexOf("IntermediateCoat") !== -1) {
                if (!this.paintIntermediateCoat) {
                    this.paintIntermediateCoat = {
                        PaintWorkItemId: 0,
                        PaintLevel: 3,
                        PaintLevelString: "IntermediateCoat"
                    };
                }
                this.intermediateCoat = isChang;

            } else if (levelPaint.indexOf("TopCoat") !== -1) {
                if (!this.paintTopCoat) {
                    this.paintTopCoat = {
                        PaintWorkItemId: 0,
                        PaintLevel: 4,
                        PaintLevelString: "TopCoat"
                    };
                }
                this.topCoat = isChang;
            }
        }
    }

    onIntAreaChange(areaChange?: number): void {

        if (areaChange && this.LinkArea) {
            if (this.paintPrimerCoat) {
                this.paintPrimerCoat.IntArea = areaChange;
                console.log("paintPrimerCoat", this.paintPrimerCoat.IntArea);
            }
            if (this.paintMidCoat) {
                this.paintMidCoat.IntArea = areaChange;
                console.log("paintMidCoat", this.paintMidCoat.IntArea);
            }
            if (this.paintIntermediateCoat) {
                this.paintIntermediateCoat.IntArea = areaChange;
            }
            if (this.paintTopCoat) {
                this.paintTopCoat.IntArea = areaChange;
            }
        }
    }
}