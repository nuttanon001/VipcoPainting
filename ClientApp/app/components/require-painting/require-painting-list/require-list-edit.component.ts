// angular
import { Component, ViewContainerRef, OnInit, Input, Output, EventEmitter } from "@angular/core";
import { FormBuilder, FormControl, Validators, FormGroup } from "@angular/forms";
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
        if (this.RequirePaintListForm.valid) {
            if (this.blastWorkItem && this.blastWork) {
                if (this.blastWorkItem.IsValid) {
                    return true;
                }
            } else if (this.paintWorks.length && this.listBoxs.findIndex(item => item === true) > -1) {
                if (this.paintWorks.findIndex(item => item.IsValid === true) > -1) {
                    // console.log("paintWorks", this.paintWorks.filter(item => item.IsValid));
                    return true;
                }
            }
        }
        return false;
    }
    // on Init
    ngOnInit(): void {
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
            SizeL: [this.RequirePaintList.SizeL,
                [
                    Validators.required,
                    Validators.min(1)
                ]
            ],
            SizeW: [this.RequirePaintList.SizeW,
                [
                    Validators.required,
                    Validators.min(1)
                ]
            ],
            SizeH: [this.RequirePaintList.SizeH,
                [
                    Validators.required,
                    Validators.min(1)
                ]
            ],
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

    // on Change
    checkBoxChage(isChange?: boolean, levelPaint?: string): void {
        if (isChange !== undefined && levelPaint) {
            if (levelPaint.indexOf("PrimerCoat") !== -1) {
                if (!this.paintWorks[0]) {
                    this.paintWorks[0] = {
                        PaintWorkItemId: 0,
                        PaintLevel: 1,
                        PaintLevelString: "PrimerCoat"
                    };
                }
                this.listBoxs[0] = isChange;
            } else if (levelPaint.indexOf("MidCoat") !== -1) {
                if (!this.paintWorks[1]) {
                    this.paintWorks[1] = {
                        PaintWorkItemId: 0,
                        PaintLevel: 2,
                        PaintLevelString: "MidCoat"
                    };
                }
                this.listBoxs[1] = isChange;
            } else if (levelPaint.indexOf("IntermediateCoat") !== -1) {
                if (!this.paintWorks[2]) {
                    this.paintWorks[2] = {
                        PaintWorkItemId: 0,
                        PaintLevel: 3,
                        PaintLevelString: "IntermediateCoat"
                    };
                }
                this.listBoxs[2] = isChange;

            } else if (levelPaint.indexOf("TopCoat") !== -1) {
                if (!this.paintWorks[3]) {
                    this.paintWorks[3] = {
                        PaintWorkItemId: 0,
                        PaintLevel: 4,
                        PaintLevelString: "TopCoat"
                    };
                }
                this.listBoxs[3] = isChange;
            } else if (levelPaint.indexOf("BlastWork") !== -1) {
                if (!this.blastWorkItem) {
                    this.blastWorkItem = {
                        BlastWorkItemId: 0,
                    };
                }
                this.blastWork = isChange;
            }
        }
    }
}

