// angular core
import { Component, ViewContainerRef, OnInit, Input, Output, EventEmitter } from "@angular/core";
import { FormBuilder, FormControl, Validators, FormGroup, AbstractControl } from "@angular/forms";
// models
import {
    RequirePaintMaster, RequirePaintList,
    ListPaintBlastWorkItem,
    PaintWorkItem,
} from "../../../models/model.index";
// services
import { DialogsService } from "../../../services/dialog/dialogs.service";
import { RequirePaintMasterService, RequirePaintMasterServiceCommunicate } from "../../../services/require-paint/require-paint-master.service";
import { RequirePaintListService } from "../../../services/require-paint/require-paint-list.service";
import { Calendar } from "primeng/components/calendar/calendar";

@Component({
    selector: "require-painting-list-workitem",
    templateUrl: "./require-painting-list-workitem.component.html",
    styleUrls: ["../../../styles/edit.style.scss"],
})
/** require-painting-list-workitem component*/
export class RequirePaintingListWorkitemComponent implements OnInit {
    /** require-painting-list-workitem ctor */
    constructor(
        private service: RequirePaintListService,
        private fb: FormBuilder,
    ) { }

    // Parameter
    maxDate: Date = new Date;
    paintBlastWorkItems: ListPaintBlastWorkItem;
    // Form
    RequirePaintListForm: FormGroup;
    //@Output @Input
    @Output("ComplateOrCancel") ComplateOrCancel = new EventEmitter<RequirePaintList>();
    @Input("RequirePaintList") RequirePaintList: RequirePaintList;

    // isCheckForm
    isPaintBlastValid: boolean;
    paintCheckBox: Array<boolean>;

    // on Init angular core
    ngOnInit(): void {
        // Debug here
        // console.log(JSON.stringify(this.RequirePaintList));
        if (!this.paintBlastWorkItems) {
            this.paintBlastWorkItems = {
                BlastWorkItems: new Array,
                PaintWorkItems: new Array
            };
        }

        if (this.RequirePaintList) {
            this.buildForm();

            if (this.RequirePaintList.BlastWorkItems) {
                this.paintBlastWorkItems.BlastWorkItems.push(...this.RequirePaintList.BlastWorkItems);
            } 

            if (this.RequirePaintList.PaintWorkItems) {
                this.paintBlastWorkItems.PaintWorkItems.push(...this.RequirePaintList.PaintWorkItems);
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
            this.paintCheckBox.forEach((item, index) => {
                if (item === true && index != 0) {
                    if (this.paintBlastWorkItems.PaintWorkItems) {
                        tempPaint.push(this.paintBlastWorkItems.PaintWorkItems[index - 1]);
                    }
                }
            });

            // set BlastWorks
            this.RequirePaintList.BlastWorkItems = new Array;
            this.RequirePaintList.BlastWorkItems.push(...this.paintBlastWorkItems.BlastWorkItems);
            // set PaintWorks
            this.RequirePaintList.PaintWorkItems = new Array;
            this.RequirePaintList.PaintWorkItems = tempPaint.slice();

            this.ComplateOrCancel.emit(this.RequirePaintList);
        }
    }

    // bug calendar not update min-max
    // update CakenderUi
    updateCalendarUI(calendar: Calendar) {
        calendar.updateUI();
    }
}