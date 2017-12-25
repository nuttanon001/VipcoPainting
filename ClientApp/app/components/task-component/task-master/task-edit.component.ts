// angular
import { Component, ViewContainerRef } from "@angular/core";
import { FormBuilder, FormControl, Validators, AbstractControl } from "@angular/forms";
// models
import {
    TaskMaster, TaskBlastDetail,
    TaskPaintDetail, RequirePaintList,
    RequirePaintMaster
} from "../../../models/model.index";
// components
import { BaseEditComponent } from "../../base-component/base-edit.component";
// 3rd party
import { SelectItem } from "primeng/primeng";
// services
import { AuthService } from "../../../services/auth/auth.service";
import { DialogsService } from "../../../services/dialog/dialogs.service";
import { TaskMasterService, TaskMasterServiceCommunicate } from "../../../services/task/task-master.service";
import { TaskBlastDetailService } from "../../../services/task/task-blast-detail.service";
import { TaskPaintDetailService } from "../../../services/task/task-paint-detail.service";
import { RequirePaintListService } from "../../../services/require-paint/require-paint-list.service";
import { PaintWorkitemService } from "../../../services/require-paint/paint-workitem.service";
import { BlastWorkitemService } from "../../../services/require-paint/blast-workitem.service";
import { RequirePaintMasterService } from "../../../services/require-paint/require-paint-master.service";

@Component({
    selector: "task-edit",
    templateUrl: "./task-edit.component.html",
    styleUrls: ["../../../styles/edit.style.scss"],
})
/** task-edit component*/
export class TaskEditComponent extends BaseEditComponent<TaskMaster, TaskMasterService> {
    /** task-edit ctor */
    constructor(
        service: TaskMasterService,
        serviceCom: TaskMasterServiceCommunicate,
        private serviceRequirePaintMaster: RequirePaintMasterService,
        private serviceRequirePaintList: RequirePaintListService,
        private serviceTaskPaint: TaskPaintDetailService,
        private serviceTaskBlast: TaskBlastDetailService,
        private servicePaintWork: PaintWorkitemService,
        private serviceBlastWork: BlastWorkitemService,
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
    requirePaintList: RequirePaintList;
    requirePaintMaster: RequirePaintMaster;

    maxDate: Date = new Date;
    // Show On/Off
    get ShowBlast(): boolean {
        if (this.editValue) {
            if (this.editValue.TaskBlastDetails) {
                if (this.editValue.TaskBlastDetails.length > 0) {
                    return true;
                }
            }
        }
        return false;
    }
    get ShowPaint(): boolean {
        if (this.editValue) {
            if (this.editValue.TaskPaintDetails) {
                if (this.editValue.TaskPaintDetails.length > 0) {
                    return true;
                }
            }
        }
        return false;
    }
    // on get data by key
    onGetDataByKey(value?: TaskMaster): void {
        if (value) {
            if (value.RequirePaintingListId) {
                this.serviceRequirePaintList.getOneKeyNumber(value.RequirePaintingListId)
                    .subscribe(dbRequirePaintList => {
                        this.requirePaintList = dbRequirePaintList;
                        // Get RequirePaintingMaster
                        if (this.requirePaintList.RequirePaintingMasterId) {
                            this.serviceRequirePaintMaster.getOneKeyNumber(this.requirePaintList.RequirePaintingMasterId)
                                .subscribe(dbRequirePaintingMaster => {
                                    this.requirePaintMaster = dbRequirePaintingMaster;
                                });
                        }
                    });
            }

            if (value.TaskMasterId) {
                this.service.getOneKeyNumber(value.TaskMasterId)
                    .subscribe(dbData => {
                        this.editValue = dbData;
                        // set Date
                        if (this.editValue.ActualEDate) {
                            this.editValue.ActualEDate = this.editValue.ActualEDate != null ?
                                new Date(this.editValue.ActualEDate) : new Date();
                        }

                        if (this.editValue.ActualSDate) {
                            this.editValue.ActualSDate = this.editValue.ActualSDate != null ?
                                new Date(this.editValue.ActualSDate) : new Date();
                        }
                        // Get TaskBlastDetails
                        this.serviceTaskBlast.getByMasterId(this.editValue.TaskMasterId)
                            .subscribe(dbTaskBlast => {
                                this.editValue.TaskBlastDetails = dbTaskBlast.slice();
                                this.editValueForm.patchValue({
                                    TaskBlastDetails: this.editValue.TaskBlastDetails,
                                });
                            });
                        // Get TaskPaintDetails
                        this.serviceTaskPaint.getByMasterId(this.editValue.TaskMasterId)
                            .subscribe(dbTaskPaint => {
                                this.editValue.TaskPaintDetails = dbTaskPaint.slice();
                                this.editValueForm.patchValue({
                                    TaskPaintDetails: this.editValue.TaskPaintDetails,
                                });
                            });

                    }, error => console.error(error), () => this.defineData());
            } else {
                this.editValue = {
                    TaskMasterId: 0,
                    TaskProgress: 0,
                    RequirePaintingListId: value.RequirePaintingListId,
                    AssignDate: new Date,
                    ActualSDate: new Date,
                };

                if (this.serviceAuth.getAuth) {
                    this.editValue.AssignBy = this.serviceAuth.getAuth.EmpCode || "";
                    this.editValue.AssignByString = this.serviceAuth.getAuth.NameThai || "";
                }

                if (this.editValue.RequirePaintingListId) {
                    this.onGetPaintWorkItemAndBlastWorkItem(this.editValue.RequirePaintingListId);
                }

                this.defineData();
            }
        }
    }

    // define data for edit form
    defineData(): void {
        this.buildForm();
    }

    // onGetPaintWorkItem
    onGetPaintWorkItemAndBlastWorkItem(MasterId: number): void {
        if (MasterId) {
            this.servicePaintWork.getByMasterId(MasterId)
                .subscribe(dbPaintWork => {
                    if (!this.editValue.TaskPaintDetails) {
                        this.editValue.TaskPaintDetails = new Array;
                    }

                    dbPaintWork.forEach(item => {
                        let newTaskPaint: TaskPaintDetail = {
                            TaskPaintDetailId: 0,
                            PaintWorkItemId: item.PaintWorkItemId,
                            PaintWorkItem: item,
                            PaintTeamId: 1,
                        };
                        if (this.editValue.TaskPaintDetails) {
                            this.editValue.TaskPaintDetails.push(newTaskPaint);
                        }
                    });

                    this.editValueForm.patchValue({
                        TaskPaintDetails: this.editValue.TaskPaintDetails,
                    });
                });

            this.serviceBlastWork.getByMasterId(MasterId)
                .subscribe(dbBlastWork => {
                    if (!this.editValue.TaskBlastDetails) {
                        this.editValue.TaskBlastDetails = new Array;
                    }

                    dbBlastWork.forEach(item => {
                        let newTaskBlast: TaskBlastDetail = {
                            TaskBlastDetailId: 0,
                            BlastWorkItemId: item.BlastWorkItemId,
                            BlastWorkItem: item,
                            BlastRoomId: 1,
                        };
                        if (this.editValue.TaskBlastDetails) {
                            this.editValue.TaskBlastDetails.push(newTaskBlast);
                        }
                    });
                    this.editValueForm.patchValue({
                        TaskBlastDetails: this.editValue.TaskBlastDetails,
                    });
                });
        }
    }

    // build form
    buildForm(): void {
        this.editValueForm = this.fb.group({
            TaskMasterId: [this.editValue.TaskMasterId],
            TaskNo: [this.editValue.TaskNo],
            AssignDate: [this.editValue.AssignDate],
            AssignBy: [this.editValue.AssignBy],
            ActualSDate: [this.editValue.ActualSDate,
                [
                    Validators.required
                ]
            ],
            ActualEDate: [this.editValue.ActualEDate],
            TaskProgress: [this.editValue.TaskProgress,
                [
                    Validators.minLength(0),
                    Validators.maxLength(100),
                ]
            ],
            TaskStatus: [this.editValue.TaskStatus],
            Creator: [this.editValue.Creator],
            CreateDate: [this.editValue.CreateDate],
            Modifyer: [this.editValue.Modifyer],
            ModifyDate: [this.editValue.ModifyDate],
            //FK
            RequirePaintingListId: [this.editValue.RequirePaintingListId,
                [
                    Validators.required
                ]
            ],
            TaskBlastDetails: [this.editValue.TaskBlastDetails],
            TaskPaintDetails: [this.editValue.TaskPaintDetails],
            //ViewModel
            AssignByString: [this.editValue.AssignByString],
            // ProjectCodeSubString: [this.editValue.ProjectCodeSubString],
        });

        this.editValueForm.valueChanges.subscribe((data: any) => this.onValueChanged(data));
        // this.onValueChanged();

        const ControlPro: AbstractControl | null = this.editValueForm.get("TaskProgress");
        if (ControlPro) {
            ControlPro.valueChanges.subscribe((Progress: number) => {
                console.log("TaskProgress");
                const controlSD: AbstractControl | null = this.editValueForm.get("ActualSDate");
                const controlED: AbstractControl | null = this.editValueForm.get("ActualEDate");

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
                        if (Progress !== 0){
                            if (!controlSD.value) {
                                this.patchGroupFormValue("s");
                            }
                        }
                    }
                }
            });
        }

        const ControlED: AbstractControl | null = this.editValueForm.get("ActualEDate");
        if (ControlED) {
            ControlED.valueChanges.subscribe((EndDate: Date) => {
                console.log("ActualEDate");
                const controlSD: AbstractControl | null = this.editValueForm.get("ActualSDate");
                const controlPro: AbstractControl | null = this.editValueForm.get("TaskProgress");

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

        const ControlSD: AbstractControl | null = this.editValueForm.get("ActualSDate");
        if (ControlSD) {
            ControlSD.valueChanges.subscribe((StartDate: Date) => {
                console.log("ActualSDate");
                if (!StartDate) {
                    this.editValueForm.patchValue({
                        TaskProgress: 0,
                        ActualEDate: undefined
                    });
                }
            });
        }
    }

    // on patch Date
    patchGroupFormValue(mode: string,value?:Date): void {
        if (!this.editValueForm) { return; }
        const form = this.editValueForm;

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
                TaskProgress: 100,
            });
        } else if (mode === "pn") {
            form.patchValue({
                TaskProgress: 90,
            });
        } else if (mode === "pm") {
            form.patchValue({
                TaskProgress: 0,
            });
        }
    }

    //OVERRIDE
    // on valid data 
    onFormValid(isValid: boolean): void {
        //let temp: any = this.editValue.TaskBlastDetails ? this.editValue.TaskBlastDetails.slice() : undefined;
        //let temp2: any = this.editValue.TaskPaintDetails ? this.editValue.TaskPaintDetails.slice() : undefined;

        this.editValue = this.editValueForm.value;

        //this.editValue.TaskBlastDetails = temp ? temp.slice() : undefined;
        //this.editValue.TaskPaintDetails = temp2 ? temp2.slice() : undefined;

        this.communicateService.toParent([this.editValue, isValid]);
    }

    needPatchValue(data?:any): void {
        this.editValueForm.patchValue({
            TaskBlastDetails: this.editValue.TaskBlastDetails,
            TaskPaintDetails: this.editValue.TaskPaintDetails,
        });
    }
}