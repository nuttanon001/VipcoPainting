// angular core
import { Component, ViewContainerRef } from "@angular/core";
import { FormBuilder, FormControl, Validators, FormGroup } from "@angular/forms";
// models
import { RequirePaintMaster, InitialRequirePaint, RequirePaintMasterHasInitial, BlastWorkItem, PaintWorkItem, ListPaintBlastWorkItem } from "../../../models/model.index";
// components
import { BaseEditComponent } from "../../base-component/base-edit.component";
// services
import { AuthService } from "../../../services/auth/auth.service";
import { DialogsService } from "../../../services/dialog/dialogs.service";
import {
    RequirePaintMasterService, RequirePaintMasterHasInitialServiceCommunicate
} from "../../../services/require-paint/require-paint-master.service";
import {
    InitialRequirePaintService
} from "../../../services/require-paint/initial-require-paint.service";

import { BlastWorkitemService } from "../../../services/require-paint/blast-workitem.service";
import { PaintWorkitemService } from "../../../services/require-paint/paint-workitem.service";
import { Calendar } from "primeng/primeng";

@Component({
    selector: "initial-require-painting-edit",
    templateUrl: "./initial-require-painting-edit.component.html",
    styleUrls: [
        "../../../styles/edit.style.scss",
        "../../base-component/data-table.style.scss"
    ],
})

/** initial-require-painting-edit component*/
export class InitialRequirePaintingEditComponent 
    extends BaseEditComponent<RequirePaintMaster, RequirePaintMasterService> {
    /** initial-require-painting-edit ctor */
    constructor(
        service: RequirePaintMasterService,
        serviceCom: RequirePaintMasterHasInitialServiceCommunicate,
        private serviceBlastWork: BlastWorkitemService,
        private servicePaintWork: PaintWorkitemService,
        private serviceInitial: InitialRequirePaintService,
        private serviceAuth: AuthService,
        private viewContainerRef: ViewContainerRef,
        private serviceDialogs: DialogsService,
        private fb: FormBuilder
    ) {
        super(
            service,
            serviceCom,
        );
    }
    // Parameter
    paintBlastWorkItems: ListPaintBlastWorkItem;
    // isCheckForm
    isPaintBlastValid: boolean;
    paintCheckBox: Array<boolean>;

    initialRequirePaint: InitialRequirePaint;
    // Form 
    initialRequireForm: FormGroup;
    // on get data by key
    onGetDataByKey(value?: RequirePaintMaster): void {
        if (value) {
            this.service.getOneKeyNumber(value.RequirePaintingMasterId)
                .subscribe(dbData => {
                    this.editValue = dbData;
                    // set Date
                    if (this.editValue.FinishDate) {
                        this.editValue.FinishDate = this.editValue.FinishDate != null ?
                            new Date(this.editValue.FinishDate) : new Date();
                    }

                    if (this.editValue.ReceiveDate) {
                        this.editValue.ReceiveDate = this.editValue.ReceiveDate != null ?
                            new Date(this.editValue.ReceiveDate) : new Date();
                    }

                    if (this.editValue.RequireDate) {
                        this.editValue.RequireDate = this.editValue.RequireDate != null ?
                            new Date(this.editValue.RequireDate) : new Date();
                    }

                    if (this.editValue.RequirePaintingMasterId) {
                        this.serviceInitial.getByMasterId(this.editValue.RequirePaintingMasterId)
                            .subscribe(dbInitials => {
                                if (dbInitials) {
                                    if (dbInitials.length > 0) {
                                        this.initialRequirePaint = dbInitials[0];
                                        // get BlastWorkItem
                                        this.serviceBlastWork.getByMasterId(this.initialRequirePaint.InitialRequireId, "GetByMaster2/")
                                            .subscribe(dbData => {
                                                if (this.initialRequirePaint) {
                                                    this.initialRequirePaint.BlastWorkItems = dbData.slice();
                                                }
                                            });
                                        // get PaintWorkItem
                                        this.servicePaintWork.getByMasterId(this.initialRequirePaint.InitialRequireId, "GetByMaster2/")
                                            .subscribe(dbData => {
                                                if (this.initialRequirePaint) {
                                                    this.initialRequirePaint.PaintWorkItems = dbData.slice();
                                                }
                                            });
                                    }
                                }
                            });
                    }

                }, error => console.error(error), () => this.defineData());
        } else {
            this.editValue = {
                RequirePaintingMasterId: 0,
                RequirePaintingStatus: 1,
                RequireDate: new Date,
                ReceiveDate: new Date,
            };

            this.initialRequirePaint = {
                InitialRequireId: 0,
                RequirePaintingMasterId:0,
                PlanStart: new Date,
                PlanEnd: new Date,
                BlastWorkItems: new Array,
                PaintWorkItems: new Array,
            }

            if (this.serviceAuth.getAuth) {
                this.editValue.RequireEmp = this.serviceAuth.getAuth.EmpCode || "";
                this.editValue.RequireString = this.serviceAuth.getAuth.NameThai || "";
            }

            this.defineData();
        }
    }

    // define data for edit form
    defineData(): void {

        if (!this.paintBlastWorkItems) {
            this.paintBlastWorkItems = {
                BlastWorkItems: new Array,
                PaintWorkItems: new Array
            };
        }

        this.buildForm();

        if (this.initialRequirePaint.BlastWorkItems) {
            this.paintBlastWorkItems.BlastWorkItems.push(...this.initialRequirePaint.BlastWorkItems);
        }

        if (this.initialRequirePaint.PaintWorkItems) {
            this.paintBlastWorkItems.PaintWorkItems.push(...this.initialRequirePaint.PaintWorkItems);
        } 
    }

    // build form
    buildForm(): void {
        this.editValueForm = this.fb.group({
            RequirePaintingMasterId: [this.editValue.RequirePaintingMasterId],
            ReceiveDate: [this.editValue.ReceiveDate],
            RequireDate: [this.editValue.RequireDate,
                [
                    Validators.required,
                ]
            ],
            FinishDate: [this.editValue.FinishDate],
            PaintingSchedule: [this.editValue.PaintingSchedule,
                [
                    Validators.maxLength(150)
                ]
            ],
            RequireNo: [this.editValue.RequireNo],
            RequirePaintingStatus: [this.editValue.RequirePaintingStatus],
            Creator: [this.editValue.Creator],
            CreateDate: [this.editValue.CreateDate],
            Modifyer: [this.editValue.Modifyer],
            ModifyDate: [this.editValue.ModifyDate],
            // FK
            RequireEmp: [this.editValue.RequireEmp],
            ReceiveEmp: [this.editValue.ReceiveEmp],
            ProjectCodeSubId: [this.editValue.ProjectCodeSubId],
            // ViewModel
            RequireString: [this.editValue.RequireString],
            ReceiveString: [this.editValue.ReceiveString],
            ProjectCodeSubString: [this.editValue.ProjectCodeSubString]
        });
        this.editValueForm.valueChanges.subscribe((data: any) => this.onValueChanged(data));
        // Initial Require Paint
        this.initialRequireForm = this.fb.group({
            InitialRequireId: [this.initialRequirePaint.InitialRequireId],
            PlanStart: [this.initialRequirePaint.PlanStart],
            PlanEnd: [this.initialRequirePaint.PlanEnd],
            DrawingNo: [this.initialRequirePaint.DrawingNo],
            UnitNo: [this.initialRequirePaint.UnitNo],
            RequirePaintingMasterId: [this.initialRequirePaint.RequirePaintingMasterId],
            BlastWorkItems: [this.initialRequirePaint.BlastWorkItems],
            PaintWorkItems: [this.initialRequirePaint.PaintWorkItems],
        });
        this.initialRequireForm.valueChanges.subscribe((data: any) => this.onValueChanged(data));
    }

    // Override
    // on value of form change 
    onValueChanged(data?: any): void {
        if (!this.editValueForm && !this.initialRequireForm) { return; }
        const form = this.editValueForm;
        const form2 = this.initialRequireForm;
        // on form valid or not
        this.onFormValid(form.valid && form2.valid);
    }

    // open dialog
    openDialog(type?: string): void {
        if (type) {
            if (type === "Employee") {
                this.serviceDialogs.dialogSelectEmployee(this.viewContainerRef)
                    .subscribe(emp => {
                        if (emp) {
                            this.editValueForm.patchValue({
                                RequireEmp: emp.EmpCode,
                                RequireString: `คุณ${emp.NameThai}`,
                            });
                        }
                    });
            } else if (type === "Project") {
                this.serviceDialogs.dialogSelectProject(this.viewContainerRef,2)
                    .subscribe(project => {
                        if (project) {
                            this.editValueForm.patchValue({
                                ProjectCodeSubId: project.ProjectCodeSubId,
                                ProjectCodeSubString: `${project.ProjectMasterString}/${project.Code}`,
                            });
                        }
                    });
            }
        }
    }

    // on valid data
    onFormValid(isValid: boolean): void {

        this.editValue = this.editValueForm.value;
        this.initialRequirePaint = this.initialRequireForm.value;

        // Update WorkItem
        if (this.isPaintBlastValid) {
            this.onUpdateWorkItem();
        }

        let editComplate: RequirePaintMasterHasInitial = {
            InitialRequirePaint: this.initialRequirePaint,
            RequirePaintMaster: this.editValue
        };
        this.communicateService.toParent([editComplate, isValid && this.isPaintBlastValid]);
    }

    // on Update Workitem
    onUpdateWorkItem(): void {
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
        this.initialRequirePaint.BlastWorkItems = new Array;
        this.initialRequirePaint.BlastWorkItems = this.paintBlastWorkItems.BlastWorkItems.slice();
        // set PaintWorks
        this.initialRequirePaint.PaintWorkItems = new Array;
        this.initialRequirePaint.PaintWorkItems = tempPaint.slice();
    }

    // bug calendar not update min-max
    // update CakenderUi
    updateCalendarUI(calendar: Calendar) {
        calendar.updateUI();
    }
}