﻿// angular
import { Component, ViewContainerRef, OnInit, Input, Output, EventEmitter } from "@angular/core";
import { FormBuilder, FormControl, Validators, FormGroup, AbstractControl } from "@angular/forms";
// models
import {
    RequirePaintMaster, RequirePaintList,
    PaintWorkItem, BlastWorkItem,IDictionary, AttachFile
} from "../../../models/model.index";
// components
import { BaseEditComponent } from "../../base-component/base-edit.component";
// services
import { DialogsService } from "../../../services/dialog/dialogs.service";
import { RequirePaintMasterService, RequirePaintMasterServiceCommunicate } from "../../../services/require-paint/require-paint-master.service";
import { RequirePaintListService } from "../../../services/require-paint/require-paint-list.service";
import { Calendar } from "primeng/components/calendar/calendar";
import { retry } from "rxjs/operator/retry";
import { transformMenu } from "@angular/material";

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
    attachFiles: Array<AttachFile> = new Array;
    paintWorks: Array<PaintWorkItem> | undefined;
    listBoxs: Array<boolean> | undefined;
    maxDate: Date = new Date;
    // Blast
    blastWork: boolean | undefined;
    blastWorkItem: BlastWorkItem | undefined;
    // Form
    RequirePaintListForm: FormGroup | undefined;
    // levelPaints
    levelPaints: Array<string> | undefined;

    @Output("ComplateOrCancel") ComplateOrCancel = new EventEmitter<RequirePaintList>();
    @Input("RequirePaintList") RequirePaintList: RequirePaintList | undefined;
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
    isPaintValid: boolean | undefined;

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
                        if (item.PaintLevel && this.listBoxs && this.paintWorks && this.levelPaints) {
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
        this.getAttach();
    }

    // build form
    buildForm(): void {
        // debug here
        // console.log("buildForm", this.RequirePaintList);
        if (!this.RequirePaintList) { return; }

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
            SendWorkItem: [this.RequirePaintList.SendWorkItem,
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
            PaintWorkItems: [this.RequirePaintList.PaintWorkItems],
            AttachFile: [this.RequirePaintList.AttachFile],
            RemoveAttach: [this.RequirePaintList.RemoveAttach]
        });

        // change validity control
        const QuantityControl: AbstractControl | null = this.RequirePaintListForm.get("Quantity");
        if (QuantityControl) {
            QuantityControl.valueChanges.subscribe((Qty: number) => {
                if (!this.RequirePaintListForm) { return; }

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
        if (this.RequirePaintListForm && this.listBoxs && this.RequirePaintList && this.blastWorkItem) {
            this.RequirePaintList = this.RequirePaintListForm.value;

            let tempPaint: Array<PaintWorkItem> = new Array;
            // get only paint selected
            this.listBoxs.forEach((item, index) => {
                if (this.paintWorks) {
                    if (item === true) {
                        tempPaint.push(this.paintWorks[index]);
                    }
                }
            });
            if (this.RequirePaintList) {
                // set BlastWorks
                this.RequirePaintList.BlastWorkItems = new Array;
                this.RequirePaintList.BlastWorkItems.push(this.blastWorkItem);
                // set PaintWorks
                this.RequirePaintList.PaintWorkItems = new Array;
                this.RequirePaintList.PaintWorkItems = tempPaint.slice();

                this.ComplateOrCancel.emit(this.RequirePaintList);
            }
        }
    }

    // on Cancel
    onCancelClick(): void {
        this.ComplateOrCancel.emit(undefined);
    }

    // on Add level of paint
    onShowOrDidNotLevelOfPaint(checkBox: boolean, index: number, paintWorkItem: PaintWorkItem) {
        if (this.paintWorks && this.listBoxs) {
            this.paintWorks[index] = this.paintWorks[index] ? (checkBox ? this.paintWorks[index] : paintWorkItem) : paintWorkItem;
            this.listBoxs[index] = checkBox;
        }
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
            this.isPaintValid = true;
            this.paintWorks.forEach(item => {
                if (item) {
                    if (!item.IsValid) {
                        this.isPaintValid = false;
                    }
                }
            });

            //if (this.paintWorks.findIndex(item => item.IsValid === false) > -1) {
            //} else {
            //    this.isPaintValid = true;
            //}
        }
    }

    // bug calendar not update min-max
    // update CakenderUi
    updateCalendarUI(calendar: Calendar) {
        calendar.updateUI();
    }

    ////////////
    // Module //
    ////////////

    // get attact file
    getAttach(): void {
        if (this.RequirePaintList && this.RequirePaintList.RequirePaintingListId > 0) {
            this.service.getAttachFile(this.RequirePaintList.RequirePaintingListId)
                .subscribe(dbAttach => {
                    this.attachFiles = dbAttach.slice();
                }, error => console.error(error));
        }
    }

    // on Attach Update List
    onUpdateAttachResults(results: FileList): void {
        if (this.RequirePaintList && this.RequirePaintListForm) {
            // debug here
            // console.log("File: ", results);
            this.RequirePaintList.AttachFile = results;
            // debug here
            // console.log("Att File: ", this.RequirePaintList.AttachFile);

            this.RequirePaintListForm.patchValue({
                AttachFile: this.RequirePaintList.AttachFile
            });
        }
    }

    // on Attach delete file
    onDeleteAttachFile(attach: AttachFile): void {
        if (attach && this.RequirePaintList && this.RequirePaintListForm) {
            if (!this.RequirePaintList.RemoveAttach) {
                this.RequirePaintList.RemoveAttach = new Array;
            }

            // remove
            this.RequirePaintList.RemoveAttach.push(attach.AttachFileId);
            // debug here
            // console.log("Remove :",this.editValue.RemoveAttach);

            this.RequirePaintListForm.patchValue({
                RemoveAttach: this.RequirePaintList.RemoveAttach
            });
            let template: Array<AttachFile> =
                this.attachFiles.filter((e: AttachFile) => e.AttachFileId !== attach.AttachFileId);

            this.attachFiles = new Array();
            setTimeout(() => this.attachFiles = template.slice(), 50);
        }
    }

    // open file attach
    onOpenNewLink(link: string): void {
        if (link) {
            window.open(link, "_blank");
        }
    }
}

