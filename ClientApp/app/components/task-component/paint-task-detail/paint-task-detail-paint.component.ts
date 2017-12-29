﻿// angular
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
        private servicePaintWork: PaintWorkitemService,
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
    // FormGroup
    paintTaskDetailForm: FormGroup;
    minProgress: number;
    maxDate: Date = new Date();
    // ComboBox
    paintTeams: Array<SelectItem>;
    // OnInit
    ngOnInit(): void {
        this.buildForm();
        this.getPaintTeamCombobox();
    }
    // get PaintTeam Array
    getPaintTeamCombobox(): void {
        if (!this.paintTeams) {
            // paintTeam ComboBox
            this.service.getAll()
                .subscribe(dbPatinTeam => {
                    this.paintTeams = new Array;
                    for (let item of dbPatinTeam) {
                        this.paintTeams.push({ label: `${(item.TeamName || "")}`, value: item.PaintTeamId });
                    }
                }, error => console.error(error));
        }
    }
    // build Form
    buildForm(): void {
        this.minProgress = this.paintTaskDetail.TaskDetailProgress || 0;
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
            //ViewModel
            PaintTeamString: [this.paintTaskDetail.PaintTeamString],
            PaintWorkItem: [this.paintTaskDetail.PaintWorkItem],
        });
        this.paintTaskDetailForm.valueChanges.subscribe((data: any) => this.onValueChanged(data));

        const ControlPro: AbstractControl | null = this.paintTaskDetailForm.get("TaskDetailProgress");
        if (ControlPro) {
            ControlPro.valueChanges.subscribe((Progress: number) => {
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
                        if (Progress > 100) {
                            this.patchGroupFormValue("p");
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
            ControlED.valueChanges.subscribe((EndDate: Date) => {
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
            ControlSD.valueChanges.subscribe((StartDate: Date) => {
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
        this.hasChange.emit(form.valid);
        if (form.valid) {
            this.paintTaskDetail = form.value;
        }
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
}