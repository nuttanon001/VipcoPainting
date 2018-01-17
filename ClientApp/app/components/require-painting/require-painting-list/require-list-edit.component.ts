// angular
import { Component, ViewContainerRef, OnInit, Input, Output, EventEmitter } from "@angular/core";
import { FormBuilder, FormControl, Validators, FormGroup, AbstractControl } from "@angular/forms";
// models
import {
    RequirePaintMaster, RequirePaintList,
    PaintWorkItem, BlastWorkItem,IDictionary
} from "../../../models/model.index";
// components
import { BaseEditComponent } from "../../base-component/base-edit.component";
// services
import { DialogsService } from "../../../services/dialog/dialogs.service";
import { RequirePaintMasterService, RequirePaintMasterServiceCommunicate } from "../../../services/require-paint/require-paint-master.service";
import { RequirePaintListService } from "../../../services/require-paint/require-paint-list.service";
import { Calendar } from "primeng/components/calendar/calendar";

@Component({
    selector: "require-list-edit",
    templateUrl: "./require-list-edit.component.html",
    styleUrls: ["../../../styles/edit.style.scss"],
})
// require-list-edit component*/
export class RequireListEditComponent implements OnInit {
    /** require-list-edit ctor */
    constructor(
        private service: RequirePaintListService,
        private fb: FormBuilder,
    ) { }

    // Parameter
    paintWorks: Array<PaintWorkItem>;
    listBoxs: Array<boolean>;
    maxDate: Date = new Date;
    // Blast
    blastWork: boolean;
    blastWorkItem: BlastWorkItem;
    // Form
    RequirePaintListForm: FormGroup;
    // levelPaints
    levelPaints: Array<string>;

    @Output("ComplateOrCancel") ComplateOrCancel = new EventEmitter<RequirePaintList>();
    @Input("RequirePaintList") RequirePaintList: RequirePaintList;
    // isCheckForm
    get isCheckForm(): boolean {
        if (this.blastWork) {
            if (this.blastWorkItem) {
                if (this.blastWorkItem.ExtArea || this.blastWorkItem.IntArea) {
                    return this.blastWorkItem.IsValid || false;
                }
            }
        }
        return false;
    }
    isPaintValid: boolean;

    // on Init
    ngOnInit(): void {
        this.isPaintValid = true;

        if (!this.levelPaints) {
            this.levelPaints = new Array;
        }

        if (!this.paintWorks) {
            this.paintWorks = new Array;
        }

        if (!this.listBoxs) {
            this.listBoxs = new Array;
            this.listBoxs.push(false);
            this.listBoxs.push(false);
            this.listBoxs.push(false);
            this.listBoxs.push(false);
        }

        if (this.RequirePaintList) {
            this.buildForm();

            if (this.RequirePaintList.BlastWorkItems) {
                if (this.RequirePaintList.BlastWorkItems[0]) {
                    this.blastWork = true;
                    this.blastWorkItem = this.RequirePaintList.BlastWorkItems[0];
                    this.blastWorkItem.IsValid = true;
                }
            }

            if (this.RequirePaintList.PaintWorkItems) {
                if (this.RequirePaintList.PaintWorkItems.length > 0) {
                    this.RequirePaintList.PaintWorkItems.forEach(item => {
                        if (item.PaintLevel) {
                            item.IsValid = true;
                            this.listBoxs[item.PaintLevel - 1] = true;
                            this.paintWorks[item.PaintLevel - 1] = item;

                            if (item.PaintLevel === 1) {
                                this.levelPaints.push("PrimerCoat");
                            } else if (item.PaintLevel === 2) {
                                this.levelPaints.push("MidCoat");
                            } else if (item.PaintLevel === 3) {
                                this.levelPaints.push("IntermediateCoat");
                            } else {
                                this.levelPaints.push("TopCoat");
                            }
                        }
                    });
                }
            }
        }
    }

    // build form
    buildForm(): void {
        // console.log("buildForm");
        this.RequirePaintListForm = this.fb.group({
            RequirePaintingListId: [this.RequirePaintList.RequirePaintingListId],
            RequirePaintingListStatus: [this.RequirePaintList.RequirePaintingListStatus],
            Description: [this.RequirePaintList.Description,
                [
                    Validators.required,
                    Validators.maxLength(250)
                ]
            ],
            PartNo: [this.RequirePaintList.PartNo,
                [
                    Validators.maxLength(150)
                ]
            ],
            MarkNo: [this.RequirePaintList.MarkNo,
                [
                    Validators.required,
                    Validators.maxLength(150)
                ]
            ],
            DrawingNo: [this.RequirePaintList.DrawingNo,
                [
                    Validators.maxLength(150)
                ]
            ],
            UnitNo: [this.RequirePaintList.UnitNo],
            Quantity: [this.RequirePaintList.Quantity,
                [
                    Validators.required,
                    Validators.min(1)
                ]
            ],
            FieldWeld: [this.RequirePaintList.FieldWeld],
            Insulation: [this.RequirePaintList.Insulation],
            ITP: [this.RequirePaintList.ITP],
            SizeL: [this.RequirePaintList.SizeL],
            SizeW: [this.RequirePaintList.SizeW],
            SizeH: [this.RequirePaintList.SizeH],
            Weight: [this.RequirePaintList.Weight,
                [
                    Validators.required,
                    Validators.min(1)
                ]
            ],
            PlanStart: [this.RequirePaintList.PlanStart,
                [
                    Validators.required
                ]
            ],
            PlanEnd: [this.RequirePaintList.PlanEnd,
                [
                    Validators.required
                ]
            ],
            Creator: [this.RequirePaintList.Creator],
            CreateDate: [this.RequirePaintList.CreateDate],
            Modifyer: [this.RequirePaintList.Modifyer],
            ModifyDate: [this.RequirePaintList.ModifyDate],
            // FK
            RequirePaintingMasterId: [this.RequirePaintList.RequirePaintingMasterId],
            BlastWorkItems: [this.RequirePaintList.BlastWorkItems],
            PaintWorkItems: [this.RequirePaintList.PaintWorkItems]
        });

        // change validity control
        const QuantityControl: AbstractControl | null = this.RequirePaintListForm.get("Quantity");
        if (QuantityControl) {
            QuantityControl.valueChanges.subscribe((Qty: number) => {
                const SizeLControl: AbstractControl | null = this.RequirePaintListForm.get("SizeL");
                const SizeWControl: AbstractControl | null = this.RequirePaintListForm.get("SizeW");
                const SizeHControl: AbstractControl | null = this.RequirePaintListForm.get("SizeH");

                if (SizeLControl && SizeWControl && SizeHControl) {
                    if (Qty <= 1) {
                        SizeLControl.setValidators([
                            Validators.required,
                            Validators.min(1)
                        ]);
                        SizeWControl.setValidators([
                            Validators.required,
                            Validators.min(1)
                        ]);
                        SizeHControl.setValidators([
                            Validators.required,
                            Validators.min(1)
                        ]);
                    } else {
                        SizeLControl.setValidators([]);
                        SizeWControl.setValidators([]);
                        SizeHControl.setValidators([]);
                    }
                    SizeLControl.updateValueAndValidity();
                    SizeWControl.updateValueAndValidity();
                    SizeHControl.updateValueAndValidity();
                }
            });
        }
    }

    // on New/Update
    onNewOrUpdateClick(): void {
        if (this.RequirePaintListForm) {
            this.RequirePaintList = this.RequirePaintListForm.value;

            let tempPaint: Array<PaintWorkItem> = new Array;
            // get only paint selected
            this.listBoxs.forEach((item, index) => {
                if (item === true) {
                    tempPaint.push(this.paintWorks[index]);
                }
            });

            // set BlastWorks
            this.RequirePaintList.BlastWorkItems = new Array;
            this.RequirePaintList.BlastWorkItems.push(this.blastWorkItem);
            // set PaintWorks
            this.RequirePaintList.PaintWorkItems = new Array;
            this.RequirePaintList.PaintWorkItems = tempPaint.slice();

            this.ComplateOrCancel.emit(this.RequirePaintList);
        }
    }

    // on Cancel
    onCancelClick(): void {
        this.ComplateOrCancel.emit(undefined);
    }

    // on Add level of paint
    onShowOrDidNotLevelOfPaint(checkBox:boolean,index:number,paintWorkItem:PaintWorkItem) {
        this.paintWorks[index] = this.paintWorks[index] ? (checkBox ? this.paintWorks[index] : paintWorkItem) : paintWorkItem;
        this.listBoxs[index] = checkBox;
    }

    // on Change
    checkBoxChage(isChange?: boolean, levelPaint?: string): void {
        if (isChange !== undefined && levelPaint) {

            let paintWorkItem: PaintWorkItem = {
                PaintWorkItemId: 0
            };

            if (levelPaint.indexOf("PrimerCoat") !== -1) {
                paintWorkItem.PaintLevel = 1;
                paintWorkItem.PaintLevelString = "PrimerCoat";

                this.onShowOrDidNotLevelOfPaint(isChange, 0, paintWorkItem);
            } else if (levelPaint.indexOf("MidCoat") !== -1) {
                paintWorkItem.PaintLevel = 2;
                paintWorkItem.PaintLevelString = "MidCoat";

                this.onShowOrDidNotLevelOfPaint(isChange, 1, paintWorkItem);
            } else if (levelPaint.indexOf("IntermediateCoat") !== -1) {
                paintWorkItem.PaintLevel = 3;
                paintWorkItem.PaintLevelString = "IntermediateCoat";

                this.onShowOrDidNotLevelOfPaint(isChange, 2, paintWorkItem);

            } else if (levelPaint.indexOf("TopCoat") !== -1) {
                paintWorkItem.PaintLevel = 4;
                paintWorkItem.PaintLevelString = "TopCoat";

                this.onShowOrDidNotLevelOfPaint(isChange, 3, paintWorkItem);
            } else if (levelPaint.indexOf("BlastWork") !== -1) {
                if (!this.blastWorkItem || isChange === false) {
                    this.blastWorkItem = {
                        BlastWorkItemId: 0,
                    };
                }
                this.blastWork = isChange;
            }
        }
    }

    //onHasChange
    onHasChange(hasChange: boolean) {
        if (this.paintWorks) {
            if (this.paintWorks.findIndex(item => item.IsValid === false) > -1) {
                this.isPaintValid = false;
            } else {
                this.isPaintValid = true;
            }
        }
    }

    // bug calendar not update min-max
    // update CakenderUi
    updateCalendarUI(calendar: Calendar) {
        calendar.updateUI();
    }
}

