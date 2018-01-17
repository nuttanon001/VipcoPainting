// angular
import { OnInit, Component, ViewContainerRef, Input, Output, EventEmitter } from "@angular/core";
import { FormBuilder, FormControl, Validators, FormGroup, AbstractControl } from "@angular/forms";
// models
import { PaintTaskDetail, BlastWorkItem } from "../../../models/model.index";
// 3rd-patry
import { SelectItem } from "primeng/primeng";
// services
import { DialogsService } from "../../../services/dialog/dialogs.service";
import { PaintTeamService } from "../../../services/task/paint-team.service";
import { PaintWorkitemService } from "../../../services/require-paint/paint-workitem.service";
import { PaintTaskDetailService } from "../../../services/paint-task/paint-task-detail.service";
import { Calendar } from "primeng/components/calendar/calendar";

@Component({
    selector: "paint-task-detail-paint",
    templateUrl: "./paint-task-detail-paint.component.html",
    styleUrls: ["../../../styles/edit.style.scss"],
})
/** paint-task-detail-paint component*/
export class PaintTaskDetailPaintComponent implements OnInit {
    /** paint-task-detail-paint ctor */
    constructor(
        private service: PaintTeamService,
        private servicePaintTaskDetail: PaintTaskDetailService,
        private servicePaintWork: PaintWorkitemService,
        private serviceDialogs: DialogsService,
        private fb: FormBuilder,
        private dialogService: DialogsService,
        private viewContainerRef: ViewContainerRef
    ) { }

    // Parameter
    // Two way binding
    _paintTaskDetail: PaintTaskDetail;
    @Output() paintTaskDetailChange = new EventEmitter<PaintTaskDetail>();
    @Input()
    get paintTaskDetail(): PaintTaskDetail {
        return this._paintTaskDetail;
    }
    set paintTaskDetail(data: PaintTaskDetail) {
        this._paintTaskDetail = data;
        this.paintTaskDetailChange.emit(this._paintTaskDetail);
    }
    // Value
    @Input() ReadOnly: boolean = false;
    @Output() hasChange = new EventEmitter<boolean>();
    @Output() showReportPaint = new EventEmitter<number>();
    cannotEdit: boolean;
    // FormGroup
    paintTaskDetailForm: FormGroup;
    minProgress: number;
    maxDate: Date = new Date();
    // ComboBox
    @Input() paintTeams: Array<SelectItem>;
    @Input() paymentDetails: Array<SelectItem>;
    // OnInit
    ngOnInit(): void {
        this.buildForm();
    }
    // build Form
    buildForm(): void {
        this.minProgress = this.paintTaskDetail.TaskDetailProgress || 0;
        this.cannotEdit = this.ReadOnly ? true : this.minProgress > 0;
        this.paintTaskDetailForm = this.fb.group({
            PaintTaskDetailId: [this.paintTaskDetail.PaintTaskDetailId],
            Remark: [this.paintTaskDetail.Remark,
                [
                    Validators.maxLength(200)
                ]
            ],
            PaintTaskDetailStatus: [this.paintTaskDetail.PaintTaskDetailStatus],
            PaintTaskDetailType: [this.paintTaskDetail.PaintTaskDetailType],
            PaintTaskDetailLayer: [this.paintTaskDetail.PaintTaskDetailLayer],
            PlanSDate: [this.paintTaskDetail.PlanSDate,
                [
                    Validators.required
                ]
            ],
            PlanEDate: [this.paintTaskDetail.PlanEDate,
                [
                    Validators.required
                ]
            ],
            ActualSDate: [this.paintTaskDetail.ActualSDate],
            ActualEDate: [this.paintTaskDetail.ActualEDate],
            TaskDetailProgress: [this.paintTaskDetail.TaskDetailProgress,
                [
                    Validators.min(this.minProgress),
                    Validators.max(100),
                ]
            ],
            Creator: [this.paintTaskDetail.Creator],
            CreateDate: [this.paintTaskDetail.CreateDate],
            Modifyer: [this.paintTaskDetail.Modifyer],
            ModifyDate: [this.paintTaskDetail.ModifyDate],
            //FK
            PaintTaskMasterId: [this.paintTaskDetail.PaintTaskMasterId],
            PaintTeamId: [this.paintTaskDetail.PaintTeamId,
                [
                    Validators.required
                ]
            ],
            PaintWorkItemId: [this.paintTaskDetail.PaintWorkItemId,
                [
                    Validators.required
                ]
            ],
            PaymentDetailId: [this.paintTaskDetail.PaymentDetailId,
                [
                    Validators.required
                ]
            ],
            //ViewModel
            PaintTeamString: [this.paintTaskDetail.PaintTeamString],
            PaintWorkItem: [this.paintTaskDetail.PaintWorkItem],
            isValid: [this.paintTaskDetail.isValid],
            CommonText: [this.paintTaskDetail.CommonText],
            SummaryActual: [this.paintTaskDetail.SummaryActual + " l."]
        });
        this.paintTaskDetailForm.valueChanges.subscribe((data: any) => this.onValueChanged(data));

        const ControlPro: AbstractControl | null = this.paintTaskDetailForm.get("TaskDetailProgress");
        if (ControlPro) {
            ControlPro.valueChanges.distinctUntilChanged().subscribe((Progress: number) => {
                const controlSD: AbstractControl | null = this.paintTaskDetailForm.get("ActualSDate");
                const controlED: AbstractControl | null = this.paintTaskDetailForm.get("ActualEDate");

                if (controlSD && controlED) {
                    if (Progress >= 100) {
                        if (!controlSD.value) {
                            this.patchGroupFormValue("s");
                        }
                        if (!controlED.value) {
                            this.patchGroupFormValue("e");
                        }
                    } else {
                        if (controlED.value) {
                            this.patchGroupFormValue("en");
                        }
                        if (Progress !== 0) {
                            if (!controlSD.value) {
                                this.patchGroupFormValue("s");
                            }
                        }
                    }
                }
            });
        }

        const ControlED: AbstractControl | null = this.paintTaskDetailForm.get("ActualEDate");
        if (ControlED) {
            ControlED.valueChanges.distinctUntilChanged().subscribe((EndDate: Date) => {
                const controlSD: AbstractControl | null = this.paintTaskDetailForm.get("ActualSDate");
                const controlPro: AbstractControl | null = this.paintTaskDetailForm.get("TaskDetailProgress");

                if (controlSD && controlPro) {
                    if (EndDate) {
                        this.patchGroupFormValue("p");
                        if (!controlSD.value) {
                            this.patchGroupFormValue("s", EndDate);
                        } else if (controlSD.value > EndDate) {
                            this.patchGroupFormValue("s", EndDate);
                        }
                    } else {
                        if (controlPro.value >= 100) {
                            if (!controlSD.value) {
                                this.patchGroupFormValue("pm");
                            } else {
                                this.patchGroupFormValue("pn");
                            }
                        }
                    }
                }
            });
        }

        const ControlSD: AbstractControl | null = this.paintTaskDetailForm.get("ActualSDate");
        if (ControlSD) {
            ControlSD.valueChanges.distinctUntilChanged().subscribe((StartDate: Date) => {
                if (!StartDate) {
                    this.paintTaskDetailForm.patchValue({
                        TaskDetailProgress: 0,
                        ActualEDate: undefined
                    });
                }
            });
        }
    }
    // on value of form change
    onValueChanged(data?: any): void {
        if (!this.paintTaskDetailForm) { return; }
        const form = this.paintTaskDetailForm;

        this.paintTaskDetail = form.value;
        this.paintTaskDetail.isValid = form.valid;

        this.hasChange.emit(this.paintTaskDetail.isValid);
    }
    // on patch Date
    patchGroupFormValue(mode: string, value?: Date): void {
        if (!this.paintTaskDetailForm) { return; }
        const form = this.paintTaskDetailForm;

        if (mode === "s") {
            if (value) {
                form.patchValue({
                    ActualSDate: value,
                });
            } else {
                form.patchValue({
                    ActualSDate: new Date,
                });
            }

        } else if (mode === "e") {
            if (value) {
                form.patchValue({
                    ActualEDate: value,
                });
            } else {
                form.patchValue({
                    ActualEDate: new Date,
                });
            }
        } else if (mode === "en") {
            form.patchValue({
                ActualEDate: undefined,
            });
        } else if (mode === "p") {
            form.patchValue({
                TaskDetailProgress: 100,
            });
        } else if (mode === "pn") {
            form.patchValue({
                TaskDetailProgress: this.minProgress,
            });
        } else if (mode === "pm") {
            form.patchValue({
                TaskDetailProgress: this.minProgress,
            });
        }
    }
    // open dialog
    openDialog(type?: string): void {
        if (type) {
            if (type === "SummaryActual") {
                if (!this.paintTaskDetail.PaintTaskDetailId) {
                    this.serviceDialogs.context("WarningMessage",
                        "Please save this task befor add requisition !!!", this.viewContainerRef);
                    return;
                }

                let temp: PaintTaskDetail = this.paintTaskDetailForm.value;
                this.serviceDialogs.dialogRequisitionListAddOrUpdate(this.viewContainerRef, temp)
                    .subscribe(Complate => {
                        if (Complate) {
                            setTimeout(() => {
                                this.servicePaintTaskDetail.getGetRequisitionSumByKeyNumber(temp.PaintTaskDetailId)
                                    .subscribe(data => {
                                        this.paintTaskDetailForm.patchValue({
                                            SummaryActual: data.TotalSummary + " l.",
                                        });
                                    });
                            }, 1500);
                        }
                    });
            }
        }
    }
    // on ShowReport
    showReportPaintMethod(): void {
        if (this.paintTaskDetail.PaintTaskDetailId) {
            this.showReportPaint.emit(this.paintTaskDetail.PaintTaskDetailId);
        }
    }
    // bug calendar not update min-max
    // update CakenderUi
    updateCalendarUI(calendar: Calendar) {
        calendar.updateUI();
    }
}